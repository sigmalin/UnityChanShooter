﻿using UnityEngine;
using System.Collections;

public sealed partial class WeaponManager
{
	GameObject mWeaponUI;

	void InitialWeaponLauncher()
	{
		ReleaseWeaponUI ();
	}

	void ReleaseWeaponLauncher()
	{
		ReleaseWeaponUI ();
	}

	void ReleaseWeaponUI()
	{
		if (mWeaponUI != null) 
		{
			GameCore.PopInterface (mWeaponUI.GetComponent<IUserInterface> ());

			GameObject.Destroy (mWeaponUI);

			mWeaponUI = null;
		}
	}

	void PushWeaponInterface()
	{
		if (mWeaponUI != null) 
		{
			IUserInterface ui = mWeaponUI.GetComponent<IUserInterface> ();
			if (ui != null) GameCore.PushInterface (ui);
		}
	}

	void PopWeaponInterface()
	{
		if (mWeaponUI != null) 
		{
			IUserInterface ui = mWeaponUI.GetComponent<IUserInterface> ();
			if (ui != null) GameCore.PopInterface (ui);
		}
	}

	void SetWeaponInterface(WeaponActor _actor)
	{
		ReleaseWeaponUI ();

		mWeaponUI = (GameObject	)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.INSTANT_RESOURCE_INPUT, TransWeaponBehavior2DevicePath (_actor.RefWeaponData.WeaponBehavior));
		if (mWeaponUI != null) 
		{
			IUserInterface ui = mWeaponUI.GetComponent<IUserInterface> ();
			if (ui != null) ui.Operation (WeaponUiBehavior.InstSet.SET_ACTOR_ID, _actor.ActorID);

			PushWeaponInterface ();
		}
	}

	void SetActorController(WeaponActor _actor)
	{
		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.SET_ACTOR_CONTROLLER, _actor.ActorID, TransWeaponBehavior2ActorController(_actor.RefWeaponData.WeaponBehavior));
	}

	void SetWeaponModel(WeaponActor _actor)
	{
		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.SET_WEAPON, _actor.ActorID, _actor.RefWeaponData.ModelID);
	}

	void PreloadResource(WeaponActor _actor)
	{
		// Bullet
		GameObject bullet = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.BULLET, _actor.BulletID);
		if (bullet != null)
			bullet.SafeRecycle ();

		// Skill
	}

	ActorController TransWeaponBehavior2ActorController(WeaponDataRepository.Behavior _behavior)
	{
		ActorController res = null;

		switch (_behavior) 
		{
		case WeaponDataRepository.Behavior.Shootgun:
			res = new Actor_UnityChan ();
			break;
		}

		return res;
	}

	string TransWeaponBehavior2DevicePath(WeaponDataRepository.Behavior _behavior)
	{
		string res = string.Empty;

		switch (_behavior) 
		{
		case WeaponDataRepository.Behavior.Shootgun:
			res = "Shootgun";
			break;
		}

		return res;
	}
}
