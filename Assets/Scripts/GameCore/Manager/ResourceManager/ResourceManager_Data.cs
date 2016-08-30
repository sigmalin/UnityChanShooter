using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class ResourceManager
{
	Dictionary<string, ScriptableObject> mDataTable = null;

	const string WEAPON_DATA = "WeaponDataRepository";

	void InitialGameData()
	{
		mDataTable = new Dictionary<string, ScriptableObject> ();
	}

	void ReleaseGameData(string _type)
	{
		if (mDataTable.ContainsKey (_type) == false)
			return;

		mDataTable.Remove (_type);
	}

	void ReleaseAllGameData()
	{
		string[] keys = mDataTable.Keys.ToArray ();

		for (int Indx = 0; Indx < keys.Length; ++Indx) 
		{
			ReleaseGameData (keys[Indx]);
		}

		mDataTable.Clear ();
	}

	ScriptableObject LoadGameData(string _type)
	{
		ScriptableObject res = (ScriptableObject)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_CACHE, GetDataPath(_type), GetDataAsset(_type), false);
		return res;
	}

	System.Object GetGameData(string _type)
	{
		if (mDataTable.ContainsKey (_type) == true)
			return(System.Object) mDataTable[_type];

		ScriptableObject res = LoadGameData (_type);
		if (res != null)
			mDataTable.Add (_type, res);

		return (System.Object)res;
	}
}
