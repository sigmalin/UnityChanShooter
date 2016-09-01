using UnityEngine;
using System.Collections;
using UniRx;

public partial class Flow_GamePlay
{
	public IUserInterface UserInterface { get { return null; } }

	Subject<Unit> mOnHandleInputSubject = new Subject<Unit>();

	// Use this for initialization
	void InitialInput ()
	{
		ReleaseInput ();

		mOnHandleInputSubject = new Subject<Unit>();
			
		mOnHandleInputSubject.AsObservable().Where (_ => Input.GetKeyUp (KeyCode.Escape))
			.BatchFrame(0, FrameCountType.EndOfFrame)
			.Subscribe ( _ => GameCore.SetFlow(null) );

		GameCore.PushInput (this);
	}

	void ReleaseInput()
	{
		if (mOnHandleInputSubject != null) 
		{
			mOnHandleInputSubject.Dispose ();

			mOnHandleInputSubject = null;
		}
	}

	// Update is called once per frame
	public bool HandleInput ()
	{
		mOnHandleInputSubject.OnNext (Unit.Default);

		return true;
	}
}
