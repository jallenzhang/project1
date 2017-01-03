Shader "Muffin/MuffinCloth" {
	Properties {
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_SpecularPower ("Specular Power", Float) = 20
		_SpecularAmplifier ("Specular Amplifier", Float) = 1
		_OutlineThickness ("Outline Thickness", Float) = 1
		_EdgeColor ("Edge Color", Color) = (1, 1, 1, 1)
		_RimPower ("Rim light power", Float) = 1
		_RimColor ("Rim light Color", Color) = (1, 1, 1, 1)
		_FallOffPower ("Falloff Power", Float) = 0.3
		_FallOffColor ("Falloff Color", Color) = (1, 1, 1, 1)
		_ColorFade("Color Fade", Range(0, 1)) = 1
		
		_MainTex ("Diffuse", 2D) = "white" {}
		_FalloffSampler ("Falloff Control", 2D) = "white" {}
		_RimLightSampler ("RimLight Control", 2D) = "white" {}
		_SpecularReflectionSampler ("Specular Mask", 2D) = "white" {}
	}
	
	//Toon And OutLine
	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
			"Queue"="Geometry"
			"LightMode"="ForwardBase"
		}

		LOD 200

		Pass
		{
			Cull Back
			ZTest LEqual
			
			
			CGPROGRAM

			//#pragma multi_compile_fwdbase
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			float4 _Color;
			float4 _LightColor0;
			float4 _MainTex_ST;
			float4 _RimColor;
			float4 _FallOffColor;
			float _SpecularPower;
			float _RimPower;
			float _FallOffPower;
			float _SpecularAmplifier;
			float _ColorFade;

			// Textures
			sampler2D _MainTex;
			sampler2D _FalloffSampler;
			sampler2D _RimLightSampler;
			sampler2D _SpecularReflectionSampler;
			sampler2D _NormalMapSampler;

			// Structure from vertex shader to fragment shader
			struct v2f
			{
				float4 pos      : SV_POSITION;
				float2 uv       : TEXCOORD0;
				float3 eyeDir   : TEXCOORD1;
				float3 normal   : TEXCOORD2;
				float3 tangent  : TEXCOORD3;
				float3 binormal : TEXCOORD4;
				float3 lightDir : TEXCOORD5;
			};

			// Float types
			#define float_t  half
			#define float2_t half2
			#define float3_t half3
			#define float4_t half4

			// Vertex shader
			v2f vert( appdata_tan v )
			{
				v2f o;
				o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
				o.uv.xy = TRANSFORM_TEX( v.texcoord.xy, _MainTex );
				o.normal = normalize( mul( _Object2World, float4_t( v.normal, 0 ) ).xyz );
				
				// Eye direction vector
				half4 worldPos = mul( _Object2World, v.vertex );
				o.eyeDir.xyz = normalize( _WorldSpaceCameraPos.xyz - worldPos.xyz ).xyz;
				
				// Binormal and tangent (for normal map)
				o.tangent = v.tangent.xyz;
				o.binormal = cross( v.normal, v.tangent.xyz ) * v.tangent.w;
				
				o.lightDir = WorldSpaceLightDir( v.vertex );

				return o;
			}

			// Fragment shader
			float4 frag( v2f i ) : COLOR
			{
				float3_t normalVec = i.normal; // GetNormalFromMap( i );
				float_t normalDotEye = dot( normalVec, i.eyeDir.xyz );
				float_t normalDotLight = dot( normalVec, i.lightDir.xyz );
				float_t falloffU = clamp( 1.0 - abs( normalDotEye ), 0.02, 0.98 );

				//diffuse
				float4_t diffSamplerColor = tex2D( _MainTex, i.uv.xy );
				float3_t combinedColor = diffSamplerColor;
				
				// Falloff
				/**/
				float4_t falloffSamplerColor = _FallOffPower * tex2D( _FalloffSampler, float2( falloffU, 0.25f ) );
				combinedColor += _FallOffColor * falloffSamplerColor.rgb * diffSamplerColor.rgb;
				

				// Specular light
				/**/
				float4_t specularMaskColor = tex2D( _SpecularReflectionSampler, i.uv.xy );
				float4_t lighting = lit( normalDotLight, normalDotEye, _SpecularPower );
				float3_t specularColor = _SpecularAmplifier * saturate( lighting.z ) * specularMaskColor.rgb; // 
				combinedColor += specularColor;
				
				//difuse light
				combinedColor *= _LightColor0.rgb;
				
				// Rimlight
				/**/
				float_t rimlightDot = saturate( 0.5 * ( dot( normalVec, i.lightDir ) + 1.0 ) );
				falloffU = saturate( rimlightDot * falloffU );
				float3_t rimColor = tex2D( _RimLightSampler, float2( falloffU, 0.25f ) );
				combinedColor += _RimColor * rimColor * _RimPower;
				
				
				// Color Fade
				float4 trueColor = float4( _Color.rgb * combinedColor, 1 );
				float averageVal = 0.3333f * (trueColor.x + trueColor.y + trueColor.z);
				trueColor.x = averageVal + (trueColor.x - averageVal) * _ColorFade;
				trueColor.y = averageVal + (trueColor.y - averageVal) * _ColorFade;
				trueColor.z = averageVal + (trueColor.z - averageVal) * _ColorFade;
				
				return trueColor;
			}

			ENDCG
		}
		
		/**/
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
			float4 _Color;
			float4 _LightColor0;
			float _OutlineThickness = 1.0;
			float4 _MainTex_ST;
			float4 _EdgeColor;
			
			// Textures
			sampler2D _MainTex;

			// Structure from vertex shader to fragment shader
			struct v2f
			{
				float4 pos : SV_POSITION;
			};

			// Float types
			#define float_t  half
			#define float2_t half2
			#define float3_t half3
			#define float4_t half4

			// Outline thickness multiplier
			#define INV_EDGE_THICKNESS_DIVISOR 0.00285
			// Outline color parameters
			#define SATURATION_FACTOR 0.6
			#define BRIGHTNESS_FACTOR 0.8

			// Vertex shader
			v2f vert( appdata_base v )
			{
				v2f o;

				float4 projSpacePos = mul( UNITY_MATRIX_MVP, v.vertex );
				float4 projSpaceNormal = normalize( mul( UNITY_MATRIX_MVP, half4( v.normal, 0 ) ) );
				float4 scaledNormal = _OutlineThickness * INV_EDGE_THICKNESS_DIVISOR * projSpaceNormal; // * projSpacePos.w;

				scaledNormal.z += 0.00001;
				o.pos = projSpacePos + scaledNormal;
				return o;
			}

			// Fragment shader
			float4 frag( v2f i ) : COLOR
			{
				return _EdgeColor; //_EdgeColor
			}

			ENDCG
		}
		
	}

	//Toon
	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
			"Queue"="Geometry"
			"LightMode"="ForwardBase"
		}

		LOD 100

		Pass
		{
			Cull Back
			ZTest LEqual
			
			
			CGPROGRAM

			//#pragma multi_compile_fwdbase
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			float4 _Color;
			float4 _LightColor0;
			float _SpecularPower;
			float4 _MainTex_ST;
			float _RimPower;
			float _FallOffPower;
			float _SpecularAmplifier;
			float4 _RimColor;
			float4 _FallOffColor;
			float _ColorFade;

			// Textures
			sampler2D _MainTex;
			sampler2D _FalloffSampler;
			sampler2D _RimLightSampler;
			sampler2D _SpecularReflectionSampler;
			sampler2D _NormalMapSampler;

			// Structure from vertex shader to fragment shader
			struct v2f
			{
				float4 pos      : SV_POSITION;
				float2 uv       : TEXCOORD0;
				float3 eyeDir   : TEXCOORD1;
				float3 normal   : TEXCOORD2;
				float3 tangent  : TEXCOORD3;
				float3 binormal : TEXCOORD4;
				float3 lightDir : TEXCOORD5;
			};

			// Float types
			#define float_t  half
			#define float2_t half2
			#define float3_t half3
			#define float4_t half4

			// Vertex shader
			v2f vert( appdata_tan v )
			{
				v2f o;
				o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
				o.uv.xy = TRANSFORM_TEX( v.texcoord.xy, _MainTex );
				o.normal = normalize( mul( _Object2World, float4_t( v.normal, 0 ) ).xyz );
				
				// Eye direction vector
				half4 worldPos = mul( _Object2World, v.vertex );
				o.eyeDir.xyz = normalize( _WorldSpaceCameraPos.xyz - worldPos.xyz ).xyz;
				
				// Binormal and tangent (for normal map)
				o.tangent = v.tangent.xyz;
				o.binormal = cross( v.normal, v.tangent.xyz ) * v.tangent.w;
				
				o.lightDir = WorldSpaceLightDir( v.vertex );

				return o;
			}

			// Fragment shader
			float4 frag( v2f i ) : COLOR
			{
				float3_t normalVec = i.normal; // GetNormalFromMap( i );
				float_t normalDotEye = dot( normalVec, i.eyeDir.xyz );
				float_t normalDotLight = dot( normalVec, i.lightDir.xyz );
				float_t falloffU = clamp( 1.0 - abs( normalDotEye ), 0.02, 0.98 );

				//diffuse
				float4_t diffSamplerColor = tex2D( _MainTex, i.uv.xy );
				float3_t combinedColor = diffSamplerColor;
				
				// Falloff
				/**/
				float4_t falloffSamplerColor = _FallOffPower * tex2D( _FalloffSampler, float2( falloffU, 0.25f ) );
				combinedColor += _FallOffColor * falloffSamplerColor.rgb * diffSamplerColor.rgb;
				

				// Specular light
				/**/
				float4_t specularMaskColor = tex2D( _SpecularReflectionSampler, i.uv.xy );
				float4_t lighting = lit( normalDotLight, normalDotEye, _SpecularPower );
				float3_t specularColor = _SpecularAmplifier * saturate( lighting.z ) * specularMaskColor.rgb; // 
				combinedColor += specularColor;
				
				//difuse light
				combinedColor *= _LightColor0.rgb;
				
				// Rimlight
				/**/
				float_t rimlightDot = saturate( 0.5 * ( dot( normalVec, i.lightDir ) + 1.0 ) );
				falloffU = saturate( rimlightDot * falloffU );
				float3_t rimColor = tex2D( _RimLightSampler, float2( falloffU, 0.25f ) );
				combinedColor += _RimColor * rimColor * _RimPower;
				

				// Color Fade
				float4 trueColor = float4( _Color.rgb * combinedColor, 1 );
				float averageVal = 0.3333f * (trueColor.x + trueColor.y + trueColor.z);
				trueColor.x = averageVal + (trueColor.x - averageVal) * _ColorFade;
				trueColor.y = averageVal + (trueColor.y - averageVal) * _ColorFade;
				trueColor.z = averageVal + (trueColor.z - averageVal) * _ColorFade;
				
				return trueColor;
			}

			ENDCG
		}
	}

	FallBack "Diffuse"
}
