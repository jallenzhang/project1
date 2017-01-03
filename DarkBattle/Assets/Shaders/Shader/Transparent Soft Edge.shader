Shader "Unlit/Transparent Soft Edge(Cutout)" {
	Properties {
	_Color ("Main Color", Color) = (1, 1, 1, 1)
	_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
	_AlphaTex ("Gray (RGB)", 2D) = "white" {}
	_Cutoff ("Base Alpha cutoff", Range (0,0.99)) = .5
	_Hue ("_Hue", Float) = 100
	_Saturation ("_Saturation", Float) = 100
	_Lightness ("_Lightness", Float) = 100
	_Outline ("Outline width", Range (0.0, 0.03)) = .005
	_OutlineColor ("Outline Color", Color) = (0.6, 0.5, 0.4, 1)
}

SubShader {
	Tags { "Queue"="AlphaTest+1" "IgnoreProjector"="True" "RenderType"="TransparentCutout" }
	Lighting off
	
	// Render both front and back facing polygons.
	Cull Off
	// first pass:
	//   render any pixels that are more than [_Cutoff] opaque
	Pass {
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#pragma target 3.0
			#include "UnityCG.cginc"
			#include "ColorSpace.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _AlphaTex;
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
				return o;
			}
			
			float4 _Color;
			half4 frag (v2f i) : COLOR
			{
				half4 col = _Color * tex2D(_MainTex, i.texcoord);
				
				if( _Hue != 100 || _Saturation != 100 || _Lightness != 100 )
				{
					col.rgb = Palette( col.rgb,fixed3(_Hue,_Saturation,_Lightness) );
				}
				
				half4 a = tex2D(_AlphaTex, i.texcoord);
				clip(a.r - _Cutoff);
								
				return half4(col.r,col.g,col.b,a.r);
			}
		ENDCG
	}

	// Second pass:
	//   render the semitransparent details.
	Pass {
		Tags { "RequireOption" = "SoftVegetation" }
		
		// Dont write to the depth buffer
		ZWrite Off
		
		// Set up alpha blending
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#pragma target 3.0
			#include "UnityCG.cginc"
			#include "ColorSpace.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _AlphaTex;
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
				return o;
			}
			
			float4 _Color;
			half4 frag (v2f i) : COLOR
			{
				half4 col = _Color * tex2D(_MainTex, i.texcoord);				
				if( _Hue != 100 || _Saturation != 100 || _Lightness != 100 )
				{
					col.rgb = Palette( col.rgb,fixed3(_Hue,_Saturation,_Lightness) );
				}
				half4 a = tex2D(_AlphaTex, i.texcoord);
				clip(-(a.r - _Cutoff));
								
				return half4(col.r,col.g,col.b,a.r);
			}
		ENDCG
	}
	
}
}
