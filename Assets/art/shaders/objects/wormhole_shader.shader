Shader "Unlit/wormhole_shader"
{
	Properties
	{
		_Normal1("Normal 1", 2D) = "bump" {}
		_Normal2("Normal 2", 2D) = "bump" {}
		_Reflections("Cubemap", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100
		Blend One One
		Cull Off

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
				float2 uv1 : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv1 : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float4 vertex : SV_POSITION;
				float3 viewDir : NORMAL;
				float4 color : COLOR;
			};

			sampler2D _Normal1;
			sampler2D _Normal2;

			float4 _Normal1_ST;
			float4 _Normal2_ST;

			sampler2D _Reflections;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.viewDir = WorldSpaceViewDir(v.vertex);
				o.uv1 = v.uv1 * _Normal1_ST.xy + _Normal1_ST.zw;
				o.uv2 = v.uv2 * _Normal2_ST.xy + _Normal2_ST.zw;
				o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 n1_xzy = normalize(UnpackNormal(tex2D(_Normal1, i.uv1)));
				float3 n2_xzy = normalize(UnpackNormal(tex2D(_Normal2, i.uv2)));

				float3 n1 = n1_xzy.xzy;
				float3 n2 = n2_xzy.xzy;

				float3 r1 = n1 - 2 * dot(n1, i.viewDir) * n1;
				float3 r2 = n2 - 2 * dot(n2, i.viewDir) * n2;

				float4 color1 = tex2D(_Reflections, r1.yz * 0.01f);
				float4 color2 = tex2D(_Reflections, r2.yz * 0.01f);
				float4 inv_color = float4(1, 1, 1, 1) - i.color;

				float4 result = i.color * (color1 * 0.5 + color2 * 0.15) / 0.65;

				return float4(result.xyz, 1);
			}
			ENDCG
		}
	}
}
