using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class ResourceManager
{
	Dictionary<uint, ResourcePool<GameObject>> mAbilityTable = null;

	void InitialAbilityData()
	{
		mAbilityTable = new Dictionary<uint, ResourcePool<GameObject>> ();
	}

	void ReleaseAbilityData()
	{
		uint[] keys = mAbilityTable.Keys.ToArray ();

		for (int Indx = 0; Indx < keys.Length; ++Indx) 
		{
			if (mAbilityTable [keys[Indx]] != null) 
			{
				mAbilityTable [keys[Indx]].Destroy ();
				mAbilityTable [keys[Indx]] = null;
			}
		}

		mAbilityTable.Clear ();
	}

	System.Object GetAbility(uint _key)
	{
		System.Object res = null;

		if (mAbilityTable == null)
			return res;

		if (mAbilityTable.ContainsKey (_key) == false)
			ProduceAbilityResPool(_key);

		GameObject go = mAbilityTable [_key].Produce ();

		if (go != null) 
		{
			GameCoreResRecycle recycle = go.GetOrAddComponent<GameCoreResRecycle> ();
			recycle.IsRecycled = false;
		}

		res = (System.Object)go;

		return res;
	}

	void RecycleAbility(uint _key, GameObject _res)
	{
		if (mAbilityTable == null || _res == null)
			return;

		if (mAbilityTable.ContainsKey (_key) == false)
			ProduceAbilityResPool(_key);

		_res.transform.parent = this.transform;

		mAbilityTable [_key].Recycle (_res);
	}

	GameObject LoadAbility(uint _key)
	{
		GameObject ability = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_ABILITY, _key, true);
		if (ability == null)
			return null;

		ability.name = string.Format ("Ability_{0}",_key.ToString());

		GameCoreResRecycle recycle = ability.GetOrAddComponent<GameCoreResRecycle> ();

		recycle.RecycleMethod = () => GameCore.SendCommand (CommandGroup.GROUP_RESOURCE, ResourceInst.RECYCLE_ABILITY, _key, ability);

		return ability;
	}

	void ProduceAbilityResPool(uint _key)
	{
		ResourcePool<GameObject> _pool = new ResourcePool<GameObject> (
			() => {	return LoadAbility (_key); },
			_ => { GameObject.Destroy (_); },
			(obj, isActive) => { obj.SetActive (isActive); },
			16
		);

		mAbilityTable.Add (_key, _pool);			
	}
}
