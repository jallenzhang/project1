Shader "Muffin/MuffinPhantom" {
	Properties {
		_normalMap ("normal map", 2D) = "bump" {}
		_rimColor ("Rim Color", Color) = (1, 1, 1, 1)
		_power ("power", Float) = 1
	}
	SubShader {
		Tags 
		{ 
			"RenderType"="Transparent" 
			"LightMode"="ForwardBase"
		}
		
		LOD 200
		
		Pass {
            Name "ForwardBase"
			
            Tags {
                "LightMode"="ForwardBase"
            }
			
			//Blend SrcAlpha OneMinusSrcAlpha
			Blend SrcAlpha One
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//#define UNITY_PASS_FORWARDBASE
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			//#pragma multi_compile_fwdbase_fullshadows
			//#pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
			//#pragma target 3.0

			uniform sampler2D _normalMap; uniform float4 _normalMap_ST;
			uniform float4 _rimColor;
			uniform float _power;

			struct VertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float2 texcoord0 : TEXCOORD0;
			};

			struct VertexOutput {
				float4 pos : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				float4 posWorld : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
				float3 tangentDir : TEXCOORD3;
				float3 binormalDir : TEXCOORD4;
				float3 eyeDir : TEXCOORD5;
				LIGHTING_COORDS(10,11)
			};

			VertexOutput vert (VertexInput v) {
				VertexOutput o;
				o.uv0 = v.texcoord0;
				o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
				o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
				o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
				o.posWorld = mul(_Object2World, v.vertex);
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				
				// Eye direction vector
				o.eyeDir.xyz = normalize( _WorldSpaceCameraPos.xyz - mul( _Object2World, v.vertex )).xyz;
				
				TRANSFER_VERTEX_TO_FRAGMENT(o)
				return o;
			}
			
			float4 frag(VertexOutput i) : COLOR 
			{
			    i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float2 node_64 = i.uv0;
                float3 normalLocal = UnpackNormal(tex2D(_normalMap,TRANSFORM_TEX(node_64.rg, _normalMap))).rgb;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Fallout:
				float normalDotEye = dot( normalDirection, i.eyeDir.xyz );
				float4 retColor = float4(_rimColor.r, _rimColor.g, _rimColor.b, pow(normalDotEye, _power));
				return retColor;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
