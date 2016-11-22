using UnityEngine;
using System.Collections;
using UniRx;

public partial class JoyStick : MonoBehaviour 
{
	public ReactiveProperty<bool> IsJoyStickUsed { get; private set; }

	public ReactiveProperty<Vector3> Vec3JoyStickMoved { get; private set; }

	[SerializeField]
	RectTransform mJoyStickBoard;

	[SerializeField]
	RectTransform mJoyStickNob;

	System.IDisposable[] mDisposeList;

	protected System.IDisposable[] DisposeList 
	{ 
		get { return mDisposeList;  } 
		set { mDisposeList = value; } 
	}

	protected string JoyStickName { get { return mJoyStickBoard.name; } }

	protected Vector2 JoyStickBoardPos { get { return mJoyStickBoard.anchoredPosition; } }

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

		ResetJoyStickPosition ();

		//OperatorForStandalone (_observable);
		OperatorForSmartphone (_observable);
	}

	public void Clear()
	{
		//ClearForStandalone ();
		ClearForSmartphone ();
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

	void SetJoyStickPosition(Vector2 _pos)
	{
		mJoyStickNob.anchoredPosition = _pos;
	}

	void ResetJoyStickPosition()
	{
		mJoyStickNob.anchoredPosition = mJoyStickBoard.anchoredPosition;
	}
}
