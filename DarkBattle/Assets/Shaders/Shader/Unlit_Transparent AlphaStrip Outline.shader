// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/Transparent AlphaStrip/Outline" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_AlphaTex ("Gray (RGB)", 2D) = "white" {}
	
	_OutlineColor ("Outline Color", Color) = (0.6, 0.5, 0.4, 1)
	_Outline ("Outline width", Float) = .005
	_Hue ("_Hue", Float) = 100
	_Saturation ("_Saturation", Float) = 100
	_Lightness ("_Lightness", Float) = 100
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 200
	
	Cull Off
	Lighting Off
	ZWrite On
	Fog { Mode Off }
	Offset -1, -1
	Blend SrcAlpha OneMinusSrcAlpha 
	
	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#pragma target 3.0
			#include "UnityCG.cginc"
			#include "ColorSpace.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _AlphaTex;
			float4 _AlphaTex_ST;
			
			float _Hue;
			float _Saturation;
			float _Lightness;
						
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			float4 frag (v2f i) : COLOR
			{
				float4 col = tex2D(_MainTex, i.texcoord);
				col.a *= tex2D(_AlphaTex, i.texcoord).r;
				if( _Hue != 100 || _Saturation != 100 || _Lightness != 100 )
				{
					col.rgb = Palette( col.rgb,float3(_Hue,_Saturation,_Lightness) );
				}
			
				return col;
			}
		ENDCG
	}
	UsePass "Hidden/Transparent Outline/OUTLINE"
}

}
