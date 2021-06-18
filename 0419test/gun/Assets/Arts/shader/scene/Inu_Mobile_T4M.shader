// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Inu/Scene/T4M 4 Textures" {
	Properties {
		_RColor ("RColor", Color) = (1,1,1,1)
		_RColorPower ("RColorPower", Float ) = 1
		_Splat0 ("Layer 1", 2D) = "white" {}
		_GColor ("GColor", Color) = (1,1,1,1)
		_GColorPower ("GColorPower", Float ) = 1
		_Splat1 ("Layer 2", 2D) = "white" {}
		_BColor ("BColor", Color) = (1,1,1,1)
		_BColorPower ("BColorPower", Float ) = 1
		_Splat2 ("Layer 3", 2D) = "white" {}
		_AColor ("AColor", Color) = (1,1,1,1)
		_AColorPower ("AColorPower", Float ) = 1
		_Splat3 ("Layer 4", 2D) = "white" {}
		_Control ("Control (RGBA)", 2D) = "white" {}
	}
//#surface-start
//#	SubShader {
//#		Tags {
//#			"SplatCount" = "4"
//#			"RenderType" = "Opaque"
//#		}
//#		CGPROGRAM
//#		#pragma surface surf Lambert addshadow
//#		#pragma exclude_renderers xbox360 ps3
//#		#pragma target 3.5
//#
//#		struct Input {
//#			float2 uv_Control : TEXCOORD0;
//#			float2 uv_Splat0 : TEXCOORD1;
//#			float2 uv_Splat1 : TEXCOORD2;
//#			float2 uv_Splat2 : TEXCOORD3;
//#			float2 uv_Splat3 : TEXCOORD4;
//#		};
//#		
//#		uniform sampler2D _Control;
//#		uniform sampler2D _Splat0,_Splat1,_Splat2,_Splat3;
//#		uniform fixed4 _RColor;
//#		uniform fixed4 _GColor;
//#		uniform fixed4 _BColor;
//#		uniform fixed _RColorPower;
//#		uniform fixed _GColorPower;
//#		uniform fixed _BColorPower;
//#		uniform fixed4 _AColor;
//#		uniform fixed _AColorPower;
//#
//#		void surf (Input IN, inout SurfaceOutput o) {
//#			fixed4 splat_control = tex2D (_Control, IN.uv_Control).rgba;
//#				
//#			fixed3 lay1 = tex2D (_Splat0, IN.uv_Splat0)* _RColor * _RColorPower;
//#			fixed3 lay2 = tex2D (_Splat1, IN.uv_Splat1)* _GColor * _GColorPower;
//#			fixed3 lay3 = tex2D (_Splat2, IN.uv_Splat2)* _BColor * _BColorPower;
//#			fixed3 lay4 = tex2D (_Splat3, IN.uv_Splat3)* _AColor * _AColorPower;
//#			o.Alpha = 0.0;
//#			o.Albedo.rgb = (lay1 * splat_control.r + lay2 * splat_control.g + lay3 * splat_control.b + lay4 * splat_control.a);
//#		}
//#		ENDCG
//#	}
//#surface-end

//#vertfrag-start
	SubShader 
	{ 
		Tags 
		{
			"RenderType" = "Opaque"
		}
		Pass 
		{
			Tags { "LightMode" = "ForwardBase" }
			LOD 300
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma shader_feature SHADOWS_NATIVE
			#pragma multi_compile SHADOWS_OFF SHADOWS_SCREEN
			#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
			#include "UnityCG.cginc"
			#include "DlInuScene.cginc"

			sampler2D _Control;
			sampler2D _AlphaTex;
			float4 _Control_ST;
			float4 _Splat0_ST;
			float4 _Splat1_ST;
			float4 _Splat2_ST;
			float4 _Splat3_ST;
			uniform fixed4 _RColor;
			uniform fixed4 _GColor;
			uniform fixed4 _BColor;
			uniform fixed _RColorPower;
			uniform fixed _GColorPower;
			uniform fixed _BColorPower;
			uniform fixed4 _AColor;
			uniform fixed _AColorPower;
			sampler2D _Splat0,_Splat1,_Splat2,_Splat3;
			fixed _FogRate;
			uniform fixed _Global_Power;
			struct appdata
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 pack0 : TEXCOORD0;
				float4 pack1 : TEXCOORD1;
				float2 tc_Control : TEXCOORD2;
				UNITY_FOG_COORDS(3)
			#ifndef LIGHTMAP_OFF
				half2 lmap : TEXCOORD4;
				#ifdef SHADOWS_SCREEN
					SHADOW_COORDS(5)
				#endif
			#endif
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.pack0.xy = TRANSFORM_TEX(v.texcoord, _Splat0);
				o.pack0.zw = TRANSFORM_TEX(v.texcoord, _Splat1);
				o.pack1.xy = TRANSFORM_TEX(v.texcoord, _Splat2);
				o.pack1.zw = TRANSFORM_TEX(v.texcoord, _Splat3);
				o.tc_Control = TRANSFORM_TEX(v.texcoord, _Control);
				UNITY_TRANSFER_FOG(o, o.pos);
				#ifndef LIGHTMAP_OFF
					o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
					#ifdef SHADOWS_SCREEN
						TRANSFER_SHADOW(o);
					#endif
				#endif
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 splat_control = tex2D(_Control, i.tc_Control);
				//splat_control.a = tex2D(_AlphaTex, i.tc_Control).r;
				fixed weight = dot(splat_control, 1.0);
				splat_control /= (weight + 1e-3f);
				fixed3 uv0 = splat_control.r * tex2D(_Splat0, i.pack0.xy).rgb* _RColor * _RColorPower;;
				fixed3 uv1 = splat_control.g * tex2D(_Splat1, i.pack0.zw).rgb* _GColor * _GColorPower;;
				fixed3 uv2 = splat_control.b * tex2D(_Splat2, i.pack1.xy).rgb* _BColor * _BColorPower;;
				fixed3 uv3 = splat_control.a * tex2D(_Splat3, i.pack1.zw).rgb* _AColor * _AColorPower;
				fixed3 texcol = uv0 + uv1 + uv2 + uv3;
				#ifndef LIGHTMAP_OFF
					fixed4 bakedColorTex = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap.xy);
					half3 bakedColor = DecodeLightmap(bakedColorTex);
					texcol *= bakedColor;
					#ifdef SHADOWS_SCREEN
					 	texcol *= CaculateShadowColor(SHADOW_ATTENUATION(i));
					//  	fixed atten = SHADOW_ATTENUATION(i);
					//  	half3 shadowLightmapColor = bakedColorTex.rgb * atten;
					//  	half3 darkerColor = max(bakedColor, shadowLightmapColor);
					// 	texcol *= bakedColor;
					// 	texcol *= min(atten + max((1.6-bakedColor.r), 0), 1);
					//  #else
					// 	texcol *= bakedColor;
					 #endif
				#endif
				fixed4 ret = fixed4(texcol, 1.0);
				UNITY_APPLY_FOG_COLOR(i.fogCoord, ret, lerp(unity_FogColor, ret, _FogRate));
				return ret * (1-_Global_Power);
			}
			ENDCG
		}
	}
//#vertfrag-end
	FallBack "Inu/Scene/VertexLit"
}
