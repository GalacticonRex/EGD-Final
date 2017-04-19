Shader "Unlit/sun_shader"
{
	Properties
	{
		_BaseColor("Base Color", Color) = (1,1,1,1)
		_MainTex ("Super Sun Muffins", 2D) = "white" {}
		_SecondTex ("Super Sun Muffins 2", 2D) = "white" {}
		_Reflections("Cubemap", 2D) = "white" {}
		_Normal1("Normal 1", 2D) = "bump" {}

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				//float3 viewDir : NORMAL;
				float2 uv1 : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv1 : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float3 viewDir : NORMAL;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _SecondTex;
			sampler2D _Reflections;
			sampler2D _Normal1;
			float4 _MainTex_ST;
			float4 _SecondTex_ST;
			float4 _BaseColor;
			float4 _Normal1_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.normal = v.normal;
				o.uv1 = v.uv1 * _MainTex_ST.xy + _MainTex_ST.zw;
				o.uv2 = v.uv2 * _SecondTex_ST.xy + _SecondTex_ST.zw;
				o.uv1 = v.uv1 * _Normal1_ST.xy + _Normal1_ST.zw;
				o.viewDir = WorldSpaceViewDir(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 n1_xzy = normalize(UnpackNormal(tex2D(_Normal1, i.uv1)));
				float3 n1 = n1_xzy.xzy;
				float3 r1 = n1 - 2 * dot(n1, i.viewDir) * n1;

				float4 color1 = tex2D(_MainTex, i.uv1) * float4(1.0, 1.0, 1.0, 0.0);
				float4 color2 = tex2D(_SecondTex, i.uv2) * float4(0.8, 0.8, 0.8, 0.0);
				float4 color3 = tex2D(_Reflections, r1.xy * 0.01f);
				return color3;//_BaseColor + color1 - color2;
			}
			ENDCG
		}
	}
}