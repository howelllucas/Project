// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Inu/Scene/BackGround" {
    Properties {
		_MainTex ("MainTex", 2D) = "white" {}
        _SecondTex("SecondTex", 2D) = "white" {}
		_Color ("Color", Color) = (0.5,0.5,0.5,1)
		_Colorpower ("Colorpower", Float) = 2
		_Alpha ("Alpha", 2D) = "white" {}
		_Alphabias ("Alphabias", Range(0,1)) = 0
		[HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="Transparent-10"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Fog {Mode Off}
			LOD 200
		    Cull Back
		    ZWrite On
		    alphatest Greater .01
		    ZTest LEqual

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"

            sampler2D _MainTex; 
            fixed4 _MainTex_ST;
            sampler2D _SecondTex; 
            sampler2D _Alpha;
		    fixed4  _Color;
		    fixed _Colorpower;
		    fixed _Alphabias;
            uniform fixed _LerpValue;
			uniform fixed _Global_Power;
			uniform fixed4 _Global_Black_Color;

            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = TRANSFORM_TEX(v.texcoord0, _MainTex);
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                fixed3 mainCol = tex2D (_MainTex, i.uv0).rgb;
                fixed3 secondCol = tex2D (_SecondTex, i.uv0).rgb;
                fixed a = tex2D (_Alpha, i.uv0).r - _Alphabias;
                clip(a - 0.5);
                fixed3 finalColor = lerp(mainCol, secondCol, _LerpValue) * _Color.rgb * _Colorpower;

				finalColor = finalColor*lerp(fixed4(1,1,1,1), _Global_Black_Color, _Global_Power);
                return fixed4(finalColor,a);
            }
            ENDCG
        }
    }
}
