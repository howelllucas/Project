// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/EZ/Utility/Fresnel" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		[Toggle]_InvertFresnel ("Invert Fresnel Color", float) = 0
		[Toggle]_ZWrite ("ZWrite On", float) = 0
		_FresnelRange ("Fresnel Range", Range(0, 5)) = 1
		_FresnelIntensity ("Fresnel Intensity", Range(0, 5)) = 1
		_AlphaBlend ("Alpha Blend [0, 1]", Range(0, 1)) = 0.0
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "LightMode" = "Always" }
		Fog { Mode Off }
		LOD 100
		
		Pass {
			Name "FRESNEL"
			ZWrite [_ZWrite]
			Lighting Off
			Blend One OneMinusSrcAlpha
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _INVERTFRESNEL_ON
			#include "UnityCG.cginc"
			
			fixed4 _Color;
			fixed _FresnelIntensity;
			fixed _AlphaBlend;
			float _FresnelRange;
			
			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				fixed4 color : COLOR0;
			};
			
			v2f vert (appdata v)
			{
				v2f o;				
				o.pos = UnityObjectToClipPos(v.vertex);
				float3 normalDir = UnityObjectToWorldNormal(v.normal);
				float3 viewDir = normalize(WorldSpaceViewDir(v.vertex));
			#ifdef _INVERTFRESNEL_ON
				float nv = max(0.0, dot(normalDir, viewDir));
			#else
				float nv = 1.0 - max(0.0, dot(normalDir, viewDir));
			#endif
				float fresnel = pow(nv, _FresnelRange);
				o.color = v.color;
				o.color.a *= _Color.a * fresnel;
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 tintColor = fixed4(_Color.rgb, i.color.a);
				tintColor *= tintColor * _FresnelIntensity;
				tintColor.rgb *= tintColor.a;
				tintColor.a *= _AlphaBlend;
				return tintColor;
			}

			ENDCG
		}
	} 
	FallBack Off
}
