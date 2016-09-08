using UnityEngine;
using System.Collections;
using System.Linq;

public static class GraphUtil
{
	public static void DrawAnalysisGraphic(RenderTexture _rt, Color _col, float[] _params)
	{
		if (_rt == null)
			return;

		if (_params == null || _params.Length < 3)
			return;


		float max = _params.Max ();

		float[] nor = _params.Select (_ => max == 0F ? 0F : _ / max).ToArray();

		float[] points = nor.Concat (new float[] { nor [0] }).ToArray();

		float stepAngle = (360f / _params.Length) * (Mathf.PI / 180f);

		float angle = 90F * (Mathf.PI / 180f);


		Graphics.SetRenderTarget (_rt);

		GL.Clear (true, true, new Color (0f, 0f, 0f, 0f));

		GL.PushMatrix ();

		GL.LoadOrtho ();

		GL.Begin (GL.TRIANGLES);

		GL.Color (_col);

		points
			.Select (_ => {
				float radius = _ * 0.5f;
				Vector2 res = new Vector2 (radius * Mathf.Cos (angle) + 0.5f, radius * Mathf.Sin (angle) + 0.5f);
				angle -= stepAngle;
				return res;
			})
			.Aggregate ((_pre, _cur) => {

				GL.Vertex3 (0.5f, 0.5f, 0f);
				GL.Vertex3 (_pre.x, _pre.y, 0f);
				GL.Vertex3 (_cur.x, _cur.y, 0f);
				return _cur;
			});

		GL.End ();

		GL.PopMatrix ();
	}
}
