Shader "Muffin/MuffinMobileParticleAlphaBlend" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
		_ColorFade("color fade", Range(0, 1)) = 1
		_Darkness("darkness", Range(0, 1)) = 1
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off 
		Lighting Off 
		ZWrite Off 
		Fog { Color (1,1,1,1) }
		
		LOD 200
		
		Pass
        {
		 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float4 _Color;
			uniform float _ColorFade;
			uniform float _Darkness;

			struct VertexInput
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
			};
			
			struct VertexOutput
			{
				float4 pos : POSITION;
				float4 uv : TEXCOORD0;
			};

			VertexOutput vert(VertexInput v)
			{
				VertexOutput o;

				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv.xy = v.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;

				return o;
			}
			
			float4 frag(VertexOutput i) : COLOR
			{
				float4 output = tex2D(_MainTex, i.uv.xy);
				
				output *= _Color;
				
////// Color Fade:
				float averageVal = 0.3333f * (output.x + output.y + output.z);
				output.x = averageVal + (output.x - averageVal) * _ColorFade;
				output.y = averageVal + (output.y - averageVal) * _ColorFade;
				output.z = averageVal + (output.z - averageVal) * _ColorFade;
////// Darkness:
				output.rgb *= _Darkness;

				return output;
			}

			ENDCG
		}
	} 
	FallBack "Diffuse"
}
