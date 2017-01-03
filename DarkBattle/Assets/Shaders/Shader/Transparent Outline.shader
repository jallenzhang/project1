Shader "Hidden/Transparent Outline" {
	Properties
	{
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_OutlineColor ("Outline Color", Color) = (0.6, 0.5, 0.4, 1)
		_Outline ("Outline width", Float) = .005
		_MainTex ("Base (RGB)", 2D) = "white" { }
		_AlphaTex ("Gray (RGB)", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	}
	SubShader {
		Tags { "Queue"="AlphaTest+1" "IgnoreProjector"="True" "RenderType"="TransparentCutout" }
		LOD 200
		Lighting off
		Cull Off
		
		// outline pass
		Pass 
		{
			Name "OUTLINE"
			Cull Front
			ZWrite Off
			Lighting Off
			CGINCLUDE
			#include "UnityCG.cginc"    
			ENDCG
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			struct appdata
			{
				float4 vertex : POSITION;	    
				float3 normal : NORMAL;
			};
			struct v2f
			{
				float4 pos : POSITION;
			};
			uniform float _Outline;
			uniform float4 _OutlineColor;
			
			v2f vert(appdata v)
			{
				v2f o;
				float4 pos = mul( UNITY_MATRIX_MV, v.vertex + float4(v.normal,0) * _Outline);
				o.pos = mul(UNITY_MATRIX_P, pos);
				return o;
			}
			half4 frag(v2f i) :COLOR
			{      
				return _OutlineColor;
			}
			ENDCG
		}
		// outline pass clip
		Pass 
		{
			Name "OUTLINE_CLIP"
			Cull Front
			ZWrite On
			Lighting Off
			CGINCLUDE
			#include "UnityCG.cginc"    
			ENDCG
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			struct appdata
			{
				float4 vertex : POSITION;    
				float3 normal : NORMAL;
				float2 texcoord : TEXCOORD0;
			};
			struct v2f
			{
				float4 pos : POSITION;
				float2 texcoord : TEXCOORD0;
			};
			uniform float _Outline;
			uniform float4 _OutlineColor;
			uniform sampler2D _MainTex;			
			float4 _MainTex_ST;
			sampler2D _AlphaTex;
			float _Cutoff;
			
			v2f vert(appdata v)
			{
				v2f o;
				float4 pos = mul( UNITY_MATRIX_MV, v.vertex + float4(v.normal,0) * _Outline);
				o.pos = mul(UNITY_MATRIX_P, pos);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			half4 frag(v2f i) :COLOR
			{
				half4 a = tex2D(_AlphaTex, i.texcoord);
				clip(a.r - _Cutoff);
				return _OutlineColor;
			}
			ENDCG
		}
	}
}
