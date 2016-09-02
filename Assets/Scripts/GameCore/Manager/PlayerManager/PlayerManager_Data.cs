using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerManager
{
	public class PlayerData
	{
		public uint ID;
		public Vector3 Move;
		public float Speed;
		public Vector3 LookAt;

		public Actor RefActor;

		public void Clear()
		{
			RefActor = null;
		}
	}

	Dictionary<uint, PlayerData> mPlayerTable = null;

	void InitialPlayerData()
	{
		mPlayerTable = new Dictionary<uint, PlayerData> ();
	}

	void CreateNewPlayer(uint _playerID, Actor _actor)
	{
		if (mPlayerTable.ContainsKey (_playerID) == true) 
		{
			mPlayerTable [_playerID].Clear ();
			mPlayerTable.Remove (_playerID);
		}

		PlayerData data = new PlayerData ();

		data.ID = _playerID;
		data.RefActor = _actor;

		mPlayerTable.Add (_playerID, data);
	}

	PlayerData GetPlayerData(uint _playerID)
	{
		if (mPlayerTable.ContainsKey (_playerID) == false)
			return null;

		return mPlayerTable[_playerID];
	}

	uint[] GetAllPlayerID()
	{
		return mPlayerTable.Keys.ToArray ();
	}

	PlayerData[] GetAllPlayerData()
	{
		return mPlayerTable.Values.ToArray ();
	}

	void ClearPlayerData()
	{
		foreach (PlayerData data in mPlayerTable.Values) 
		{
			if (data != null)
				data.Clear ();
		}

		mPlayerTable.Clear ();
	}
}
