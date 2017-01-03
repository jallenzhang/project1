// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Muffin/MuffinMoblieDiffuse" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ColorFade("color fade", Range(0, 1)) = 1
		_Darkness("darkness", Range(0, 1)) = 1
	}
	
	SubShader 
    {
         LOD 200
 
         Tags 
         { 
             "RenderType" = "Opaque" 
             "Queue" = "Geometry" 
         }
         
         Pass
         {
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma glsl_no_auto_normalization
            #include "UnityCG.cginc"
                 
            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
			uniform float _ColorFade;
			uniform float _Darkness;
 
            // sampler2D unity_Lightmap;
            // float4 unity_LightmapST;
 
			struct Vertex
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
				float4 uv2 : TEXCOORD1;
			};
 
			struct Fragment
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
				float4 uv2 : TEXCOORD1;
			};
 
			Fragment vert(Vertex v)
			{
				Fragment o;

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv.xy = v.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				o.uv2.xy = v.uv2.xy * unity_LightmapST.xy + unity_LightmapST.zw;

				return o;
			}
                                                 
			float4 frag(Fragment i) : COLOR
			{
				float4 output = float4(0, 0, 0, 1);

				output.rgb = tex2D(_MainTex, i.uv.xy).rgb;
				output.rgb *= (DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv2.xy)));
				
////// Color Fade:
				float averageVal = 0.3333f * (output.x + output.y + output.z);
				output.x = averageVal + (output.x - averageVal) * _ColorFade;
				output.y = averageVal + (output.y - averageVal) * _ColorFade;
				output.z = averageVal + (output.z - averageVal) * _ColorFade;

////// Darkness:
				output.rgb *= _Darkness;
				
				return saturate(output);
			}
             
            ENDCG
         }
    }
 
    Fallback "Moblie/Diffuse"
}

