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

	// Use this for initialization
	void Start () 
	{
		RectTransform rectTrans = this.GetComponent<RectTransform> ();

		rectTrans.anchorMax = Vector2.zero;
		rectTrans.anchorMin = Vector2.zero;
		rectTrans.pivot = new Vector2 (0.5f, 0.5f);

		this.OnEnableAsObservable()
			.Where (_ => mTarget != null && mMainCamera != null)
			.Subscribe (_ => 
				{
					rectTrans.anchoredPosition = mMainCamera.WorldToScreenPoint (mTarget.position);
				});

		this.UpdateAsObservable ()
			.Where (_ => mTarget != null && mMainCamera != null)
			.Subscribe (_ => 
				{
					rectTrans.anchoredPosition = mMainCamera.WorldToScreenPoint (mTarget.position);
				});
	}

	void OnDestroy()
	{
		mTarget = null;

		mMainCamera = null;
	}
}
