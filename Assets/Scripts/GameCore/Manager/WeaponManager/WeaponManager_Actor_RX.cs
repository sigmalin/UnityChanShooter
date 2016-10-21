using UnityEngine;
using System.Linq;
using UniRx;
using UniRx.Triggers;

public sealed partial class WeaponManager
{
	System.IDisposable mShootFreqDisposable = null;

	System.IDisposable mReloadDisposable = null;

	void InitialActorObservable()
	{
		ReleaseActorObservable ();

		IObservable<WeaponActor> actorObserver = UpdateObservable
			.SelectMany (_ => GetAllActorID ().ToObservable ())
			.Select (_ => GetWeaponActor (_))
			.Where (_ => _ != null).Publish().RefCount();

		mReloadDisposable = actorObserver
			.Where (_ => _.AmmoCount.Value < _.MaxAmmoCount)
			.Do (_ => _.ReloadTime -= Time.deltaTime)
			.Where (_ => _.ReloadTime <= 0F)
			.Subscribe (
				_ => 
				{
					_.ReloadTime = _.MaxReloadTime;
					++_.AmmoCount.Value;
				});
	}

	void ReleaseActorObservable()
	{
		if (mShootFreqDisposable != null) 
		{
			mShootFreqDisposable.Dispose ();
			mShootFreqDisposable = null;
		}

		if (mReloadDisposable != null) 
		{
			mReloadDisposable.Dispose ();
			mReloadDisposable = null;
		}
	}
}
