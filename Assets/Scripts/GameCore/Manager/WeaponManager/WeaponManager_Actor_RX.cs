﻿using UnityEngine;
using System.Linq;
using UniRx;
using UniRx.Triggers;

public sealed partial class WeaponManager
{
	void InitialActorObservable()
	{
		this.UpdateAsObservable ()
			.Subscribe (_ => 
				{
					uint[] actorIDs = mActorTable.Keys.ToArray ();

					for(int Indx = 0; Indx < actorIDs.Length; ++Indx)
					{
						WeaponActor actor = GetWeaponActor(actorIDs[Indx]);
						if (actor == null) continue;

						actor.ShootFreq = actor.ShootFreq - Time.deltaTime;
						if (actor.ShootFreq < 0F) actor.ShootFreq = 0F;

						if (actor.AmmoCount < actor.MaxAmmoCount)
						{
							actor.ReloadTime -= Time.deltaTime;
							if (actor.ReloadTime <= 0F)
							{
								actor.ReloadTime = actor.MaxReloadTime;
								++actor.AmmoCount;
							}
						}
					}
				});
	}
}
