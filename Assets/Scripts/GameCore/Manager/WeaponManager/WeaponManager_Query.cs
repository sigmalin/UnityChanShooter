using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public sealed partial class WeaponManager
{
	System.Object GetHostilityList(uint _mineID)
	{
		WeaponActor mineActor = GetWeaponActor (_mineID);
		if (mineActor == null)
			return (System.Object)null;


		uint[] actorIDs = GetAllActorID ();

		return (System.Object)actorIDs
			.Where (_ => _ != _mineID)
			.Select (_ => GetWeaponActor (_))
			.Where (_ => _.Team != mineActor.Team)
			.Select(_ => _.ActorID)
			.ToArray ();
	}

	System.Object GetAllyList(uint _mineID)
	{
		WeaponActor mineActor = GetWeaponActor (_mineID);
		if (mineActor == null)
			return (System.Object)null;


		uint[] actorIDs = GetAllActorID ();

		return (System.Object)actorIDs
			.Where (_ => _ != _mineID)
			.Select (_ => GetWeaponActor (_))
			.Where (_ => _.Team == mineActor.Team)
			.Select(_ => _.ActorID)
			.ToArray ();
	}
}
