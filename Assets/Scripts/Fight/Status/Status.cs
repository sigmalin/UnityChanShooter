using UnityEngine;
using System.Collections;
using UniRx;

public partial class Status : MonoBehaviour 
{
	uint mActorID = 0;

	void OnDestroy()
	{
		ReleaseItemPool ();

		ReleasePortrait ();
	}

	public void Initial(uint _actorID)
	{
		WeaponManager.WeaponActor mineActor = (WeaponManager.WeaponActor)GameCore.GetParameter (ParamGroup.GROUP_WEAPON, WeaponParam.WEAPON_ACTOR_DATA, _actorID);
		if (mineActor == null)
			return;

		mActorID = _actorID;

		InitialPortrait (mineActor);

		UpdateList (mineActor);
	}

	public void Localization()
	{
		WeaponManager.WeaponActor mineActor = (WeaponManager.WeaponActor)GameCore.GetParameter (ParamGroup.GROUP_WEAPON, WeaponParam.WEAPON_ACTOR_DATA, mActorID);
		if (mineActor == null)
			return;
		
		UpdateList (mineActor);
	}
}
