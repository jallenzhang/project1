Shader "Muffin/MuffinSkin" {
	Properties {
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_ShinnyPower ("Skin Shinny Power", Float) = 20
		_OutlineThickness ("Outline Thickness", Float) = 1
		_FallOffPower ("Falloff Power", Float) = 0.3
		_ColorFade("Color Fade", Range(0, 1)) = 1

		_MainTex ("Diffuse", 2D) = "white" {}
		_FalloffSampler ("Falloff Control", 2D) = "white" {}
		_RimLightSampler ("RimLight Control", 2D) = "white" {}
		
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

			// Material parameters
			float4 _Color;
			float4 _LightColor0;
			float4 _MainTex_ST;
			float _FallOffPower;
			float _ShinnyPower;
			float _ColorFade;
			
			// Textures
			sampler2D _MainTex;
			sampler2D _FalloffSampler;
			sampler2D _RimLightSampler;

			// Structure from vertex shader to fragment shader
			struct v2f
			{
				float4 pos    : SV_POSITION;
				float3 normal : TEXCOORD0;
				float2 uv     : TEXCOORD1;
				float3 eyeDir : TEXCOORD2;
				float3 lightDir : TEXCOORD3;
			};

			// Float types
			#define float_t  half
			#define float2_t half2
			#define float3_t half3
			#define float4_t half4

			// Vertex shader
			v2f vert( appdata_base v )
			{
				v2f o;
				o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
				o.uv = TRANSFORM_TEX( v.texcoord.xy, _MainTex );
				o.normal = normalize( mul( _Object2World, float4_t( v.normal, 0 ) ).xyz );
	
				// Eye direction vector
				float4_t worldPos =  mul( _Object2World, v.vertex );
				o.eyeDir = normalize( _WorldSpaceCameraPos - worldPos ).xyz;

				o.lightDir = WorldSpaceLightDir( v.vertex );

				return o;
			}

			// Fragment shader
			float4 frag( v2f i ) : COLOR
			{
				float4_t diffSamplerColor = tex2D( _MainTex, i.uv );

				float3_t normalVec = i.normal;

				// Falloff
				/**/
				float_t normalDotEye = dot( i.normal, i.eyeDir );
				float_t falloffU = clamp( 1 - abs( normalDotEye ), 0.02, 0.98 );
				float4_t falloffSamplerColor = _FallOffPower * tex2D( _FalloffSampler, float2( falloffU, 0.25f ) );
				float3_t combinedColor = lerp( diffSamplerColor.rgb, falloffSamplerColor.rgb * diffSamplerColor.rgb, falloffSamplerColor.a );
				
				// shinny
				/**/
				float_t specularDot = dot( normalVec, i.eyeDir.xyz );
				float4_t lighting = lit( normalDotEye, specularDot, 1 );
				float3_t shinnyColor = _ShinnyPower * saturate( lighting.y ) * _LightColor0;
				combinedColor += shinnyColor;

				// Rimlight
				/**/
				float_t rimlightDot = saturate( 0.5 * ( dot( i.normal, i.lightDir ) + 1.0 ) );
				falloffU = saturate( rimlightDot * falloffU );
				float3_t rimColor = tex2D( _RimLightSampler, float2( falloffU, 0.25f ) );
				combinedColor += rimColor;
				

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
			float _OutlineThickness;
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

				half4 projSpacePos = mul( UNITY_MATRIX_MVP, v.vertex );
				half4 projSpaceNormal = normalize( mul( UNITY_MATRIX_MVP, half4( v.normal, 0 ) ) );
				half4 scaledNormal = _OutlineThickness * INV_EDGE_THICKNESS_DIVISOR * projSpaceNormal; // * projSpacePos.w;

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

			// Material parameters
			float4 _Color;
			float4 _LightColor0;
			float4 _MainTex_ST;
			float _FallOffPower;
			float _ShinnyPower;
			float _ColorFade;

			// Textures
			sampler2D _MainTex;
			sampler2D _FalloffSampler;
			sampler2D _RimLightSampler;

			// Structure from vertex shader to fragment shader
			struct v2f
			{
				float4 pos    : SV_POSITION;
				float3 normal : TEXCOORD0;
				float2 uv     : TEXCOORD1;
				float3 eyeDir : TEXCOORD2;
				float3 lightDir : TEXCOORD3;
			};

			// Float types
			#define float_t  half
			#define float2_t half2
			#define float3_t half3
			#define float4_t half4

			// Vertex shader
			v2f vert( appdata_base v )
			{
				v2f o;
				o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
				o.uv = TRANSFORM_TEX( v.texcoord.xy, _MainTex );
				o.normal = normalize( mul( _Object2World, float4_t( v.normal, 0 ) ).xyz );
	
				// Eye direction vector
				float4_t worldPos =  mul( _Object2World, v.vertex );
				o.eyeDir = normalize( _WorldSpaceCameraPos - worldPos ).xyz;

				o.lightDir = WorldSpaceLightDir( v.vertex );

				return o;
			}

			// Fragment shader
			float4 frag( v2f i ) : COLOR
			{
				float4_t diffSamplerColor = tex2D( _MainTex, i.uv );

				float3_t normalVec = i.normal;

				// Falloff
				/**/
				float_t normalDotEye = dot( i.normal, i.eyeDir );
				float_t falloffU = clamp( 1 - abs( normalDotEye ), 0.02, 0.98 );
				float4_t falloffSamplerColor = _FallOffPower * tex2D( _FalloffSampler, float2( falloffU, 0.25f ) );
				float3_t combinedColor = lerp( diffSamplerColor.rgb, falloffSamplerColor.rgb * diffSamplerColor.rgb, falloffSamplerColor.a );
				
				// shinny
				/**/
				float_t specularDot = dot( normalVec, i.eyeDir.xyz );
				float4_t lighting = lit( normalDotEye, specularDot, 1 );
				float3_t shinnyColor = _ShinnyPower * saturate( lighting.y ) * _LightColor0;
				combinedColor += shinnyColor;

				// Rimlight
				/**/
				float_t rimlightDot = saturate( 0.5 * ( dot( i.normal, i.lightDir ) + 1.0 ) );
				falloffU = saturate( rimlightDot * falloffU );
				float3_t rimColor = tex2D( _RimLightSampler, float2( falloffU, 0.25f ) );
				combinedColor += rimColor;
				

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
