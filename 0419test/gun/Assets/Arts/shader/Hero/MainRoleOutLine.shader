// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Unlit shader. Simplest possible textured shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "EZ/Unlit/MainRoleOutLine" {
	Properties{
		_MainTex("Main Texture", 2D) = "white" {}
		_Color("Main Color", Color) = (1,1,1,1)
		[Toggle]_InvertFresnel("Invert Fresnel Color", float) = 0
		_FresnelRange("Fresnel Range", Range(0, 5)) = 1
		_FresnelIntensity("Fresnel Intensity", Range(0, 5)) = 1
		_AlphaBlend("Alpha Blend [0, 1]", Range(0, 1)) = 0.
	}

		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 100
			//UsePass "Hidden/EZ/Utility/SimpleOutline/OUTLINE"
			Pass {
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma target 2.0
					#pragma multi_compile_fog

					#include "UnityCG.cginc"

					struct appdata_t {
						float4 vertex : POSITION;
						float2 texcoord : TEXCOORD0;
						UNITY_VERTEX_INPUT_INSTANCE_ID
					};

					struct v2f {
						float4 vertex : SV_POSITION;
						float2 texcoord : TEXCOORD0;
						UNITY_FOG_COORDS(1)
						UNITY_VERTEX_OUTPUT_STEREO
					};

					sampler2D _MainTex;
					float4 _MainTex_ST;

					v2f vert(appdata_t v)
					{
						v2f o;
						UNITY_SETUP_INSTANCE_ID(v);
						UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
						o.vertex = UnityObjectToClipPos(v.vertex);
						o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
						UNITY_TRANSFER_FOG(o,o.vertex);
						return o;
					}

					fixed4 frag(v2f i) : SV_Target
					{
						fixed4 col = tex2D(_MainTex, i.texcoord);
						UNITY_APPLY_FOG(i.fogCoord, col);
						UNITY_OPAQUE_ALPHA(col.a);
						return col;
					}
				ENDCG
			}
			UsePass "Hidden/EZ/Utility/Fresnel/FRESNEL"
			UsePass "Hidden/EZ/SimplerShadowCaster/SIMPLERSHADOWCASTER"

	}

}
