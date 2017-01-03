Shader "Unlit/Transparent Colored HSLSpecialModify"
{
	Properties
	{
		_MainTex("Base (RGB), Alpha (A)", 2D) = "black" {}
		_AlphaTex("Alpha (RGB), Alpha (A)", 2D) = "white" {}
	}
	
	SubShader
	{

		LOD 200

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
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "ColorSpace.cginc"

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float4 _MainTex_ST;
	
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			v2f o;

			v2f vert (appdata_t v)
			{
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}

			float4 frag (v2f IN) : COLOR
			{
				float4 col = tex2D(_MainTex, IN.texcoord);
				col.a *= tex2D(_AlphaTex, IN.texcoord).r * IN.color.a;

				if (IN.color.r < 1 || IN.color.g < 1 || IN.color.b < 1)
				{
					fixed3 hsl = RGBtoHSL(col.rgb);
					if (IN.color.r < 0.5)
						hsl.r = IN.color.r + IN.color.r;
					else
						hsl.r = fmod(hsl.r + IN.color.r + IN.color.r, 1);
					if (IN.color.g < 0.5)
						hsl.g = 1 - (1 - hsl.g) * (1 - IN.color.g - IN.color.g);
					else
						hsl.g *= IN.color.g + IN.color.g - 1;
					col.rgb = HSLtoRGB(hsl);
					if (IN.color.b < 0.5)
						col.rgb = 1 - (1 - col.rgb) * (1 - IN.color.b - IN.color.b);
					else
						col.rgb *= IN.color.b + IN.color.b - 1;
				}
				return col;
			}
			ENDCG
		}
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
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}
