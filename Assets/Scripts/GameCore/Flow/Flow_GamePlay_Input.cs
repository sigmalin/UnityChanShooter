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
		mOnHandleInputSubject.Where (_ => Input.GetKeyUp (KeyCode.Escape))
			.BatchFrame(0, FrameCountType.EndOfFrame)
			.Subscribe ( _ => GameCore.SetFlow(null) );

		GameCore.PushInput (this);
	}

	// Update is called once per frame
	public bool HandleInput ()
	{
		mOnHandleInputSubject.OnNext (Unit.Default);

		return true;
	}
}
