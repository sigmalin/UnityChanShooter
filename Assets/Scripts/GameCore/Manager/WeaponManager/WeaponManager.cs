using UnityEngine;
using System.Collections;

public sealed partial class WeaponManager : CommandBehaviour, IParam, IRegister
{
	// Use this for initialization
	void Start () 
	{
		InitialWeaponActor ();
	}
	
	public void OnRegister ()
	{
		GameCore.RegisterCommand (CommandGroup.GROUP_WEAPON, this);	

		GameCore.RegisterParam (ParamGroup.GROUP_WEAPON, this);

		InitialRequestQueue ();

		InitialActorObservable ();

		InitialFireRequest ();

		InitialWeaponLauncher ();
	}

	public void OnUnRegister ()
	{
		ReleaseWeaponLauncher ();

		ReleaseFireRequest ();

		ReleaseActorObservable ();

		ReleaseRequestQueue ();

		RemoveAllActor ();

		GameCore.UnRegisterCommand (CommandGroup.GROUP_WEAPON);

		GameCore.UnRegisterParam (ParamGroup.GROUP_WEAPON);
	}

	//public void ExecCommand (uint _inst, params System.Object[] _params)
	protected override void BatchCommand (uint _inst, params System.Object[] _params)
	{
		switch (_inst) 
		{
		case WeaponInst.REGISTER_ACTOR:
			RegisterActor ((uint)_params [0], (uint)_params [1]);
			break;

		case WeaponInst.REMOVE_ALL_ACTOR:
			RemoveAllActor ();
			break;

		case WeaponInst.REMOVE_ACTOR:
			RemoveActor ((uint)_params [0]);
			break;

		case WeaponInst.MAIN_ACTOR:
			SetMainActor ((uint)_params [0]);
			break;

		case WeaponInst.SET_TEAM:
			SetActorTeam ((uint)_params [0], (int)_params [1]);
			break;

		case WeaponInst.SET_INVINCIBLE:
			SetActorFlag ((uint)_params [0], Flags.INVINCIBLE, (bool)_params [1]);
			break;

		case WeaponInst.ARM_FIRE:
			AddFireRequest ((uint)_params [0], (IArm)_params [1]);
			break;

		case WeaponInst.ADD_FIRE_DAMAGE:
			AddFireDamage ((uint)_params [0], (uint)_params [1], (uint)_params [2], (Vector3)_params [3]);
			break;

		case WeaponInst.HEAL:
			HealActor ((uint)_params [0], (uint)_params [1]);
			break;

		case WeaponInst.USE_ABILITY:
			UseAbility ((uint)_params [0], (uint)_params [1]);
			break;

		case WeaponInst.PUSH_MAIN_WEAPON_INTERFACE:
			PushWeaponInterface ();
			break;

		case WeaponInst.POP_MAIN_WEAPON_INTERFACE:
			PopWeaponInterface ();
			break;
		}
	}

	public System.Object GetParameter (uint _inst, params System.Object[] _params)
	{
		System.Object output = default(System.Object);

		switch (_inst) 
		{
		case WeaponParam.WEAPON_ACTOR_DATA:
			output = (System.Object)GetWeaponActor ((uint)_params [0]);
			break;

		case WeaponParam.WEAPON_ACTOR_SPEED:
			output = (System.Object)GetActorSpeed ((uint)_params [0]);
			break;

		case WeaponParam.WEAPON_ACTOR_RANGE:
			output = (System.Object)GetActorRange ((uint)_params [0]);
			break;

		case WeaponParam.WEAPON_ACTOR_FREQ:
			output = (System.Object)GetActorFreq ((uint)_params [0]);
			break;

		case WeaponParam.GET_LASTEST_MURDERER:
			output = (System.Object)GetLastestMurdererID ((uint)_params [0]);
			break;

		case WeaponParam.GET_MAIN_ACTOR_ID:
			output = (System.Object)MainActorID;
			break;

		case WeaponParam.GET_ALLY_LIST:
			output = (System.Object)GetAllyList ((uint)_params [0]);
			break;

		case WeaponParam.GET_HOSTILITY_LIST:
			output = (System.Object)GetHostilityList ((uint)_params [0]);
			break;
		}

		return output;
	}
}
