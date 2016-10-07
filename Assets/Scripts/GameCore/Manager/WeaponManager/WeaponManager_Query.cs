using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public sealed partial class WeaponManager
{
	uint GetLastestMurdererID(uint _actorID)
	{
		WeaponActor actor = GetWeaponActor (_actorID);
		return actor == null ? 0u : actor.MurdererID;
	}

	uint[] GetHostilityList(uint _mineID)
	{
		WeaponActor mineActor = GetWeaponActor (_mineID);
		if (mineActor == null)
			return null;


		uint[] actorIDs = GetAllActorID ();

		return actorIDs
			.Where (_ => _ != _mineID)
			.Select (_ => GetWeaponActor (_))
			.Where (_ => _.Team != mineActor.Team)
			.Select(_ => _.ActorID)
			.ToArray ();
	}

	uint[] GetAllyList(uint _mineID)
	{
		WeaponActor mineActor = GetWeaponActor (_mineID);
		if (mineActor == null)
			return null;


		uint[] actorIDs = GetAllActorID ();

		return actorIDs
			.Where (_ => _ != _mineID)
			.Select (_ => GetWeaponActor (_))
			.Where (_ => _.Team == mineActor.Team)
			.Select(_ => _.ActorID)
			.ToArray ();
	}

	int GetActiveHostilityCount()
	{		
		uint[] actorIDs = GetHostilityList (MainActorID);

		return actorIDs
			.Select (_ => GetWeaponActor (_))
			.Where (_ => _.IsDead.Value == false)
			.Count ();
	}
}
