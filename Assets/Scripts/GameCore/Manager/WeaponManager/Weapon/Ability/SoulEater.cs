using UnityEngine;
using System.Collections;
using UniRx;

public class SoulEater : IAbility 
{
	public uint AbilityID { get { return AbilitySerial.SOUL_EATER; } }

	public bool IsPassive { get { return true; } }

	public ReadOnlyReactiveProperty<float> Charge { get { return null; } }

	System.IDisposable mDisposable = null;

	public void Initial (uint _ownerID)
	{
		Release ();

		WeaponManager.WeaponActor ownerActor = (WeaponManager.WeaponActor)GameCore.GetParameter (ParamGroup.GROUP_WEAPON, WeaponParam.WEAPON_ACTOR_DATA, _ownerID);
		if (ownerActor == null)
			return;

		mDisposable = ownerActor.Victims
			.Select(_ => (WeaponManager.WeaponActor)GameCore.GetParameter (ParamGroup.GROUP_WEAPON, WeaponParam.WEAPON_ACTOR_DATA, _))
			.Where(_ => _ != null && _.IsDead.Value == true)
			.Subscribe(_ => CreateSoul(_ownerID, _.ActorID));
	}

	public void FrameMove ()
	{
	}

	public void Release ()
	{
		if (mDisposable != null) 
		{
			mDisposable.Dispose ();
			mDisposable = null;
		}
	}

	public void Use ()
	{
	}

	void CreateSoul(uint _ownerID, uint _victimsID)
	{
		Transform transVictims = (Transform)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_TRANSFORM, _victimsID);
		if (transVictims == null)
			return;

		GameObject ability = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.ABILITY, AbilityID);
		if (ability == null)
			return;

		Soul aoul = ability.GetOrAddComponent<Soul> ();
		aoul.SoulOwnerID = _ownerID;

		ability.transform.position = transVictims.position;
	}
}
