Shader "Custom/Cloud"
{
	Properties
	{
		_MainTex ("Back Cloud Texture", 2D) = "white" {}
		_DatailTex ("Front Cloud Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Front ZWrite Off ZTest LEQUAL
		Lighting Off
		Blend SrcAlpha OneMinusSrcAlpha

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

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _DatailTex;
			float4 _DatailTex_ST;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv0 = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv1 = TRANSFORM_TEX(v.uv, _DatailTex);

				o.uv0 = float2(o.uv0.x + _Time.x, o.uv0.y);
				o.uv1 = float2(o.uv1.x + (_Time.x * 0.5), o.uv1.y);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col0 = tex2D(_MainTex, i.uv0);
				fixed4 col1 = tex2D(_DatailTex, i.uv1);

				return fixed4(lerp(col0.rgb, col1.rgb, col1.a), max(col0.a, col1.a));
			}
			ENDCG
		}
	}
}
