Shader "PostEffect/Blood"
{
	Properties
	{
		_MainTex ("MainTex", 2D) = "white" {}
		_BloodTex ("BloodTex", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv0 = v.uv;
				o.uv1 = saturate(float2((v.uv.x - 0.5) + (_SinTime.x * 0.1) + 0.5, (v.uv.y - 0.5) + (_SinTime.x * 0.1) + 0.5));
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _BloodTex;

			half _BloodWeight;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 main = tex2D(_MainTex, i.uv0);
				fixed4 blood = tex2D(_BloodTex, i.uv1);

				blood.rgb = blood.rgb * float3(_BloodWeight, 0, 0); 
				main.rgb = saturate(main.rgb + blood.rgb);

				return main;
			}
			ENDCG
		}
	}
}
