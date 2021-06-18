// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Unlit shader. Simplest possible textured shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "EZ/Unlit/TankLvDai" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_SpeedLvDai("SpeedLvDai", float) = 1
		_Speed("Speed",float) = 0.0
	}

		SubShader{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent-10" }
			LOD 100
			Blend SrcAlpha OneMinusSrcAlpha

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
					float _SpeedLvDai;
					float _Speed;

					v2f vert(appdata_t v)
					{
						v2f o;
						UNITY_SETUP_INSTANCE_ID(v);
						UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
						o.vertex = UnityObjectToClipPos(v.vertex);
						o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
						o.texcoord.x += _Time.x * _SpeedLvDai;
						UNITY_TRANSFER_FOG(o,o.vertex);
						return o;
					}

					fixed4 frag(v2f i) : SV_Target
					{
						fixed4 col = tex2D(_MainTex, i.texcoord);
						UNITY_APPLY_FOG(i.fogCoord, col);
						UNITY_OPAQUE_ALPHA(col.a);
						float4 newColor = col;
						if (_Speed > 0.0f) {
							newColor.a = sin(_Time.y * _Speed);
						}
						return newColor;
					}
				ENDCG
			}
			UsePass "Hidden/EZ/SimplerShadowCaster/SIMPLERSHADOWCASTER"

	}

}
