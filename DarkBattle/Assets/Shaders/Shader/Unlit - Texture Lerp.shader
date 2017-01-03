// Unlit shader. Simplest possible textured shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/Texture Lerp" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_OverlayTex("Overlay (RGB)", 2D) = "white" {}
		_Color("Lerp Color (A)", Color) = (0, 0, 0, 0)
	}

	SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		Pass{
			Lighting Off

			SetTexture[_MainTex] {
				combine texture
			}

			SetTexture[_OverlayTex] {
				ConstantColor[_Color]
				Combine texture lerp(constant) previous
			}
		}
	}
}