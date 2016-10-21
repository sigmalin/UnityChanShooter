using UnityEngine;
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
		if (actor.IsDead.Value == true) return;
		if (actor.AmmoCount.Value == 0) return;

		uint bulletCount = actor.AmmoCount.Value < actor.MaxShootRayCount ? actor.AmmoCount.Value : actor.MaxShootRayCount;
		actor.AmmoCount.Value -= bulletCount;

		if (_arm == null) return;

		_arm.OnPullTrigger ();

		for (int Indx = 0; Indx < bulletCount; ++Indx) 
		{
			_arm.OnFire (_id, actor.BulletID, actor.ShootATK, GetAttackLayer(actor));
		}
	}

	int GetAttackLayer(WeaponActor _actor)
	{
		WeaponActor mainActor = GetWeaponActor (mMainActorID);

		return _actor.Team == mainActor.Team ? 
			GameCore.GetRaycastLayer (GameCore.LAYER_DEFAULT) | GameCore.GetRaycastLayer (GameCore.LAYER_ENEMY) :
			GameCore.GetRaycastLayer (GameCore.LAYER_DEFAULT) | GameCore.GetRaycastLayer (GameCore.LAYER_PLAYER);
	}
}
