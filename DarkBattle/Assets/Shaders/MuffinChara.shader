Shader "Muffin/MuffinChara" {

	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_NormalTex ("Normal (RGB)", 2D) = "white" {}
		_Mask1Tex ("Mask1 (RGB)", 2D) = "white" {}
		_Mask2Tex ("Mask2 (RGB)", 2D) = "white" {}
		_RimTex ("Rim (RGB)", 2D) = "white" {}
		_DetailTex ("Detail (RGB)", 2D) = "black" {}
		
		_ColorTint ("Color tint", Color) = (1, 1, 1, 1)
		_SpecularAmp ("Amplify speculation", float) = 1
		_SpecularPower ("Speculation power", float) = 1
		_ColorFade ("Color Fade", Range(0, 1)) = 1
		_RimPower ("Rim light power", Float) = 1
		_RimColor ("Rim light Color", Color) = (1, 1, 1, 1)
		_SelfIlluminationPower ("Self illumination power", Float) = 1
		_MetalnessLimit ("Metalness limit", Range(0, 1)) = 0
		_EdgeColor ("Edge Color", Color) = (0, 0, 0, 1)
		_EdgeThickness ("Edge Thickness", float) = 1
	}
	
	SubShader {
		Tags 
		{
            "RenderType"="Opaque"
			"Queue"="Geometry"
			"LightMode"="ForwardBase"
        }
		
		LOD 200
		
		Pass {
		
			Cull Back
			ZTest LEqual
				
			CGPROGRAM
			
			#pragma target 3.0
			
			#pragma vertex vert
            #pragma fragment frag
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			
			uniform float4 _LightColor0;	
			uniform float _SpecularAmp;
			uniform float _SpecularPower;
			uniform float _ColorFade;
			uniform float _RimPower;
			uniform float4 _RimColor;
			uniform float _SelfIlluminationPower;
			uniform float _MetalnessLimit;
			uniform float4 _ColorTint;
			
			uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
			uniform sampler2D _NormalTex; uniform float4 _NormalTex_ST;
			uniform sampler2D _Mask1Tex; uniform float4 _Mask1Tex_ST;
			uniform sampler2D _Mask2Tex; uniform float4 _Mask2Tex_ST;
			uniform sampler2D _RimTex; uniform float4 _RimTex_ST;
			uniform sampler2D _DetailTex; uniform float4 _DetailTex_ST;
			
			struct v2f
			{
				float4 pos      : SV_POSITION;
				float2 uv       : TEXCOORD0;
				float2 uv1      : TEXCOORD1;
				float3 eyeDir   : TEXCOORD2;
				float3 normal   : TEXCOORD3;
				float3 tangent  : TEXCOORD4;
				float3 binormal : TEXCOORD5;
				float3 lightDir : TEXCOORD6;
				float4 posWorld : TEXCOORD7;
			};
			
			v2f vert (appdata_full v) {
                v2f o;
                o.uv = v.texcoord;
				o.uv1 = v.texcoord1;
                o.normal = mul(float4(v.normal,0), _World2Object).xyz;
                o.tangent = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormal = normalize(cross(o.normal, o.tangent) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
				o.eyeDir = normalize( _WorldSpaceCameraPos.xyz - o.posWorld.xyz ).xyz;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.lightDir = WorldSpaceLightDir( v.vertex );
                return o;
            }
			
			float4 frag( v2f i ) : COLOR
			{
				float4 finalColor = float4(1, 1, 1, 1);
				float3x3 tangentTransform = float3x3( i.tangent, i.binormal, i.normal);
				float4 maskColor1 = tex2D( _Mask1Tex, TRANSFORM_TEX(i.uv, _Mask1Tex));
				float4 maskColor2 = tex2D( _Mask2Tex, TRANSFORM_TEX(i.uv, _Mask2Tex));
				
/////// Normals:
				float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
				float3 normalLocal = UnpackNormal(tex2D(_NormalTex, TRANSFORM_TEX(i.uv, _NormalTex))).rgb;
				float3 normalDirection =  normalize(mul( normalLocal, tangentTransform ));
				float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz, _WorldSpaceLightPos0.w));
				float3 halfDirection = normalize(viewDirection + lightDirection);
				float normalDotEye = dot( normalDirection, i.eyeDir.xyz );
				float normalDotLight = dot( normalDirection, i.lightDir.xyz );
				float falloffU = clamp( 1.0 - abs( normalDotEye ), 0.02, 0.98 );
				
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
				
/////// Diffuse:
				float4 diffuseColor = tex2D( _MainTex, TRANSFORM_TEX(i.uv, _MainTex));
				finalColor = diffuseColor;

/////// Detail:				
				float4 detailColor = tex2D( _DetailTex, TRANSFORM_TEX(i.uv1, _DetailTex));
				finalColor += detailColor;
				
/////// Self illumination:
				float selfIllumination = maskColor1.g;
				finalColor += selfIllumination * _SelfIlluminationPower * diffuseColor;

/////// Specular:
				float specularIntensity = maskColor2.r;
				float tintSpecByColor = maskColor2.b;
				float specularExponent = maskColor1.r;
				
				float3 specularColor = (1 - tintSpecByColor) * attenColor + tintSpecByColor * diffuseColor;
				float3 specular = attenColor * pow(max(0, dot(halfDirection, normalDirection)), _SpecularPower * specularExponent) * specularColor;
				finalColor.rgb += _SpecularAmp * specularIntensity * specular;
				
/////// Metalness:
				float metal = maskColor1.b;		
				finalColor.rgb -= metal *  (1 - max(pow(max(0, dot(halfDirection, normalDirection)), _SpecularPower * specularExponent), _MetalnessLimit));
			
/////// Rim:
				float rimLightIntensity = maskColor2.g;
				float rimlightDot = saturate( 0.5 * ( dot( normalDirection, i.lightDir ) + 1.0 ) );
				float3 rimColor = tex2D( _RimTex, float2( saturate( rimlightDot * falloffU ), 0.25f ) );
				finalColor.rgb += rimLightIntensity * _RimColor * rimColor * _RimPower;
				
/////// _ColorTint:
				finalColor.rgb *= _ColorTint.rgb;
			
/////// Color Fade:
				float averageVal = 0.3333f * (finalColor.x + finalColor.y + finalColor.z);
				finalColor.x = averageVal + (finalColor.x - averageVal) * _ColorFade;
				finalColor.y = averageVal + (finalColor.y - averageVal) * _ColorFade;
				finalColor.z = averageVal + (finalColor.z - averageVal) * _ColorFade;
			
				return finalColor;
			}

			ENDCG	
		}
		/*
		Pass
		{
			Cull Front
			ZTest Less
					
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			// Outline shader

			// Material parameters
			float _EdgeThickness = 1.0;
			float4 _EdgeColor;
			
			// Textures
			sampler2D _MainTex;

			// Structure from vertex shader to fragment shader
			struct v2f
			{
				float4 pos : SV_POSITION;
			};

			// Outline thickness multiplier
			#define INV_EDGE_THICKNESS_DIVISOR 0.00285

			// Vertex shader
			v2f vert( appdata_base v )
			{
				v2f o;

				float4 projSpacePos = mul( UNITY_MATRIX_MVP, v.vertex );
				float4 projSpaceNormal = normalize( mul( UNITY_MATRIX_MVP, half4( v.normal, 0 ) ) );
				float4 scaledNormal = _EdgeThickness * INV_EDGE_THICKNESS_DIVISOR * projSpaceNormal;

				scaledNormal.z += 0.00001;
				o.pos = projSpacePos + scaledNormal;
				return o;
			}

			// Fragment shader
			float4 frag( v2f i ) : COLOR
			{
				return _EdgeColor;
			}

			ENDCG
		}
		*/
	} 
	FallBack "Diffuse"
}
