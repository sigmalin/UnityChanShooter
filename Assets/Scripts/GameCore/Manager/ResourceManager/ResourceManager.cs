using UnityEngine;
using System.Collections;

public sealed partial class ResourceManager : CommandBehaviour, IParam, IRegister
{
	// Use this for initialization
	void Start () 
	{
		InitialCharacterData ();
		InitialContainerData ();
		InitialWeaponData ();
		InitialBulletData ();
		InitialRagdollData ();
		InitialAbilityData ();
	}

	public void OnRegister ()
	{
		GameCore.RegisterCommand (CommandGroup.GROUP_RESOURCE, this);	

		GameCore.RegisterParam (ParamGroup.GROUP_RESOURCE, this);

		InitialRequestQueue ();
	}

	public void OnUnRegister ()
	{
		ReleaseRequestQueue ();

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

		case ResourceInst.RELEASE_RAGDOLL_MODEL:
			ReleaseRagdollData ();
			break;

		case ResourceInst.RELEASE_ABILITY:
			ReleaseAbilityData ();
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

		case ResourceInst.RECYCLE_RAGDOLL_MODEL:
			RecycleRagdoll ((uint)_params[0], (GameObject)_params[1]);
			break;

		case ResourceInst.RECYCLE_ABILITY:
			RecycleAbility ((uint)_params[0], (GameObject)_params[1]);
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
			output = GetWeaponData ();
			break;

		case ResourceParam.WEAPON_MODEL:
			output = GetWeapon ((uint)_params[0]);
			break;

		case ResourceParam.BULLET:
			output = GetBullet ((uint)_params[0]);
			break;

		case ResourceParam.ABILITY:
			output = GetAbility ((uint)_params[0]);
			break;

		case ResourceParam.INSTANT_RESOURCE_UI:
			output = GetInstantResUi ((string)_params[0]);
			break;

		case ResourceParam.INSTANT_RESOURCE_INPUT:
			output = GetInstantResInput ((string)_params[0]);
			break;

		case ResourceParam.RAGDOLL_MODEL:
			output = GetRagdoll ((uint)_params[0]);
			break;
		}

		return output;
	}

	void ReleaseAllResource()
	{
		ReleaseCharacterData ();
		ReleaseContainerData ();
		ReleaseWeaponData ();
		ReleaseBulletData ();
		ReleaseRagdollData ();
		ReleaseAbilityData ();
	}
}
