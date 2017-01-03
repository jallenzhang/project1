// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/Transparent AlphaStrip" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_AlphaTex ("Gray (RGB)", 2D) = "white" {}
	
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
			
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 col = tex2D(_MainTex, i.texcoord);
				fixed4 a = tex2D(_AlphaTex, i.texcoord);
				if( _Hue != 100 || _Saturation != 100 || _Lightness != 100 )
				{
					col.rgb = Palette( col.rgb,fixed3(_Hue,_Saturation,_Lightness) );
				}
			
				return float4(col.r,col.g,col.b,a.r);
			}
		ENDCG
	}
}

}
