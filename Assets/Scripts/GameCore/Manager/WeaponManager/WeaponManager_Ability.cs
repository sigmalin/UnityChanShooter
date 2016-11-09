using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public sealed partial class WeaponManager
{
	IAbility GetAbility(uint _ownerID, uint _abilityID)
	{
		IAbility ability = null;

		switch (_abilityID) 
		{
		case AbilitySerial.TELEPORT:
			ability = new Teleport ();
			break;

		case AbilitySerial.SOUL_EATER:
			ability = new SoulEater ();
			break;

		case AbilitySerial.CONCENTRATE:
			ability = new Concentrate ();
			break;
		}

		if (ability != null)
			ability.Initial (_ownerID);

		return ability;
	}

	void ReleaseAbility(WeaponActor _actor)
	{
		if (_actor.Abilities != null) 
		{
			_actor.Abilities.ToObservable ()
				.Where (_ => _ != null)
				.Subscribe (_ => _.Release ());

			_actor.Abilities = null;
		}
	}

	void UseAbility(uint _ownerID, uint _abilityID)
	{
		WeaponActor actor = GetWeaponActor (_ownerID);
		if (actor == null)
			return;

		if (actor.Abilities != null) 
		{
			actor.Abilities.ToObservable ()
				.Where (_ => _ != null && _.AbilityID == _abilityID)
				.FirstOrDefault()
				.Where (_ => _ != default(IAbility))
				.Subscribe (_ => _.Use ());
		}
	}
}
