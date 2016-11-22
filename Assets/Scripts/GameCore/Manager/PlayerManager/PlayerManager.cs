using UnityEngine;
using System.Collections;
using UniRx;

public sealed partial class PlayerManager : CommandBehaviour, IParam, IRegister
{
	// Use this for initialization
	void Start () 
	{		
		InitialPlayerData ();
	}

	public void OnRegister ()
	{
		GameCore.RegisterCommand (CommandGroup.GROUP_PLAYER, this);	

		GameCore.RegisterParam (ParamGroup.GROUP_PLAYER, this);

		InitialRequestQueue ();

		InitialActorUpdate ();
	}

	public void OnUnRegister ()
	{
		ReleaseActorUpdate ();

		ReleaseRequestQueue ();

		ClearPlayerData ();

		GameCore.UnRegisterCommand (CommandGroup.GROUP_PLAYER);

		GameCore.UnRegisterParam (ParamGroup.GROUP_PLAYER);
	}

	//public void ExecCommand (uint _inst, params System.Object[] _params)
	protected override void BatchCommand (uint _inst, params System.Object[] _params)
	{
		switch (_inst) 
		{
		case PlayerInst.CREATE_PLAYER:			
			CreateNewPlayer ((uint)_params[0], (uint)_params[1]);
			TransCommand((uint)_params[0], PlayerInst.CREATE_PLAYER, (uint)_params[0]);
			break;

		case PlayerInst.REMOVE_PLAYER:
			RemovePlayer ((uint)_params[0]);
			break;

		default:
			System.Object[] objects = new System.Object[_params.Length - 1];
			System.Array.Copy (_params, 1, objects, 0, _params.Length - 1);
			TransCommand ((uint)_params[0], _inst, objects);
			break;
		}
	}

	public System.Object GetParameter (uint _inst, params System.Object[] _params)
	{
		System.Object output = default(System.Object);

		switch (_inst) 
		{
		case PlayerParam.PLAYER_DATA:
			output = (System.Object)GetPlayerData ((uint)_params[0]);
			break;

		case PlayerParam.PLAYER_TRANSFORM:
			PlayerActor actor = GetPlayerData ((uint)_params [0]);
			output = actor == null ? null : (System.Object)(actor.transform);
			break;

		case PlayerParam.PLAYER_LOCK_TARGET:
			output = (System.Object)GetLockActorID ((uint)_params [0]);
			break;

		case PlayerParam.PLAYER_DISTANCE_BETWEEN_LOCK_ACTOR:
			output = (System.Object)CalcDistanceBetweenLockActor ((uint)_params [0]);
			break;
		}

		return output;
	}

	void TransCommand(uint _ID, uint _inst, params System.Object[] _params)
	{
		PlayerActor player = GetPlayerData (_ID);
		if (player != null) 
			player.ExecCommand (_inst, _params);
	}

	void BroadcastCommand (uint _inst, params System.Object[] _params)
	{
		uint[] allIDs = GetAllPlayerID ();

		for (int Indx = 0; Indx < allIDs.Length; ++Indx)
			TransCommand (allIDs [Indx], _inst, _params);
	}
}
