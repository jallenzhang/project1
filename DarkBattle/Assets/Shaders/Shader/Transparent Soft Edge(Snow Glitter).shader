Shader "Unlit/Transparent Soft Edge(Cutout)(Snow Glitter)" {
	Properties {
	_Color ("Main Color", Color) = (1, 1, 1, 1)
	_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
	_AlphaTex ("Gray (RGB)", 2D) = "white" {}
	_GlitterTex	("Specular (RGB)", 2D) = "black" {}
	_Specular ("Specular Intensity", Range (0.0, 5.0))	= 1.0
	_Shininess ("Shininess", Range (0.01, 1.0))	= 0.08
	_Aniso ("Anisotropic Mask", Range (0.0, 1.0)) = 0.0
	_Glitter ("Anisotropic Intensity", Range (0.0, 15.0))= 0.5
	_Cutoff ("Base Alpha cutoff", Range (0,0.99)) = .5
	_SpecularColor		("Glitter (RGB) Interpolator (A)", Color)	= (1, 1, 1, 0)
	_Speed				("Glitter Speed", Range (0.0, 10.0))		= 3.0
	_Density			("Glitter Density", Range (1.25, 0.0))		= 0.95
	_DensityStatic		("Glitter Static Density", Range (5.0, 1.0))= 4
	_Power				("Glitter Power", Range (0.0, 1.0))			= 0.2
	_Hue ("_Hue", Float) = 100
	_Saturation ("_Saturation", Float) = 100
	_Lightness ("_Lightness", Float) = 100
}

SubShader {
	Tags { "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Lighting off
	
	// Render both front and back facing polygons.
	Cull Off
	
	// first pass:
	//   render any pixels that are more than [_Cutoff] opaque
	Pass {  
		CGPROGRAM
		#pragma target 3.0
		#pragma vertex vert
		#pragma fragment frag
		
		#include "UnityCG.cginc"
		#include "SnowCore.cginc"
		#include "ColorSpace.cginc"

		struct appdata_t {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 color : COLOR;
			float2 texcoord : TEXCOORD0;
			float2 texcoord1 : TEXCOORD1;
		};

		struct v2f {
			float4 vertex : POSITION;
			float4 color : COLOR;
			float2 texcoord : TEXCOORD0;
			half2 texcoord1 : TEXCOORD1;
			half3 viewDir : TEXCOORD2;
			half3 Normal: TEXCOORD3;
			half3 lightDir : TEXCOORD4;
		};

		sampler2D _MainTex;
		float4 _MainTex_ST;
		sampler2D _AlphaTex;
		sampler2D _GlitterTex;
		float4 _GlitterTex_ST;
		half _Glitter;
		half _Aniso;
		half _Shininess;
		half _Specular;
		half4 _SpecularColor;
		half _Speed;
		half _Density;
		half _DensityStatic;
		half _Power;
		float _Cutoff;
		float _Hue;
		float _Saturation;
		float _Lightness;
		
		v2f vert (appdata_t v)
		{
			v2f o;
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			o.color = v.color;
			o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.texcoord1 = TRANSFORM_TEX(v.texcoord1, _GlitterTex);
			o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
			o.Normal = v.normal;
			o.lightDir = normalize(WorldSpaceLightDir(v.vertex));
			return o;
		}
				
		float4 _Color;
		half4 frag (v2f i) : COLOR
		{
			half4 col = _Color * tex2D(_MainTex, i.texcoord);
			fixed4 gil = tex2D(_GlitterTex,i.texcoord1);
			half4 a = tex2D(_AlphaTex, i.texcoord);			
			
			half3 H	= normalize(i.viewDir.xyz+i.lightDir.xyz);
			half NdotH = max(0, dot(i.Normal, H));
			half3 V = normalize(i.viewDir.xyz);
			half3 L = normalize(i.lightDir.xyz);		
			half3 view	= mul((float3x3)UNITY_MATRIX_V, i.Normal);
			half3 glitter = frac(0.7 * i.Normal + 9 * gil.rgb + _Speed * V * L * view);
			glitter *= (_Density - glitter);
			glitter = saturate(1 - _DensityStatic * (glitter.x + glitter.y + glitter.z));
			glitter = (glitter * _SpecularColor.rgb) * _SpecularColor.a + half3(Overlay(glitter, gil.rgb * _Power)) * (1 - _SpecularColor.a);				
			half3 specular		= saturate(pow(NdotH, _Shininess * 128.0) * _Specular * glitter);				
			half3 anisotropic	= max(0, sin(radians((NdotH + _Aniso) * 180)));
			anisotropic			= saturate(glitter * anisotropic * _Glitter);
			half4 c;
			c.rgb	= col.rgb + (anisotropic + specular);
			
			if( _Hue != 100 || _Saturation != 100 || _Lightness != 100 )
			{
				c.rgb = Palette( c.rgb,fixed3(_Hue,_Saturation,_Lightness) );
			}
			clip(a.r - _Cutoff);
			return float4(c.r,c.g,c.b,a.r);
		}
		ENDCG
	}

	// Second pass:
	//   render the semitransparent details.
	Pass {
		Tags { "RequireOption" = "SoftVegetation" }
		
		// Dont write to the depth buffer
		ZWrite off
		
		// Set up alpha blending
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma target 3.0
		#pragma vertex vert
		#pragma fragment frag
		
		#include "UnityCG.cginc"
		#include "SnowCore.cginc"
		#include "ColorSpace.cginc"

		struct appdata_t {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 color : COLOR;
			float2 texcoord : TEXCOORD0;
			float2 texcoord1 : TEXCOORD1;
		};

		struct v2f {
			float4 vertex : POSITION;
			float4 color : COLOR;
			float2 texcoord : TEXCOORD0;
			half2 texcoord1 : TEXCOORD1;
			half3 viewDir : TEXCOORD2;
			half3 Normal: TEXCOORD3;
		};

		sampler2D _MainTex;
		float4 _MainTex_ST;
		sampler2D _AlphaTex;
		sampler2D _GlitterTex;
		float4 _GlitterTex_ST;
		half _Glitter;
		half _Aniso;
		half _Shininess;
		half _Specular;
		half4 _SpecularColor;
		half _Speed;
		half _Density;
		half _DensityStatic;
		half _Power;
		float _Cutoff;
		float _Hue;
		float _Saturation;
		float _Lightness;
		
		v2f vert (appdata_t v)
		{
			v2f o;
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			o.color = v.color;
			o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.texcoord1 = TRANSFORM_TEX(v.texcoord1, _GlitterTex);
			o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
			o.Normal = v.normal;
			return o;
		}
				
		float4 _Color;
		half4 frag (v2f i) : COLOR
		{
			half4 col = _Color * tex2D(_MainTex, i.texcoord);
			fixed4 gil = tex2D(_GlitterTex,i.texcoord1);
			half4 a = tex2D(_AlphaTex, i.texcoord);
						
			half3 H	= normalize(i.viewDir.xyz);
			half NdotH = max(0, dot(i.Normal, H));
			half3 view	= mul((float3x3)UNITY_MATRIX_V, i.Normal);
			half3 glitter = frac(0.7 * i.Normal + 9 * gil.rgb + _Speed * H * view);
			glitter *= (_Density - glitter);
			glitter = saturate(1 - _DensityStatic * (glitter.x + glitter.y + glitter.z));
			glitter = (glitter * _SpecularColor.rgb) * _SpecularColor.a + half3(Overlay(glitter, gil.rgb * _Power)) * (1 - _SpecularColor.a);				
			half3 specular		= saturate(pow(NdotH, _Shininess * 128.0) * _Specular * glitter);				
			half3 anisotropic	= max(0, sin(radians((NdotH + _Aniso) * 180)));
			anisotropic			= saturate(glitter * anisotropic * _Glitter);
			half4 c;
			c.rgb	= col.rgb + (anisotropic + specular);
			if( _Hue != 100 || _Saturation != 100 || _Lightness != 100 )
			{
				c.rgb = Palette( c.rgb,fixed3(_Hue,_Saturation,_Lightness) );
			}
			clip(-(a.r - _Cutoff));
			return float4(c.r,c.g,c.b,a.r);
		}
		ENDCG
	}
}
}
