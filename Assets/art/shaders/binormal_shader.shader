Shader "Custom/binormal_shader" {
	Properties {
		_Normal1 ("Normal 1", 2D) = "bump" {}
		_Normal2 ("Normal 2", 2D) = "bump" {}
		_Glossiness ("Smoothness", Range(0,1)) = 1.0
		_Metallic ("Metallic", Range(0,1)) = 1.0
		_Reflections("Cubemap", CUBE) = "" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		//Blend One One
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		sampler2D _Normal1;
		sampler2D _Normal2;
			samplerCUBE _Reflections;

		struct Input {
			float3 viewDir;
			float3 worldRefl;
			float2 uv_Normal1;
			float2 uv2_Normal2;
			float4 vertex_color : COLOR;
			float3 worldNormal;
		};

		void surf (Input IN, inout SurfaceOutput  o) {
			float3 n1 = UnpackNormal(tex2D(_Normal1, IN.uv_Normal1));
			float3 n2 = UnpackNormal(tex2D(_Normal2, IN.uv2_Normal2));

			float3 r1 = n1 - 2 * dot(n1, IN.viewDir) * n1;
			float3 r2 = n2 - 2 * dot(n2, IN.viewDir) * n2;

			float4 color1 = texCUBE(_Reflections, r1);
			float4 color2 = texCUBE(_Reflections, r2);
			float4 color3 = texCUBE(_Reflections, IN.worldRefl);

			o.Albedo = color3;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
