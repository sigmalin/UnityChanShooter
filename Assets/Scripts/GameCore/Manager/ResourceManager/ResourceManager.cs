﻿using UnityEngine;
using System.Collections;

public sealed partial class ResourceManager : CommandBehaviour, IParam, IRegister
{
	// Use this for initialization
	void Start () 
	{
		InitialRequestQueue ();

		InitialCharacterData ();
		InitialContainerData ();
		InitialGameData ();
		InitialWeaponData ();
		InitialBulletData ();
	}

	public void OnRegister ()
	{
		GameCore.RegisterCommand (CommandGroup.GROUP_RESOURCE, this);	

		GameCore.RegisterParam (ParamGroup.GROUP_RESOURCE, this);
	}

	public void OnUnRegister ()
	{
		ReleaseAllResource ();

		GameCore.UnRegisterCommand (CommandGroup.GROUP_RESOURCE);

		GameCore.UnRegisterParam (ParamGroup.GROUP_RESOURCE);
	}

	//public void ExecCommand (uint _inst, params System.Object[] _params)
	protected override void BatchCommand (uint _inst, params System.Object[] _params)
	{
		switch(_inst)
		{
		case ResourceInst.RELEASE_ALL:
			ReleaseAllResource ();
			break;

		case ResourceInst.RELEASE_CHARACTER_MODEL:
			ReleaseCharacterData ();
			break;

		case ResourceInst.RELEASE_CONTAINER:
			ReleaseContainerData ();
			break;

		case ResourceInst.RELEASE_WEAPON:
			ReleaseWeaponData ();
			break;

		case ResourceInst.RELEASE_BULLET:
			ReleaseBulletData ();
			break;

		case ResourceInst.RECYCLE_CHARACTER_MODEL:
			RecycleCharacter ((uint)_params[0], (GameObject)_params[1]);
			break;

		case ResourceInst.RECYCLE_CONTAINER:
			RecycleContainer ((uint)_params[0], (GameObject)_params[1]);
			break;

		case ResourceInst.RECYCLE_WEAPON_MODEL:
			RecycleWeapon ((uint)_params[0], (GameObject)_params[1]);
			break;

		case ResourceInst.RECYCLE_BULLET:
			RecycleBullet ((uint)_params[0], (GameObject)_params[1]);
			break;

		case ResourceInst.RELEASE_WEAPON_DATA:
			ReleaseGameData (WEAPON_DATA);
			break;
		}
	
	}

	public System.Object GetParameter (uint _inst, params System.Object[] _params)
	{
		System.Object output = default(System.Object);

		switch (_inst) 
		{
		case ResourceParam.CHARACTER_MODEL:
			output = GetCharacter ((uint)_params[0]);
			break;

		case ResourceParam.CONTAINER:
			output = GetContainer ((uint)_params[0]);
			break;

		case ResourceParam.WEAPON_DATA:
			output = GetGameData (WEAPON_DATA);
			break;

		case ResourceParam.WEAPON_MODEL:
			output = GetWeapon ((uint)_params[0]);
			break;

		case ResourceParam.BULLET:
			output = GetBullet ((uint)_params[0]);
			break;

		case ResourceParam.INSTANT_RESOURCE_INPUT:
			output = GetInstantResInput ((string)_params[0]);
			break;

		case ResourceParam.GET_CHARACTER_PATH:
			output = (System.Object)GetCharacterPath ((uint)_params[0]);
			break;

		case ResourceParam.GET_CONTAINER_PATH:
			output = (System.Object)GetContainerPath ((uint)_params[0]);
			break;

		case ResourceParam.GET_WEAPON_DATA_PATH:
			output = (System.Object)GetDataPath (WEAPON_DATA);
			break;

		case ResourceParam.GET_WEAPON_PATH:
			output = (System.Object)GetWeaponPath ((uint)_params[0]);
			break;

		case ResourceParam.GET_BULLET_PATH:
			output = (System.Object)GetBulletPath ((uint)_params[0]);
			break;

		case ResourceParam.GET_INSTANT_RESOURCE_INPUT_PATH:
			output = (System.Object)GetInstantResInputPath ();
			break;
		}

		return output;
	}

	void ReleaseAllResource()
	{
		ReleaseCharacterData ();
		ReleaseContainerData ();
		ReleaseAllGameData ();
		ReleaseWeaponData ();
		ReleaseBulletData ();
	}
}
