// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "EZ/Scene/Diffuse" 
{
	Properties {
		_Color("Main Color", Color) = (1,1,1,1)
		_Power ("Power", Float ) = 1
		_LightRange ("Range", Range(0,1)) = 1
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LightMap ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 100
		Pass
		{
			Tags { "RenderType"="Opaque" }
			LOD 100
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			// make fog work
			#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
				#ifndef LIGHTMAP_OFF
		 			half2 lmap : TEXCOORD4;
				#endif
			};

			sampler2D _MainTex;
			sampler2D _LightMap;
			float4 _MainTex_ST;
			fixed4 _Color;
			fixed _Power;
			fixed _LightRange;
			uniform fixed _Global_Power;
			uniform fixed4 _Global_Black_Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				#ifndef LIGHTMAP_OFF
		 			o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv)*_Color*_Power;
				#ifndef LIGHTMAP_OFF
		 			fixed4 bakedColorTex = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap.xy);
		 			half3 bakedColor = DecodeLightmap(bakedColorTex);
					fixed4 bakedColorTex2 = tex2D(_LightMap, i.lmap.xy);
					half3 bakedColor2 = DecodeLightmap(bakedColorTex2);
					bakedColor = bakedColor * _LightRange + bakedColor2 * (1.0 - _LightRange);
					col.rgb *= bakedColor;
				#endif
				return col ;
			}
			ENDCG
		}
	}
	Fallback "Inu/Scene/VertexLit"
}
