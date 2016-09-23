﻿using UnityEngine;
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

		BroadcastCommand (PlayerInst.REMOVE_PLAYER);

		ClearPlayerData ();

		GameCore.UnRegisterCommand (CommandGroup.GROUP_PLAYER);

		GameCore.UnRegisterParam (ParamGroup.GROUP_PLAYER);
	}

	//public void ExecCommand (uint _inst, params System.Object[] _params)
	protected override void BatchCommand (uint _inst, params System.Object[] _params)
	{
		switch (_inst) 
		{
		case PlayerInst.GAME_START:
			BroadcastCommand (_inst, _params);
			break;

		case PlayerInst.CREATE_PLAYER:			
			CreateNewPlayer ((uint)_params[0], (uint)_params[1]);
			TransCommand((uint)_params[0], PlayerInst.CREATE_PLAYER, (uint)_params[0]);
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
			output = (System.Object)(GetPlayerData ((uint)_params[0]).transform);
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
		GetAllPlayerData().ToObservable ()
			.Where (_ => _ != null)
			.Subscribe (_ => _.ExecCommand (_inst, _params));
	}
}
