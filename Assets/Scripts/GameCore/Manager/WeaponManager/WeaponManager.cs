using UnityEngine;
using System.Collections;

public sealed partial class WeaponManager : CommandBehaviour, IParam, IRegister
{
	// Use this for initialization
	void Start () 
	{
		InitialWeaponData ();	
		InitialWeaponActor ();
	}
	
	public void OnRegister ()
	{
		GameCore.RegisterCommand (CommandGroup.GROUP_WEAPON, this);	

		GameCore.RegisterParam (ParamGroup.GROUP_WEAPON, this);

		InitialRequestQueue ();

		InitialActorObservable ();

		InitialFireRequest ();
	}

	public void OnUnRegister ()
	{
		ReleaseFireRequest ();

		ReleaseActorObservable ();

		ReleaseRequestQueue ();

		RemoveAllActor ();

		LoadWeaponData (null);

		GameCore.UnRegisterCommand (CommandGroup.GROUP_WEAPON);

		GameCore.UnRegisterParam (ParamGroup.GROUP_WEAPON);
	}

	//public void ExecCommand (uint _inst, params System.Object[] _params)
	protected override void BatchCommand (uint _inst, params System.Object[] _params)
	{
		switch (_inst) 
		{
		case WeaponInst.LOAD_DATA:
			LoadWeaponData ((WeaponDataRepository)_params [0]);
			break;

		case WeaponInst.RELEASE_DATA:
			LoadWeaponData (null);
			break;

		case WeaponInst.REGISTER_ACTOR:
			RegisterActor ((uint)_params [0], (uint)_params [1]);
			break;

		case WeaponInst.REMOVE_ALL_ACTOR:
			RemoveAllActor ();
			break;

		case WeaponInst.REMOVE_ACTOR:
			RemoveActor ((uint)_params [0]);
			break;

		case WeaponInst.ARM_FIRE:
			AddFireRequest ((uint)_params [0], (IArm)_params [1]);
			break;
		}
	}

	public System.Object GetParameter (uint _inst, params System.Object[] _params)
	{
		System.Object output = default(System.Object);

		switch(_inst)
		{
		case WeaponParam.MODEL_ID:
			WeaponData data = GetWeaponData ((uint)_params [0]);
			output = (System.Object)data.ModelID;
			break;
		}

		return output;
	}
}
