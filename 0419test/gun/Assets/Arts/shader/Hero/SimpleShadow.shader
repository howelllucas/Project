Shader "Hidden/EZ/SimplerShadowCaster"
{
	SubShader
	{
		 Pass
		{
			Name "SIMPLERSHADOWCASTER"
			Tags { "LightMode" = "ShadowCaster" }
       
			 ZWrite On ZTest Less Cull Off
     
			CGPROGRAM
			#pragma multi_compile_instancing
    		#pragma vertex vert
    		#pragma fragment frag
    		#pragma multi_compile_shadowcaster
    		#include "UnityCG.cginc"

    		struct v2f { 
    			V2F_SHADOW_CASTER;
				UNITY_VERTEX_INPUT_INSTANCE_ID
    		};

    		v2f vert (appdata_base v) {
    			v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
    			TRANSFER_SHADOW_CASTER(o)
    			return o;
    		}

    		float4 frag (v2f i) : COLOR {
				UNITY_SETUP_INSTANCE_ID(i);
    			SHADOW_CASTER_FRAGMENT(i)
    		}
    		ENDCG
		}
	}
}
