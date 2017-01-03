// Toony Colors Pro+Mobile Shaders
// (c) 2013,2014 Jean Moreno

Shader "Toony Colors Pro/Rim Outline/OneDirLight/Basic_WC"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
		
		//COLORS
		_Color ("Highlight Color", Color) = (0.8,0.8,0.8,1)
		_SColor ("Shadow Color", Color) = (0.0,0.0,0.0,1)
		
		//RIM OUTLINE
		_RimEnable( "RimSwitch 0-off 1-on",Float) = 1
		_RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.6)
		_RimMin ("Rim min", Range(0,1)) = 0.4
		_RimMax ("Rim max", Range(0,1)) = 0.6
		
		_Outline ("Outline Width", float) = 0.005
		_OutlineColor ("Outline Color", Color) = (0.2, 0.2, 0.2, 1)
		
		_Hue ("_Hue", Float) = 100
		_Saturation ("_Saturation", Float) = 100
		_Lightness ("_Lightness", Float) = 100
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma target 3.0
		#include "TGP_Include.cginc"
		#include "ColorSpace.cginc"
		
		//nolightmap nodirlightmap		LIGHTMAP
		//noforwardadd					ONLY 1 DIR LIGHT (OTHER LIGHTS AS VERTEX-LIT)
		#pragma surface surf ToonyColors nolightmap nodirlightmap vertex:vert noforwardadd 
		#pragma exclude_renderers flash
		
		sampler2D _MainTex;
		fixed4 _RimColor;
		float _RimMin;
		float _RimMax;
		float _RimEnable;
		
		float _Hue;
		float _Saturation;
		float _Lightness;
		
		struct Input
		{
			half2 uv_MainTex : TEXCOORD0;
			fixed3 rim;
		};
		
		void vert (inout appdata_full v, out Input o)
		{
			#if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X)
			UNITY_INITIALIZE_OUTPUT(Input,o);
			#endif
			
			if( _RimEnable == 1  )
				o.rim = 1.0f - saturate( dot(normalize(ObjSpaceViewDir(v.vertex)), v.normal) );
		}
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			
			if( _Hue != 100 || _Saturation != 100 || _Lightness != 100 )
			{
				c.rgb = Palette( c.rgb,fixed3(_Hue,_Saturation,_Lightness) );
			}
			
			if( _RimEnable == 1  )
			{
				//Rim Outline
				IN.rim = smoothstep(_RimMin, _RimMax, IN.rim);
				o.Albedo = lerp(c.rgb, _RimColor, IN.rim);
			}
			else
				o.Albedo = c.rgb;
			o.Alpha = 1;
		}		
		ENDCG
		
		UsePass "Hidden/ToonyColors-Outline/OUTLINE"
	}
	
	Fallback "VertexLit"
}
