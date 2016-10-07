using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(RectTransform))]
public class UiTracer : MonoBehaviour 
{
	Transform mTarget;
	public Transform Target { set { mTarget = value; } }

	Camera mMainCamera;
	public Camera MainCamera { set { mMainCamera = value; } }

	RectTransform mRectTrans;

	// Use this for initialization
	void Start () 
	{
		mRectTrans = this.GetComponent<RectTransform> ();

		mRectTrans.anchorMax = Vector2.zero;
		mRectTrans.anchorMin = Vector2.zero;
		mRectTrans.pivot = new Vector2 (0.5f, 0.5f);

		this.UpdateAsObservable ()
			.Subscribe (_ => UpdateScreenPoint());
	}

	void OnDestroy()
	{
		mTarget = null;

		mMainCamera = null;
	}

	public void UpdateScreenPoint()
	{
		if (mTarget == null || mMainCamera == null || mRectTrans == null)
			return;

		mRectTrans.anchoredPosition = mMainCamera.WorldToScreenPoint (mTarget.position);
	}
}
