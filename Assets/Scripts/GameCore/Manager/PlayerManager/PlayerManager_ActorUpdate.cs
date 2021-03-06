﻿using UnityEngine;
using System.Collections;
using UniRx;

public partial class PlayerManager
{
	System.IDisposable mActorDisposable = null;

	void InitialActorUpdate()
	{
		ReleaseActorUpdate ();

		mActorDisposable = UpdateObservable.Select (_ => GetAllPlayerID ())
			.SelectMany (_ => _.ToObservable ())
			.Select(_ => GetPlayerData(_))
			.Where(_ => _ != null)
			.Subscribe (_ => _.FrameMove ());
	}

	void ReleaseActorUpdate()
	{
		if (mActorDisposable != null) 
		{
			mActorDisposable.Dispose ();

			mActorDisposable = null;
		}
	}
}
