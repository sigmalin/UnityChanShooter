using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class ResourceManager
{
	Dictionary<uint, ResourcePool<GameObject>> mCharacterTable = null;

	void InitialCharacterData()
	{
		mCharacterTable = new Dictionary<uint, ResourcePool<GameObject>> ();
	}

	void ReleaseCharacterData()
	{
		uint[] keys = mCharacterTable.Keys.ToArray ();

		for (int Indx = 0; Indx < keys.Length; ++Indx) 
		{
			if (mCharacterTable [keys[Indx]] != null) 
			{
				mCharacterTable [keys[Indx]].Destroy ();
				mCharacterTable [keys[Indx]] = null;
			}
		}

		mCharacterTable.Clear ();
	}

	System.Object GetCharacter(uint _key)
	{
		System.Object res = null;

		if (mCharacterTable == null)
			return res;

		if (mCharacterTable.ContainsKey (_key) == false)
			ProduceCharacterResPool(_key);

		res = (System.Object)mCharacterTable [_key].Produce ();

		return res;
	}

	void RecycleCharacter(uint _key, GameObject _res)
	{
		if (mCharacterTable == null || _res == null)
			return;

		if (mCharacterTable.ContainsKey (_key) == false)
			ProduceCharacterResPool(_key);

		_res.transform.parent = this.transform;

		mCharacterTable [_key].Recycle (_res);
	}

	GameObject LoadCharacter(uint _key)
	{
		GameObject character = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_CHARACTER, _key, CharacterDefine.CHARACTER_KEY_MODEL, true);
		if (character == null)
			return null;

		character.name = string.Format ("Character_{0}",_key.ToString());

		GameCoreResRecycle recycle = character.GetComponent<GameCoreResRecycle> ();
		if (recycle == null)
			recycle = character.AddComponent<GameCoreResRecycle> ();

		recycle.RecycleMethod = () => GameCore.SendCommand (CommandGroup.GROUP_RESOURCE, ResourceInst.RECYCLE_CHARACTER_MODEL, _key, character);
		
		return character;
	}

	void ProduceCharacterResPool(uint _key)
	{
		ResourcePool<GameObject> _pool = new ResourcePool<GameObject> (
			() => {	return LoadCharacter (_key); },
			_ => { GameObject.Destroy (_); },
			(obj, isActive) => { obj.SetActive (isActive); },
			4
		);

		mCharacterTable.Add (_key, _pool);			
	}
}
