Shader "Custom/scanner_shader" {
	Properties {
		_Mode("Mode", Range(0,1)) = 1.0
		_ModeColor("Mode Color", Color) = (1,1,1,1)

		_Color ("Color", Color) = (1,1,1,1)
		_Albedo("Albedo (RGB)", 2D) = "white" {}
		_Normal("Normal Map", 2D) = "bump" {}
		_Metallic("Metallic", 2D) = "black" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Emission("Emission", 2D) = "black" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200
		Cull Off
		ZWrite Off
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _Albedo;
		sampler2D _Normal;
		sampler2D _Metallic;
		sampler2D _Emission;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		fixed4 _Color;

		float _Mode;
		fixed4 _ModeColor;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_Albedo, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb * _Mode + _ModeColor.rgb * (1.0f-_Mode);

			// Metallic and smoothness come from slider variables
			o.Normal = UnpackNormal(tex2D(_Albedo, IN.uv_MainTex));
			o.Metallic = tex2D(_Metallic, IN.uv_MainTex) * _Mode;

			o.Smoothness = _Glossiness * _Mode;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
