Shader "Custom/asteroid_shader" {
	Properties {
		_Mode("Mode", Range(0,1)) = 1.0
		_ModeColor("Mode Color", Color) = (1,1,1,1)

		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
		_Metallic ("Metallic", 2D) = "black" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _Metallic;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		fixed4 _Color;

		float _Mode;
		fixed4 _ModeColor;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb * _Mode + _ModeColor.rgb * (1.0f - _Mode);
			o.Metallic = tex2D(_MainTex, IN.uv_MainTex).r * _Mode;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
			o.Smoothness = tex2D(_MainTex, IN.uv_MainTex).r * _Mode;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
