Shader "Unlit/sun_shader"
{
	Properties
	{
		_BaseColor("Base Color", Color) = (1,1,1,1)
		_MainTex ("Super Sun Muffins", 2D) = "white" {}
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
				float3 normal : NORMAL;
				float2 uv1 : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv1 : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float2 normal : NORMAL;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _BaseColor;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = v.normal;
				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 color1 = tex2D(_MainTex, i.uv1) * float4(1.0, 1.0, 1.0, 0.0);
				float4 color2 = tex2D(_MainTex, i.uv2) * float4(0.8, 0.8, 0.8, 0.0);
				return _BaseColor + color1 - color2;
			}
			ENDCG
		}
	}
}