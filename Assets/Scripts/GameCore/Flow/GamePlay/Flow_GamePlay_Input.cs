using UnityEngine;
using System.Collections;
using UniRx;

public partial class Flow_GamePlay
{
	public IInput Operator { get { return this; } }

	Subject<Unit> mOnHandleInputSubject = new Subject<Unit>();

	// Use this for initialization
	void InitialInput ()
	{
		ReleaseInput ();

		mOnHandleInputSubject = new Subject<Unit>();
			
		mOnHandleInputSubject.AsObservable().Where (_ => Input.GetKeyUp (KeyCode.Escape))
			.Subscribe ( _ => 
				{
					GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.MAIN_PLAYER, (uint)0);
					GameCore.SetNextFlow(null);
				});

		GameCore.PushInterface (this);
	}

	void ReleaseInput()
	{
		if (mOnHandleInputSubject != null) 
		{
			mOnHandleInputSubject.Dispose ();

			mOnHandleInputSubject = null;

			GameCore.PopPopInterface (this);
		}
	}

	// Update is called once per frame
	public bool HandleInput ()
	{
		mOnHandleInputSubject.OnNext (Unit.Default);

		return true;
	}

	public void Show(Transform _root)
	{
	}

	public void Hide()
	{
	}

	public void SendCommand(uint _inst, params System.Object[] _params)
	{
	}
}
