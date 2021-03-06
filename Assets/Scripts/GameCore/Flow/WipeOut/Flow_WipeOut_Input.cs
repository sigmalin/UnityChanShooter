﻿using UnityEngine;
using System.Collections;
using UniRx;

public partial class Flow_WipeOut
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

			GameCore.PopInterface (this);
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

	public void Localization()
	{
	}

	public void Clear()
	{
	}

	public void Operation(uint _inst, params System.Object[] _params)
	{
	}
}
