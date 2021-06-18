// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "EZ/Particles/Additive_UV_Anim" {
	Properties {
		_MainTex ("Particle Texture", 2D) = "white" {}
		_Color("Main Color", Color) = (1,1,1,1)
		_SpeedX ("UV Offset X", float) = 0
		_SpeedY ("UV Offset Y", float) = 0
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Fog { Mode Off }
		LOD 100
		
		Pass {
			Blend SrcAlpha One
			Lighting Off
			Cull Off
			ZWrite Off
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;

			float _SpeedX;
			float _SpeedY;

			struct appdata {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				float2 offset = frac(float2(_SpeedX, _SpeedY) * _Time.y);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex) + offset;
				o.color = v.color;
				return o;
			}

			fixed4 frag (v2f i) : COLOR
			{
				fixed4 texcol = tex2D(_MainTex, i.uv) * i.color * _Color;
				return texcol;
			}

			ENDCG
		}
	} 
	FallBack Off
}
