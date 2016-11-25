using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class ResourceManager
{
	Dictionary<string, ResourcePool<GameObject>> mEffectTable = null;

	void InitialEffectData()
	{
		mEffectTable = new Dictionary<string, ResourcePool<GameObject>> ();
	}

	void ReleaseEffectData()
	{
		string[] keys = mEffectTable.Keys.ToArray ();

		for (int Indx = 0; Indx < keys.Length; ++Indx) 
		{
			if (mEffectTable [keys[Indx]] != null) 
			{
				mEffectTable [keys[Indx]].Destroy ();
				mEffectTable [keys[Indx]] = null;
			}
		}

		mEffectTable.Clear ();
	}

	System.Object GetEffect(string _key)
	{
		System.Object res = null;

		if (mEffectTable == null)
			return res;

		if (mEffectTable.ContainsKey (_key) == false)
			ProduceEffectResPool(_key);

		GameObject go = mEffectTable [_key].Produce ();

		if (go != null) 
		{
			GameCoreResRecycle recycle = go.GetOrAddComponent<GameCoreResRecycle> ();
			recycle.IsRecycled = false;
		}

		res = (System.Object)go;

		return res;
	}

	void RecycleEffect(string _key, GameObject _res)
	{
		if (mEffectTable == null || _res == null)
			return;

		if (mEffectTable.ContainsKey (_key) == false)
			ProduceEffectResPool(_key);

		_res.transform.parent = this.transform;

		mEffectTable [_key].Recycle (_res);
	}

	GameObject LoadEffect(string _key)
	{
		GameObject effect = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_EFFECT, _key, true);
		if (effect == null)
			return null;

		effect.name = string.Format ("Effect_{0}",_key.ToString());

		GameCoreResRecycle recycle = effect.GetOrAddComponent<GameCoreResRecycle> ();

		recycle.RecycleMethod = () => GameCore.SendCommand (CommandGroup.GROUP_RESOURCE, ResourceInst.RECYCLE_EFFECT, _key, effect);

		return effect;
	}

	void ProduceEffectResPool(string _key)
	{
		ResourcePool<GameObject> _pool = new ResourcePool<GameObject> (
			() => {	return LoadEffect (_key); },
			_ => { GameObject.Destroy (_); },
			(obj, isActive) => { obj.SetActive (isActive); },
			16
		);

		mEffectTable.Add (_key, _pool);			
	}
}
