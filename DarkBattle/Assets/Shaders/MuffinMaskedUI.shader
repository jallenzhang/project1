Shader "Muffin/MuffinMaskedUI"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
		_MaskTex ("Black White", 2D) = "white" {}
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_ColorFade("Color Fade", Range(0, 1)) = 1
	}
	
	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _MaskTex;

			float4 _MainTex_ST;
			float4 _MaskTex_ST;
			float4 _Color;
			float _ColorFade;
	
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				half2 texcoord1 : TEXCOORD1;
				fixed4 color : COLOR;
			};
	
			v2f o;

			v2f vert (appdata_t v)
			{
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = v.texcoord;
				o.texcoord1 = v.texcoord1;
				o.color = v.color;
				return o;
			}
				
			float4 frag (v2f IN) : COLOR
			{
				float4 col = tex2D(_MainTex, IN.texcoord) * IN.color;
				col *= tex2D(_MaskTex, IN.texcoord1);
				col *= _Color;
				float averageVal = 0.3333f * (col.r + col.g + col.b);
				col.r = averageVal + (col.r - averageVal) * _ColorFade;
				col.g = averageVal + (col.g - averageVal) * _ColorFade;
				col.b = averageVal + (col.b - averageVal) * _ColorFade;
				return col;
			}
			ENDCG
		}
	}
}
