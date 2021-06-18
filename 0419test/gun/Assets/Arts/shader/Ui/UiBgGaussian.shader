Shader "EZ/UI/UiBgGaussian"
{
		Properties
		{
			[PerRendererData]_StencilComp("Stencil Comparison", Float) = 8
			[PerRendererData]_Stencil("Stencil ID", Float) = 0
			[PerRendererData]_StencilOp("Stencil Operation", Float) = 0
			[PerRendererData]_StencilWriteMask("Stencil Write Mask", Float) = 255
			[PerRendererData]_StencilReadMask("Stencil Read Mask", Float) = 255
			[PerRendererData]_ColorMask("Color Mask", Float) = 15
			_GaussianRadio("GaussianRadio",float) = 1
		}

			SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Stencil
			{
				Ref[_Stencil]
				Comp[_StencilComp]
				Pass[_StencilOp]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
			}

			Cull Off
			Lighting Off
			ZWrite Off
			ZTest[unity_GUIZTestMode]
			Fog { Mode Off }
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask[_ColorMask]
			GrabPass{}
			Pass
			{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					half2 texcoord  : TEXCOORD0;
				};

				float _GaussianRadio;
				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = IN.texcoord;
	#ifdef UNITY_HALF_TEXEL_OFFSET
					OUT.vertex.xy += (_ScreenParams.zw - 1.0)*float2(-1,1);
	#endif
					OUT.color = IN.color;
					return OUT;
				}


				sampler2D _GrabTexture;
				fixed4 frag(v2f IN) : SV_Target
				{
					float widthDt = 1.0 / _ScreenParams.x;
					float heightDt = 1.0 / _ScreenParams.y;
					half4 color0 = tex2D(_GrabTexture, IN.texcoord) * 0.16;;
					half4 color1 = tex2D(_GrabTexture, IN.texcoord + float2(_GaussianRadio * widthDt,_GaussianRadio * heightDt)) * 0.12;;
					half4 color2 = tex2D(_GrabTexture, IN.texcoord + float2(widthDt * _GaussianRadio,-heightDt * _GaussianRadio)) * 0.12;;
					half4 color3 = tex2D(_GrabTexture, IN.texcoord + float2(-widthDt * _GaussianRadio,-heightDt * _GaussianRadio)) * 0.12;;
					half4 color4 = tex2D(_GrabTexture, IN.texcoord + float2(-widthDt * _GaussianRadio,heightDt * _GaussianRadio)) * 0.12;;
					half4 color5 = tex2D(_GrabTexture, IN.texcoord + float2(_GaussianRadio * widthDt,0)) * 0.09;
					half4 color6 = tex2D(_GrabTexture, IN.texcoord + float2(0,-heightDt * _GaussianRadio)) * 0.09;
					half4 color7 = tex2D(_GrabTexture, IN.texcoord + float2(-widthDt * _GaussianRadio,0)) * 0.09;
					half4 color8 = tex2D(_GrabTexture, IN.texcoord + float2(0,heightDt *_GaussianRadio)) * 0.09;
					half4 finalColor = color0 + color1 + color2 + color3 + color4 + color5 + color6 + color7 + color8;

					//half4 finalColor = color0 + color1 + color2 + color3 + color4;

					return finalColor * IN.color;
				}
			ENDCG
			}
		}
	}
