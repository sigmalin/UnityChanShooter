using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public sealed partial class AiManager 
{
	Dictionary<uint, IAi> mAiTable = null;

	void InitialAiTable()
	{
		mAiTable = new Dictionary<uint, IAi> ();
	}

	void RegisterAi(uint _playerID, int _aiID)
	{
		RemoveAi (_playerID);

		IAi ai = GenerateAI (_aiID);
		if (ai == null)
			return;

		ai.Initial (_playerID);

		mAiTable.Add (_playerID, ai);
	}

	void RemoveAi(uint _playerID)
	{
		if (mAiTable.ContainsKey (_playerID) == false)
			return;

		mAiTable [_playerID].Release ();

		mAiTable.Remove (_playerID);
	}

	IAi GetAiData(uint _playerID)
	{
		if (mAiTable.ContainsKey (_playerID) == false)
			return null;

		return mAiTable[_playerID];
	}

	uint[] GetAllPlayerID()
	{
		return mAiTable.Keys.ToArray ();
	}

	void ClearAiData()
	{
		GetAllPlayerID ().ToObservable ()
			.Subscribe (_ => RemoveAi (_));
		
		mAiTable.Clear ();
	}
}
