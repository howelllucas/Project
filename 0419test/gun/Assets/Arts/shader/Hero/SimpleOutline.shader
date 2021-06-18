Shader "Hidden/EZ/Utility/SimpleOutline"
{	
	SubShader
	{ 
		Tags { "RenderType" = "Opaque" "LightMode" = "ForwardBase"}
		LOD 100
		Pass {
			Name"OUTLINE"
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
				o.pos = UnityObjectToClipPos(v.vertex);

				//float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);

				float3 normal = UnityObjectToWorldNormal(v.normal);
				float2 offset = mul(UNITY_MATRIX_VP, float4(normal, 0)).xy;;
				o.pos.xy += offset * _Outline * v.color.a;

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
