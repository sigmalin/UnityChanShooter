using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public partial class PlayerManager
{
	Dictionary<uint, PlayerActor> mPlayerTable = null;

	void InitialPlayerData()
	{
		mPlayerTable = new Dictionary<uint, PlayerActor> ();
	}

	void CreateNewPlayer(uint _playerID, uint _containerID)
	{
		RemovePlayer (_playerID);

		GameObject container = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.CONTAINER, _containerID);
		PlayerActor actor = container.GetOrAddComponent<PlayerActor> ();

		mPlayerTable.Add (_playerID, actor);
	}

	void RemovePlayer(uint _playerID)
	{
		if (mPlayerTable.ContainsKey (_playerID) == false)
			return;

		mPlayerTable [_playerID].ExecCommand (PlayerInst.REMOVE_PLAYER);
		mPlayerTable.Remove (_playerID);
	}

	PlayerActor GetPlayerData(uint _playerID)
	{
		if (mPlayerTable.ContainsKey (_playerID) == false)
			return null;

		return mPlayerTable[_playerID];
	}

	uint[] GetAllPlayerID()
	{
		return mPlayerTable.Keys.ToArray ();
	}

	PlayerActor[] GetAllPlayerData()
	{
		return mPlayerTable.Values.ToArray ();
	}

	void ClearPlayerData()
	{
		BroadcastCommand (PlayerInst.REMOVE_PLAYER);

		mPlayerTable.Clear ();
	}
}
