Shader "Custom/ColorOnly" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass
		{
			Color [_Color]
		}
	}
	FallBack "Diffuse"
}
