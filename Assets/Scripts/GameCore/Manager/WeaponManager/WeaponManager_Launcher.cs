using UnityEngine;
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

	void SetMainActor(uint _id)
	{
		WeaponActor actor = GetWeaponActor (_id);

		SetWeaponInterface (ref actor);

		SetMainCamera (ref actor);
	}

	void SetWeaponInterface(ref WeaponActor _actor)
	{
		ReleaseWeaponUI ();

		mWeaponUI = (GameObject	)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.INSTANT_RESOURCE_INPUT, TransWeaponBehavior2DevicePath (_actor.RefWeaponData.WeaponBehavior));
		if (mWeaponUI != null) 
		{
			IUserInterface ui = mWeaponUI.GetComponent<IUserInterface> ();
			if (ui != null) 
			{
				ui.Operation (WeaponUiBehavior.InstSet.SET_ACTOR_ID, _actor.ActorID);
				GameCore.PushInterface (ui);
			}
		}
	}

	void SetMainCamera(ref WeaponActor _actor)
	{
		uint cameraID = (uint)GameCore.GetParameter (ParamGroup.GROUP_CAMERA, CameraParam.MAIN_CAMERA);

		GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.CAMERA_TARGET, cameraID, _actor.ActorID);

		GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.SET_CAMERA_MODE, cameraID, TransWeaponBehavior2CameraMode(_actor.RefWeaponData.WeaponBehavior));
	}

	void SetActorController(ref WeaponActor _actor)
	{
		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.SET_ACTOR_CONTROLLER, _actor.ActorID, TransWeaponBehavior2ActorController(_actor.RefWeaponData.WeaponBehavior));
	}

	void SetWeaponModel(ref WeaponActor _actor)
	{
		GameObject weapon = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.WEAPON_MODEL, _actor.RefWeaponData.WeaponID);
		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.SET_WEAPON, _actor.ActorID, weapon);
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

	IMode TransWeaponBehavior2CameraMode(WeaponDataRepository.Behavior _behavior)
	{
		IMode res = null;

		switch (_behavior) 
		{
		case WeaponDataRepository.Behavior.Shootgun:
			res = new Mode_Follow();
			break;
		}

		return res;
	}
}
