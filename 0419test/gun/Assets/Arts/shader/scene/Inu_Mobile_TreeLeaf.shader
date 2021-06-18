Shader "Inu/Scene/Tree_Leaf" {
Properties {
    _Color("Main Color",Color)=(1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Pos("pos center",Vector) =(1000,0,1000,0)
	_x ("x", Range(0,1)) = 1
	_z ("z", Range(0,1)) = 1

	_ColorPower("Power", Float) = 1
	_Desaturate("Desaturate", Range(0, 1)) = 0
	_WindWave("WindWave", range(0.1,1)) = 1
	_WindPower("WindPower", range(0.1,1)) = .15 

	_Alpha("Alpha", 2D) = "white" {}	
}

SubShader {
	Tags { "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 100
	cull Off
//	ZWrite Off
//	Blend SrcAlpha OneMinusSrcAlpha 
	Pass {  
		CGPROGRAM
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it uses non-square matrices
			//#pragma exclude_renderers gles
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase SHADOWS_OFF SHADOWS_SCREEN 
			#include "UnityCG.cginc"

			struct appdata_t {
				fixed4 vertex : POSITION;
				fixed2 texcoord : TEXCOORD0;
				fixed2 texcoord1 : TEXCOORD1;
			};

			struct v2f {
				fixed4 vertex : SV_POSITION;
				fixed2 texcoord : TEXCOORD0;
				#ifndef LIGHTMAP_OFF
				fixed2 uvLM : TEXCOORD1;
				#endif 
				UNITY_FOG_COORDS(2)
			};

			sampler2D _MainTex;
			sampler2D _Alpha;
			fixed4 _MainTex_ST;
			fixed4 _Pos;
			fixed _x;
			fixed _z;
			fixed _WindWave;
			fixed _WindPower;
			fixed4 _Color;
			uniform fixed _Desaturate;
			uniform fixed _ColorPower;
			uniform fixed _Global_Power;
			uniform fixed4 _Global_Black_Color;

			v2f vert (appdata_t v)
			{
				v2f o;
				fixed dis = sin(distance (v.vertex.xyz , _Pos.xyz) * _WindWave + _Time.y * 10 * _WindPower) ;
				v.vertex.xyz += fixed3(_x,  0, _z) * dis * 0.1;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				#ifndef LIGHTMAP_OFF
					o.uvLM = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 colmap =tex2D(_MainTex, i.texcoord) *_ColorPower;
				colmap.a = colmap.a*tex2D(_Alpha, i.texcoord).r;
				clip(colmap.a - .5);

				colmap.rgb = lerp(colmap, dot(colmap.rgb, float3(0.3, 0.59, 0.11)), _Desaturate);

				#ifndef LIGHTMAP_OFF
					fixed3 lm = DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uvLM.xy));		
					colmap.rgb *= lm;
				#endif

				UNITY_APPLY_FOG(i.fogCoord, colmap);
				return colmap * (1-_Global_Power) + _Global_Power *colmap*_Global_Black_Color;
			}
		ENDCG
	}
}

}






















