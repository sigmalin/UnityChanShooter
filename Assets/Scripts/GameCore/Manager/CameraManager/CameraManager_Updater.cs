﻿using UnityEngine;
using System.Collections;
using UniRx;

public partial class CameraManager
{
	System.IDisposable mCameraDisposable = null;

	void InitialUpdater () 
	{
		mCameraDisposable = LateUpdateObservable
			.Select(_ => GetAllCameraID ())
			.SelectMany (_ => _.ToObservable ())
			.Select(_ => GetCameraData(_))
			.Where (_ => _ != null)
			.Subscribe (_ => _.RefCamera.FrameMove());
	
	}

	void ReleaseUpdater () 
	{
		if (mCameraDisposable != null) 
		{
			mCameraDisposable.Dispose ();

			mCameraDisposable = null;
		}	
	}
}