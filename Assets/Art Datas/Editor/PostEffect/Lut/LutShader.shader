Shader "PostEffect/LutShader"
{
	Properties
	{
		_MainTex ("MainTex", 2D) = "white" {}
		_LutTex ("LutTex", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		ZTest Always Cull Off ZWrite Off Blend Off
		Fog { Mode off }
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag_1024
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#include "UnityCG.cginc"

			
			sampler2D _MainTex;
			sampler2D _LutTex;
			
			half4 frag (v2f_img i) : COLOR
			{			
				fixed4 col = tex2D(_MainTex, i.uv);
				
				//half zSlice0 = min(floor(col.b * 15.0), 15.0);
				//half zSlice1 = min(zSlice0 + 1.0, 15.0);
				//half xOffset = col.r * 0.05859375;
				
				//half u0 = xOffset + (zSlice0 * 0.0625);
				//half u1 = xOffset + (zSlice1 * 0.0625);
				//half v = 1 - col.g;
				
				//fixed4 result1 = tex2D(_LutTex, half2(u0, v));
				//fixed4 result2 = tex2D(_LutTex, half2(u1, v));
				
				//half zOffset = frac(col.b * 15.0);
				
				//return lerp(result1,result2,frac(zOffset));
				
				half zSlice0 = min(floor(col.b * 15.0), 15.0);
				half zSlice1 = min(zSlice0 + 1.0, 15.0);
				half xOffset = (15.0 * col.r + 0.5) * 0.00390625; //1 / 256 * 0.5 + r * 15 / 256;
				
				half u0 = xOffset + (zSlice0 * 0.0625);
				half u1 = xOffset + (zSlice1 * 0.0625);
				
				half v = ((15.0 * col.g + 0.5) * 0.0625);
				
				fixed4 result1 = tex2D(_LutTex, half2(u0, v));
				fixed4 result2 = tex2D(_LutTex, half2(u1, v));
				
				half zOffset = frac(col.b * 15.0);
				
				return lerp(result1,result2,zOffset);
			}
			
			half4 frag_1024 (v2f_img i) : COLOR
			{
				const half4 coord_scale = half4( 0.0302734375, 0.96875, 31.0, 0.0 );
				const half4 coord_offset = half4( 0.00048828125, 0.015625, 0.0, 0.0 );
				const half2 texel_height_X0 = half2( 0.03125, 0.0 );
				
				fixed4 col = tex2D(_MainTex, i.uv);
				
				half4 coord = col * coord_scale + coord_offset;
				
				half4 coord_frac = frac(coord);
				half4 coord_floor = coord - coord_frac;

				half2 coord_bot = coord.xy + coord_floor.zz * texel_height_X0;
				half2 coord_top = coord_bot + texel_height_X0;

				half4 lutcol_bot = tex2D(_LutTex, coord_bot );
				half4 lutcol_top = tex2D(_LutTex, coord_top );

				return lerp(lutcol_bot, lutcol_top, coord_frac.z);
			}
			
			ENDCG
		}
	}
}
