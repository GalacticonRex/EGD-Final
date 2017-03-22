Shader "Custom/beacon_shader" {
	Properties {
		_Mode("Mode", Range(0,1)) = 1.0
		_ModeColor("Mode Color", Color) = (1,1,1,1)

		_OriginPoint("Origin Point", Vector) = (0,0,0,0)
		_BandOffset("Band Offset", Float) = 0.0
		_BandRadius("Band Radius", Float) = 0.0
		_BandColor("Band Color", Color) = (1,1,1,1)

		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
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

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		float4 _OriginPoint;
		float _BandOffset;
		float _BandRadius;
		fixed4 _BandColor;

		float _Mode;
		fixed4 _ModeColor;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			float dif = abs(_BandOffset - _OriginPoint.y - IN.worldPos.y);
			float amount = clamp(dif/_BandRadius, 0, 1);

			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color + _BandColor * clamp(_BandRadius, 0, 0.1);
			fixed4 col = c * amount + _BandColor * (1.0f - amount);

			o.Albedo = col.rgb * _Mode + _ModeColor.rgb * (1.0f - _Mode);

			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic * _Mode;
			o.Smoothness = _Glossiness * _Mode;
			o.Emission = (1.0f - amount) / 2.0f * _Mode;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
