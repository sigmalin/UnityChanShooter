using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class ResourceManager
{
	Dictionary<uint, ResourcePool<GameObject>> mRagdollTable = null;

	void InitialRagdollData()
	{
		mRagdollTable = new Dictionary<uint, ResourcePool<GameObject>> ();
	}

	void ReleaseRagdollData()
	{
		uint[] keys = mRagdollTable.Keys.ToArray ();

		for (int Indx = 0; Indx < keys.Length; ++Indx) 
		{
			if (mRagdollTable [keys[Indx]] != null) 
			{
				mRagdollTable [keys[Indx]].Destroy ();
				mRagdollTable [keys[Indx]] = null;
			}
		}

		mRagdollTable.Clear ();
	}

	System.Object GetRagdoll(uint _key)
	{
		System.Object res = null;

		if (mRagdollTable == null)
			return res;

		if (mRagdollTable.ContainsKey (_key) == false)
			ProduceRagdollResPool(_key);

		GameObject go = mRagdollTable [_key].Produce ();

		if (go != null) 
		{
			GameCoreResRecycle recycle = go.GetOrAddComponent<GameCoreResRecycle> ();
			recycle.IsRecycled = false;
		}

		res = (System.Object)go;

		return res;
	}

	void RecycleRagdoll(uint _key, GameObject _res)
	{
		if (mRagdollTable == null || _res == null)
			return;

		if (mRagdollTable.ContainsKey (_key) == false)
			ProduceRagdollResPool(_key);

		_res.transform.parent = this.transform;

		mRagdollTable [_key].Recycle (_res);
	}

	GameObject LoadRagdoll(uint _key)
	{
		GameObject character = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_CHARACTER, _key, CharacterDefine.CHARACTER_KEY_RAGDOLL, true);
		if (character == null)
			return null;

		character.name = string.Format ("Ragdoll_{0}",_key.ToString());

		GameCoreResRecycle recycle = character.GetOrAddComponent<GameCoreResRecycle> ();

		recycle.RecycleMethod = () => GameCore.SendCommand (CommandGroup.GROUP_RESOURCE, ResourceInst.RECYCLE_RAGDOLL_MODEL, _key, character);

		return character;
	}

	void ProduceRagdollResPool(uint _key)
	{
		ResourcePool<GameObject> _pool = new ResourcePool<GameObject> (
			() => {	return LoadRagdoll (_key); },
			_ => { GameObject.Destroy (_); },
			(obj, isActive) => { obj.SetActive (isActive); },
			4
		);

		mRagdollTable.Add (_key, _pool);			
	}
}
