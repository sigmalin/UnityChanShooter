Shader "PostEffect/ShakeShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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
				float2 uv : TEXCOORD0;
				float2 tab[4] : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};
			
			sampler2D _MainTex;
			half4 _MainTex_TexelSize;
			half4 _ShakeParam;
			half _ShakeTime;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;

				//#if UNITY_UV_STARTS_AT_TOP
				//o.uv.y = 1 - o.uv.y;
				//#endif
				
				half2 diff = _ShakeParam.xy * _MainTex_TexelSize.xy * _ShakeTime;
				o.tab[0] = o.uv + diff;
				o.tab[1] = o.uv - diff;
				o.tab[2] = o.uv + diff * half2(1,-1);
				o.tab[3] = o.uv + diff * half2(-1,1);
				
				return o;
			}
			
			

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				
				col.rgb = (col.rgb * _ShakeParam.z) + (tex2D(_MainTex, i.tab[0]).rgb * _ShakeParam.w);
				col.rgb = (col.rgb * _ShakeParam.z) + (tex2D(_MainTex, i.tab[1]).rgb * _ShakeParam.w);
				col.rgb = (col.rgb * _ShakeParam.z) + (tex2D(_MainTex, i.tab[2]).rgb * _ShakeParam.w);
				col.rgb = (col.rgb * _ShakeParam.z) + (tex2D(_MainTex, i.tab[3]).rgb * _ShakeParam.w);
				
				return col;
			}
			ENDCG
		}
	}
}
