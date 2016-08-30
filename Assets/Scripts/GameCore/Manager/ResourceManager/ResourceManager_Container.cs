using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class ResourceManager
{
	Dictionary<uint, ResourcePool<GameObject>> mContainerTable = null;

	void InitialContainerData()
	{
		mContainerTable = new Dictionary<uint, ResourcePool<GameObject>> ();
	}

	void ReleaseContainerData()
	{
		uint[] keys = mContainerTable.Keys.ToArray ();

		for (int Indx = 0; Indx < keys.Length; ++Indx) 
		{
			if (mContainerTable [keys[Indx]] != null) 
			{
				mContainerTable [keys[Indx]].Destroy ();
				mContainerTable [keys[Indx]] = null;
			}
		}

		mContainerTable.Clear ();
	}

	System.Object GetContainer(uint _key)
	{
		System.Object res = null;

		if (mContainerTable == null)
			return res;

		if (mContainerTable.ContainsKey (_key) == false)
			ProduceContainerResPool(_key);

		res = (System.Object)mContainerTable [_key].Produce ();

		return res;
	}

	void RecycleContainer(uint _key, GameObject _res)
	{
		if (mContainerTable == null || _res == null)
			return;

		if (mContainerTable.ContainsKey (_key) == false)
			ProduceContainerResPool(_key);

		_res.transform.parent = this.transform;

		mContainerTable [_key].Recycle (_res);
	}

	GameObject LoadContainer(uint _key)
	{
		GameObject container = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_CACHE, GetContainerPath(_key), GetContainerAsset(_key), true);
		if (container == null)
			return null;

		container.name = string.Format ("Container_{0}",_key.ToString());

		GameCoreResRecycle recycle = container.GetComponent<GameCoreResRecycle> ();
		if (recycle == null)
			recycle = container.AddComponent<GameCoreResRecycle> ();

		recycle.RecycleMethod = () => GameCore.SendCommand (CommandGroup.GROUP_RESOURCE, ResourceInst.RECYCLE_CONTAINER, _key, container);

		return container;
	}

	void ProduceContainerResPool(uint _key)
	{
		ResourcePool<GameObject> _pool = new ResourcePool<GameObject> (
			() => {	return LoadContainer (_key); },
			_ => { GameObject.Destroy (_); },
			(obj, isActive) => { obj.SetActive (isActive); },
			8
		);

		mContainerTable.Add (_key, _pool);			
	}
}
