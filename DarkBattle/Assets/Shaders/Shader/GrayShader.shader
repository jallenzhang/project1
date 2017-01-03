Shader "Unlit/GrayShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
	Tags { "RenderType"="Opaque" }
		pass
		{		    
			Cull Off
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			sampler2D _MainTex;
			float4 _MainTex_ST;

			struct appdata {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
			    float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			v2f vert( appdata v )
			{
			 	v2f o;
			 	o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			 	o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);			 				 	
			 	return o;
			}
			
			half4 frag (v2f i) : COLOR
			{
			    half4 texcol = tex2D (_MainTex, i.uv);
			    float t = (texcol.r+texcol.g+texcol.b)/3;
			    return float4(t,t,t,1);
			}
			ENDCG
		}
	} 
	Fallback "VertexLit"
}
