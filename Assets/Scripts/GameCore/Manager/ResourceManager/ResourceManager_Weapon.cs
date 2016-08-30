using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class ResourceManager
{
	Dictionary<uint, ResourcePool<GameObject>> mWeaponTable = null;

	void InitialWeaponData()
	{
		mWeaponTable = new Dictionary<uint, ResourcePool<GameObject>> ();
	}

	void ReleaseWeaponData()
	{
		uint[] keys = mWeaponTable.Keys.ToArray ();

		for (int Indx = 0; Indx < keys.Length; ++Indx) 
		{
			if (mWeaponTable [keys[Indx]] != null) 
			{
				mWeaponTable [keys[Indx]].Destroy ();
				mWeaponTable [keys[Indx]] = null;
			}
		}

		mWeaponTable.Clear ();
	}

	System.Object GetWeapon(uint _key)
	{
		System.Object res = null;

		if (mWeaponTable == null)
			return res;

		if (mWeaponTable.ContainsKey (_key) == false)
			ProduceWeaponResPool(_key);

		res = (System.Object)mWeaponTable [_key].Produce ();

		return res;
	}

	void RecycleWeapon(uint _key, GameObject _res)
	{
		if (mWeaponTable == null || _res == null)
			return;

		if (mWeaponTable.ContainsKey (_key) == false)
			ProduceWeaponResPool(_key);

		_res.transform.parent = this.transform;

		mWeaponTable [_key].Recycle (_res);
	}

	GameObject LoadWeapon(uint _key)
	{
		GameObject weapon = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_CACHE, GetWeaponPath(_key), GetWeaponAsset(_key), true);
		if (weapon == null) 
			return null;

		weapon.name = string.Format ("Weapon_{0}",_key.ToString());

		GameCoreResRecycle recycle = weapon.GetComponent<GameCoreResRecycle> ();
		if (recycle == null)
			recycle = weapon.AddComponent<GameCoreResRecycle> ();

		recycle.RecycleMethod = () => GameCore.SendCommand (CommandGroup.GROUP_RESOURCE, ResourceInst.RECYCLE_WEAPON_MODEL, _key, weapon);

		return weapon;
	}

	void ProduceWeaponResPool(uint _key)
	{
		ResourcePool<GameObject> _pool = new ResourcePool<GameObject> (
			() => {	return LoadWeapon (_key); },
			_ => { GameObject.Destroy (_); },
			(obj, isActive) => { obj.SetActive (isActive); },
			4
		);

		mWeaponTable.Add (_key, _pool);			
	}
}
