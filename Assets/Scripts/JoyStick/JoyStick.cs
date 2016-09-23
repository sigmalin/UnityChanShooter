using UnityEngine;
using System.Collections;
using UniRx;

public class JoyStick : MonoBehaviour 
{
	public ReactiveProperty<bool> IsJoyStickUsed { get; private set; }

	public ReactiveProperty<Vector3> Vec3JoyStickMoved { get; private set; }

	[SerializeField]
	RectTransform mJoyStickBoard;

	[SerializeField]
	RectTransform mJoyStickNob;

	System.IDisposable[] mDisposeList;

	bool mIsInvalid;

	void OnDestroy()
	{
		ReleaseDispose ();
	}
	
	// Update is called once per frame
	public void Initial (IObservable<Unit> _observable) 
	{
		mIsInvalid = !CheckJoyStick ();

		if (mIsInvalid == true)
			return;
		
		if (IsJoyStickUsed == null)
			IsJoyStickUsed = new ReactiveProperty<bool> (false);

		if (Vec3JoyStickMoved == null)
			Vec3JoyStickMoved = new ReactiveProperty<Vector3> (Vector3.zero);

		ReleaseDispose ();

		mDisposeList = new System.IDisposable[2];

		mDisposeList[0] = _observable.Where(_ => IsJoyStickUsed.Value == false)
			.Where (_ => Input.GetMouseButtonDown (0) == true)
			.Select (_ => GameCore.InterfaceRaycast (Input.mousePosition))
			.SelectMany (_ => _.ToObservable ())
			.Where (_ => string.Equals (mJoyStickBoard.name, _.gameObject.name))
			.Subscribe (_ => {
				IsJoyStickUsed.Value = true;
				mJoyStickNob.anchoredPosition = Input.mousePosition;
			});

		mDisposeList[1] = _observable.Where(_ => IsJoyStickUsed.Value == true)
			.Do (_ => {
				Vector2 dir = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - mJoyStickBoard.anchoredPosition;
				float dis = Mathf.Min(50f, dir.magnitude);
				dir = dir.normalized;

				Vec3JoyStickMoved.Value = new Vector3(dir.x, 0f, dir.y);
				mJoyStickNob.anchoredPosition = mJoyStickBoard.anchoredPosition + (dir * dis);
			})
			.Where (_ => Input.GetMouseButtonUp (0) == true)
			.Subscribe (_ => {
				IsJoyStickUsed.Value = false;
				mJoyStickNob.anchoredPosition = mJoyStickBoard.anchoredPosition;
			});
	}

	bool CheckJoyStick()
	{
		if (mJoyStickBoard == null || mJoyStickNob == null)
			return false;

		mJoyStickBoard.anchorMin = new Vector2 (0F,0F);
		mJoyStickBoard.anchorMax = new Vector2 (0F,0F);
		mJoyStickBoard.pivot = new Vector2 (0.5F, 0.5F);

		mJoyStickNob.anchorMin = new Vector2 (0F,0F);
		mJoyStickNob.anchorMax = new Vector2 (0F,0F);
		mJoyStickNob.pivot = new Vector2 (0.5F, 0.5F);

		return true;
	}

	void ReleaseDispose()
	{
		if (mDisposeList != null) 
		{
			mDisposeList.ToObservable ()
				.Where (_ => _ != null)
				.Subscribe (_ => _.Dispose ());

			mDisposeList = null;
		}
	}
}
