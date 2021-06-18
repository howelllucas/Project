// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "SS/Scene/Unlit Reflect Bump"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_ReflectMap ("Reflect Map (RGB)", 2D) = "black" {}
		_ReflectLevel ("Reflect Level", float) = 0.5
		_ReflectBumpLevel ("Reflect Bump Level", Range (0, 1)) = 0.5
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Tags { "LightMode" = "ForwardBase" }
			Fog {Mode Off}
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _BumpMap;
			sampler2D _ReflectMap;
			half4 _MainTex_ST;
			half4 _ReflectMap_ST;
			fixed _ReflectLevel;
			fixed _ReflectBumpLevel;
			
			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
				half2 uv2 : TEXCOORD1;
			#ifndef LIGHTMAP_OFF
				half2 lmap : TEXCOORD2;
			#endif
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				float2 tmp = o.pos.xy / o.pos.w;
				tmp = tmp.xy * 0.5 + 0.5;
				o.uv2 = tmp.xy * _ReflectMap_ST.xy + _ReflectMap_ST.zw;
			#ifndef LIGHTMAP_OFF
				o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
			#endif
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 texcol = tex2D(_MainTex, i.uv);
				fixed3 bump = tex2D(_BumpMap, i.uv).rgb * 2 - 1;
				fixed4 refl = tex2D(_ReflectMap, i.uv2 + bump.rg * _ReflectBumpLevel);
				texcol += refl * _ReflectLevel;
			#ifndef LIGHTMAP_OFF
				fixed4 bakedColorTex = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap.xy);
				half3 bakedColor = DecodeLightmap(bakedColorTex);
				texcol.rgb *= bakedColor;
			#endif
				return texcol;
			}
			ENDCG
		}
	}
	FallBack Off
}
