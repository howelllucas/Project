// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Unlit shader. Simplest possible textured shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "EZ/Unlit/MainRole" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}

		CGINCLUDE

	#include "UnityCG.cginc"

	struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		float2 texcoord : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;

	v2f vert(appdata_t v)
	{
		v2f o;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_TRANSFER_INSTANCE_ID(v, o);
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		UNITY_SETUP_INSTANCE_ID(i);
		fixed4 col = tex2D(_MainTex, i.texcoord);
		return col;
	}
		ENDCG

		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass {
				CGPROGRAM
				#pragma multi_compile_instancing
				#pragma vertex vert
				#pragma fragment frag
				ENDCG
			}
			Pass
			{
				Tags { "LightMode" = "ShadowCaster" }

				 ZWrite On ZTest Less Cull Off

				CGPROGRAM
				#pragma multi_compile_instancing
				#pragma multi_compile_shadowcaster
				#pragma vertex vert
				#pragma fragment frag
				ENDCG
		}
	}
}
