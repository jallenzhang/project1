// Blackfire Studio
// Matthieu Ostertag

Shader "Blackfire Studio/Snow/Snow Full_NoOutline" {
	Properties {
		_MainTex			("Diffuse (RGB)", 2D) 						= "white" {}
		_GlitterTex			("Specular (RGB)", 2D)						= "black" {}
		_Specular			("Specular Intensity", Range (0.0, 5.0))	= 1.0
		_Shininess			("Shininess", Range (0.01, 1.0))			= 0.08
		_Aniso				("Anisotropic Mask", Range (0.0, 1.0))		= 0.0
		_Glitter			("Anisotropic Intensity", Range (0.0, 15.0))= 0.5
		_SpecularColor		("Glitter (RGB) Interpolator (A)", Color)	= (1, 1, 1, 0)
		_Speed				("Glitter Speed", Range (0.0, 10.0))		= 3.0
		_Density			("Glitter Density", Range (1.25, 0.0))		= 0.95
		_DensityStatic		("Glitter Static Density", Range (5.0, 1.0))= 4
		_Power				("Glitter Power", Range (0.0, 1.0))			= 0.2

		//RIM OUTLINE
		_RimEnable( "RimSwitch 0-off 1-on",Float) = 1
		_RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.6)
		_RimMin ("Rim min", Range(0,1)) = 0.4
		_RimMax ("Rim max", Range(0,1)) = 0.6
				
		_Outline ("Outline Width", Range(0,0.05)) = 0.005
		_OutlineColor ("Outline Color", Color) = (0.2, 0.2, 0.2, 1)
		
		_Hue ("_Hue", Float) = 50
		_Saturation ("_Saturation", Float) = 100
		_Lightness ("_Lightness", Float) = 100
	}
	
	SubShader {
		Tags { "Queue" = "Geometry" "RenderType" = "Opaque" }
		LOD 400
		
		CGPROGRAM
		#pragma target 3.0
		#pragma surface SnowSurface Snow nolightmap nodirlightmap vertex:vert noforwardadd
		
		#ifdef SHADER_API_OPENGL	
			#pragma glsl
		#endif
		#define SNOW_GLITTER
		
		#include "SnowCore.cginc"
		#include "SnowInputs.cginc"
		#include "SnowLighting.cginc"
		#include "SnowSurface.cginc"

		ENDCG
	}
	FallBack "Diffuse"
}