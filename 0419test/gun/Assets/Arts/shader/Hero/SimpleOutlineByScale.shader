Shader "Hidden/EZ/Utility/SimpleOutlineByScale"
{	
	SubShader
	{ 
		Tags { "RenderType" = "Opaque" "LightMode" = "ForwardBase"}
		LOD 100
		Pass {
			Name"OUTLINESCALE"
			Cull Front
			ZWrite Off
			//ZTest Always
			ColorMask RGB
			Offset 10,10

			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert_outline
			#pragma fragment frag_outline

			uniform float _Outline;
			uniform fixed4 _OutlineColor;

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				fixed4 color : COLOR;
			};

			struct v2f_outline {
				float4 pos : POSITION;
			};

			v2f_outline vert_outline(appdata v) {
				v2f_outline o;
				v.vertex.xyz *= (_Outline + 1.0f);
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag_outline(v2f_outline i) : COLOR
			{
				return _OutlineColor;
			}

		ENDCG
		}
	}
}
