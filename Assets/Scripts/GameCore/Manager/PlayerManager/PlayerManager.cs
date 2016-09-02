using UnityEngine;
using System.Collections;

public sealed partial class PlayerManager : CommandBehaviour, IParam, IRegister
{
	uint mMainPlayer = 0;

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
			CreateNewPlayer ((uint)_params[0], (Actor)_params[1]);
			TransCommand((uint)_params[0], PlayerInst.CREATE_PLAYER, GetPlayerData ((uint)_params[0]));
			break;

		case PlayerInst.MAIN_PLAYER:
			TransCommand (mMainPlayer, _inst, false);
			mMainPlayer = (uint)_params[0];
			TransCommand (mMainPlayer, _inst, true);
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
		case PlayerParam.MAIN_PLAYER:
			output = (System.Object)mMainPlayer;
			break;

		case PlayerParam.PLAYER_DATA:
			output = (System.Object)GetPlayerData ((uint)_params[0]);
			break;

		case PlayerParam.PLAYER_TRANSFORM:
			output = (System.Object)(GetPlayerData ((uint)_params[0]).RefActor.transform);
			break;

		case PlayerParam.MAIN_PLAYER_DATA:
			output = (System.Object)GetPlayerData (mMainPlayer);
			break;
		}

		return output;
	}

	void TransCommand(uint _ID, uint _inst, params System.Object[] _params)
	{
		PlayerData player = GetPlayerData (_ID);
		if (player != null && player.RefActor != null) 
			player.RefActor.ExecCommand (_inst, _params);
	}

	void BroadcastCommand (uint _inst, params System.Object[] _params)
	{
		PlayerData[] players = GetAllPlayerData ();

		for (int Indx = 0; Indx < players.Length; ++Indx) 
		{
			PlayerData player = players[Indx];
			if (player != null && player.RefActor != null) 
				player.RefActor.ExecCommand (_inst, _params);
		}
	}
}
