using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniRx;

public class DevelopTestCode : MonoBehaviour 
{
	Subject<float> mSubject = new Subject<float>();

	public Transform[] mUI;

	public Transform mCanvas;

	public int pickIndx = 0;

	public UnityEngine.UI.Button button;

	public RenderTexture mTex = null;

	public Material mMat;

	// Use this for initialization
	void Start () 
	{
		//button.OnClickAsObservable ()
		//	.Subscribe (_ => Debug.Log ("yaya"));

		mTex = RenderTexture.GetTemporary (128,128);

		mSubject.AsObservable ()
			.Where (_ => Input.GetMouseButtonDown (0)).First ()
			.SelectMany (mSubject.AsObservable ()
				.Where (_ => Input.GetMouseButtonUp (0)).First ())
			.Subscribe (_ => DrawGraph (mTex, new float[]{1,1,1,1,1}));
	}

	void OnDestroy()
	{
		if (mTex != null) 
		{
			RenderTexture.ReleaseTemporary (mTex);

			mTex = null;
		}
	}

	void Update()
	{
		if(mSubject != null)
			mSubject.OnNext (Time.deltaTime);
	}

	void Move2Canvas()
	{
		mUI [pickIndx].SetParent(mCanvas);
		mUI [pickIndx].gameObject.SetActive (true);

		++pickIndx;
		if (mUI.Length <= pickIndx)
			pickIndx = 0;
	}

	void Recover()
	{
		for (int Indx = 0; Indx < mUI.Length; ++Indx) 
		{
			mUI [Indx].gameObject.SetActive (false);
			mUI [Indx].SetParent(this.transform);
		}

		pickIndx = Random.Range (0, mUI.Length);
	}

	void DrawGraph(RenderTexture _rt, float[] _params)
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

		//mMat.SetPass(0);


		GL.Begin (GL.TRIANGLES);
		GL.Color (Color.red);
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
		//GL.Vertex3 (0f,0f,0f);
		//GL.Vertex3 (0.5f,1f,0f);
		//GL.Vertex3 (1f,0f,0f);



		GL.PopMatrix ();
	}

	void Test()
	{
		float[] num = new float[] { 1, 2, 3, 4, 5 };

		float angle = 90F * (Mathf.PI / 180f);

		float stepA = (360f / num.Length) * (Mathf.PI / 180f);

		Vector3 center = new Vector3 (0.5f,0.5f,0f);


		float[] cat = num.Concat (new float[]{num[0]}).ToArray();


		Debug.Log ("cat = " + cat.Length);

		foreach (float item in cat) {
			Debug.Log ("item = " + item);
		}
		/*
		num
			.Select (_ => {
			Vector3 res = new Vector3 (_ * 0.5f * Mathf.Cos (angle), _ * 0.5f * Mathf.Sin (angle), 0F);
			angle -= stepA;
				Debug.Log(" angle_cur = " + angle);
				return res + center;
		})
			.Aggregate ((_pre, _cur) => {
			Debug.Log ("center = " + center);
			Debug.Log ("_pre = " + _pre);
				Debug.Log ("_cur = " + (_cur));
			return _cur;
		});
		*/
	}
}
