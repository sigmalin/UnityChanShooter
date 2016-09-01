﻿using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public sealed partial class WeaponManager
{
	public struct FireRequest
	{
		public uint ID;
		public IArm RefArm;

		public FireRequest(uint _id, IArm _arm)
		{
			ID = _id;
			RefArm = _arm;
		}

		FireRequest(FireRequest _src)
		{
			ID = _src.ID;
			RefArm = _src.RefArm;
		}
	}

	ObservableQueue<FireRequest> mRequest = null;

	void InitialFireRequest()
	{
		ReleaseFireRequest ();

		mRequest = new ObservableQueue<FireRequest> ();

		mRequest.Initial (LateUpdateObservable, _ => BatchFireRequest (_));
	}

	void AddFireRequest(uint _id, IArm _arm)
	{
		mRequest.Enqueue (new FireRequest (_id, _arm));
	}

	void ReleaseFireRequest()
	{
		if (mRequest != null) 
		{
			mRequest.Release ();

			mRequest = null;
		}
	}

	void BatchFireRequest(RequestQueue<FireRequest> _request)
	{
		while(_request.IsEmpty () == false)
		{
			FireRequest request = _request.Dequeue();

			OnFire(request.ID, request.RefArm);
		}
	}

	void OnFire(uint _id, IArm _arm)
	{
		WeaponActor actor = GetWeaponActor(_id);
		if (actor == null) return;
		if (actor.AmmoCount == 0) return;
		if (0F < actor.ShootFreq) return;

		actor.ShootFreq = actor.MaxShootFreq;

		uint bulletCount = actor.AmmoCount < actor.MaxShootRayCount ? actor.AmmoCount : actor.MaxShootRayCount;
		actor.AmmoCount -= bulletCount;

		if (_arm == null) return;

		_arm.OnPullTrigger ();

		for (int Indx = 0; Indx < bulletCount; ++Indx) 
		{
			_arm.OnFire (_id, actor.BulletID, actor.ShootATK);
		}
	}
}