Shader "EZ/Scene/Inu_SceneReceiveShadow"
{
		Properties{
			_Power("Power", Float) = 1
		}
			SubShader
		{
			LOD 100
					Pass
					{
						Tags 
						{ 
								"LightMode" = "ForwardBase" 
								"RenderType" = "Transparent"
								"Queue" = "Transparent"
								"IgnoreProjector" = "True"
						}
					ZWrite Off
					Blend Zero SrcColor 
						CGPROGRAM
						#pragma vertex vert
						#pragma fragment frag
						#include "UnityCG.cginc"
						#include "AutoLight.cginc"
						#include "DlInuScene.cginc"
			// make fog work
			#pragma multi_compile SHADOWS_ON SHADOWS_SCREEN

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
#ifdef SHADOWS_SCREEN
				SHADOW_COORDS(5)
#endif
			};

			fixed _Power;

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
#ifdef SHADOWS_SCREEN
				TRANSFER_SHADOW(o);
#endif
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = fixed4(1,1,1,1);
#ifdef SHADOWS_SCREEN
			//col.rgb = CaculateShadowColor(SHADOW_ATTENUATION(i));
			float atten = SHADOW_ATTENUATION(i);
			col = 1 - saturate(1 - atten)* unity_ShadowColor;
#endif
				return col;
			}
			ENDCG
		}
		UsePass "Hidden/EZ/SimplerShadowCaster/SIMPLERSHADOWCASTER"
			//#vertfrag-end
		}
			FallBack Off
	}

