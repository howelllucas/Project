// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Inu/Scene/Diffuse_Shadow" {
    Properties {
        _Color("Main Color", Color) = (1,1,1,1)
        _Power ("Power", Float ) = 1
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader 
    {
		Tags { "RenderType"="Opaque" }
		LOD 100
//#surface-start
//#		CGPROGRAM
//#		#pragma surface surf Lambert 
//#
//#		sampler2D _MainTex;
//#		fixed4 _Color;
//#		fixed _Power;
//#		struct Input {
//#			float2 uv_MainTex;
//#		};
//#
//#		void surf (Input IN, inout SurfaceOutput o) {
//#			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
//#			o.Albedo = c.rgb * _Color.rgb * _Power;
//#			o.Alpha = c.a;
//#		}
//#		ENDCG
//#surface-end

//#vertfrag-start
        Pass
        {
        	Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "DlInuScene.cginc"
			// make fog work
			#pragma multi_compile_fog
			#pragma multi_compile SHADOWS_ON SHADOWS_SCREEN
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
				UNITY_FOG_COORDS(2)
				float4 pos : SV_POSITION;
			    #ifndef LIGHTMAP_OFF
		 			half2 lmap : TEXCOORD4;
					#ifdef SHADOWS_SCREEN
						SHADOW_COORDS(5)
					#endif
				#endif
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			fixed _Power;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.pos);
				#ifndef LIGHTMAP_OFF
		 			o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
					#ifdef SHADOWS_SCREEN
						TRANSFER_SHADOW(o);
					#endif
				#endif
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv)*_Color*_Power;
				#ifndef LIGHTMAP_OFF
		 			fixed4 bakedColorTex = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap.xy);
		 			half3 bakedColor = DecodeLightmap(bakedColorTex);
		 			col.rgb *= bakedColor;
					#ifdef SHADOWS_SCREEN
						col.rgb *= CaculateShadowColor(SHADOW_ATTENUATION(i));
						//col *= min(atten + max((1.6-bakedColor.r), 0), 1);
					#endif
				#endif
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
//#vertfrag-end
	}
	FallBack "TF/Mobile/VertexLit"
}
