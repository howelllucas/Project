Shader "EZ/UI/UiGaussian"
{
	Properties
	{
		[PerRendererData]_StencilComp("Stencil Comparison", Float) = 8
		[PerRendererData]_Stencil("Stencil ID", Float) = 0
		[PerRendererData]_StencilOp("Stencil Operation", Float) = 0
		[PerRendererData]_StencilWriteMask("Stencil Write Mask", Float) = 255
		[PerRendererData]_StencilReadMask("Stencil Read Mask", Float) = 255
		[PerRendererData]_ColorMask("Color Mask", Float) = 15
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[PerRendererData] _Color("Tint", Color) = (1,1,1,1)
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
		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLI
			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			float _GaussianRadio;
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				OUT.worldPosition = IN.vertex;

				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
				OUT.texcoord = IN.texcoord;
#ifdef UNITY_HALF_TEXEL_OFFSET
				OUT.vertex.xy += (_ScreenParams.zw - 1.0)*float2(-1,1);
#endif
				OUT.color = IN.color;
				return OUT;
			}


			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			float4 _ClipRect;
			fixed4 frag(v2f IN) : SV_Target
			{
				float widthDt =  _MainTex_TexelSize.x;
				float heightDt = _MainTex_TexelSize.y;
				float radio = _GaussianRadio * IN.color.a;
				half4 color0 = tex2D(_MainTex, IN.texcoord) * 0.16;
				half4 color1 = tex2D(_MainTex, IN.texcoord + float2(radio * widthDt, radio * heightDt)) * 0.12;
				half4 color2 = tex2D(_MainTex, IN.texcoord + float2(widthDt * radio,-heightDt * radio)) * 0.12;
				half4 color3 = tex2D(_MainTex, IN.texcoord + float2(-widthDt * radio,-heightDt * radio)) * 0.12;
				half4 color4 = tex2D(_MainTex, IN.texcoord + float2(-widthDt * radio,heightDt * radio)) * 0.12;
				half4 color5 = tex2D(_MainTex, IN.texcoord + float2(radio * widthDt,0)) * 0.09;
				half4 color6 = tex2D(_MainTex, IN.texcoord + float2(0,-heightDt * radio)) * 0.09;
				half4 color7 = tex2D(_MainTex, IN.texcoord + float2(-widthDt * radio,0)) * 0.09;
				half4 color8 = tex2D(_MainTex, IN.texcoord + float2(0,heightDt *radio)) * 0.09;
				half4 finalColor = color0 + color1 + color2 + color3 + color4 + color5 + color6 + color7 + color8;
#ifdef UNITY_UI_CLIP_RECT
				finalColor.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
#endif
#ifdef UNITY_UI_ALPHACLIP
				clip(finalColor.a - 0.001);
#endif
				return finalColor * fixed4(IN.color.rgb,1);
			}
		ENDCG
		}
	}
}
