//透明
//接受光照
//自身亮度
///切换至vertfrag
//支持lightmap读取
Shader "EZ/Scene/TransparentCutOffDiffuse" {
    Properties {
		_LightRange("Range", Range(0,1)) = 1
		_MainTex("Base (RGB)", 2D) = "white" {}
		_LightMap("Base (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Colorpower ("Colorpower", Float ) = 1
        _Alpha ("Alpha", 2D) = "white" {}
        _Clipbias ("Clipbias", Range(0, 0.5)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }

    SubShader {
        Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
        LOD 200
        Pass{
        CGPROGRAM  
   
        #pragma vertex vert    
        #pragma fragment frag
        #include "UnityCG.cginc"

			#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON

        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
            float2 texcoord1 : TEXCOORD1;
        };

        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 pos : SV_POSITION;
            #ifndef LIGHTMAP_OFF
                half2 lmap : TEXCOORD4;
            #endif
        };
		fixed _LightRange;
		sampler2D _LightMap;
        sampler2D _MainTex;
        float4 _MainTex_ST;
        fixed4 _Color;
        fixed _Colorpower;
        sampler2D _Alpha;
        float4 _Alpha_ST;
        fixed _Clipbias; 
        uniform fixed _LerpValue;
		uniform fixed _Global_Power;
        uniform fixed4 _Global_Black_Color;
        uniform fixed4 _Global_Conversion_Color;

        struct vertexInput {  
            float4 vertex : POSITION;  
            float4 texcoord : TEXCOORD0;  
        };  
        struct vertexOutput {  
            float4 pos : SV_POSITION;  
            float4 tex : TEXCOORD0;  
        };  

        v2f vert (appdata v)
        {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            #ifndef LIGHTMAP_OFF
                o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
            #endif
            return o;
        }
        fixed4 frag (v2f i) : SV_Target
        {
            fixed4 col = tex2D(_MainTex, i.uv)*_Color*lerp(fixed4(1,1,1,1),_Global_Conversion_Color,_LerpValue);
            fixed4 alpha = tex2D(_Alpha, i.uv);
            clip(saturate((alpha.r-_Clipbias)) - 0.5);
            col.rgb = col.rgb * _Colorpower;
		#ifndef LIGHTMAP_OFF
			fixed4 bakedColorTex = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap.xy);
			half3 bakedColor = DecodeLightmap(bakedColorTex);
			fixed4 bakedColorTex2 = tex2D(_LightMap, i.lmap.xy);
			half3 bakedColor2 = DecodeLightmap(bakedColorTex2);
			bakedColor = bakedColor * _LightRange + bakedColor2 * (1.0 - _LightRange);
			col.rgb *= bakedColor;
		#endif
            // apply fog
            return col*lerp(fixed4(1,1,1,1), _Global_Black_Color, _Global_Power);
        }
        ENDCG  
        }
//#vertfrag-end
    }
    Fallback "Legacy Shaders/Transparent/Cutout/VertexLit"
}

    
