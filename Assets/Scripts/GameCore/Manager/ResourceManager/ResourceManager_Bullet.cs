using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class ResourceManager
{
	Dictionary<uint, ResourcePool<GameObject>> mBulletTable = null;

	void InitialBulletData()
	{
		mBulletTable = new Dictionary<uint, ResourcePool<GameObject>> ();
	}

	void ReleaseBulletData()
	{
		uint[] keys = mBulletTable.Keys.ToArray ();

		for (int Indx = 0; Indx < keys.Length; ++Indx) 
		{
			if (mBulletTable [keys[Indx]] != null) 
			{
				mBulletTable [keys[Indx]].Destroy ();
				mBulletTable [keys[Indx]] = null;
			}
		}

		mBulletTable.Clear ();
	}

	System.Object GetBullet(uint _key)
	{
		System.Object res = null;

		if (mBulletTable == null)
			return res;

		if (mBulletTable.ContainsKey (_key) == false)
			ProduceBulletResPool(_key);

		res = (System.Object)mBulletTable [_key].Produce ();

		return res;
	}

	void RecycleBullet(uint _key, GameObject _res)
	{
		if (mBulletTable == null || _res == null)
			return;

		if (mBulletTable.ContainsKey (_key) == false)
			ProduceBulletResPool(_key);

		_res.transform.parent = this.transform;

		mBulletTable [_key].Recycle (_res);
	}

	GameObject LoadBullet(uint _key)
	{
		GameObject bullet = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_CACHE, GetBulletPath(_key), GetBulletAsset(_key), true);
		if (bullet == null)
			return null;

		bullet.name = string.Format ("Bullet_{0}",_key.ToString());

		GameCoreResRecycle recycle = bullet.GetComponent<GameCoreResRecycle> ();
		if (recycle == null)
			recycle = bullet.AddComponent<GameCoreResRecycle> ();

		recycle.RecycleMethod = () => GameCore.SendCommand (CommandGroup.GROUP_RESOURCE, ResourceInst.RECYCLE_BULLET, _key, bullet);

		return bullet;
	}

	void ProduceBulletResPool(uint _key)
	{
		ResourcePool<GameObject> _pool = new ResourcePool<GameObject> (
			() => {	return LoadBullet (_key); },
			_ => { GameObject.Destroy (_); },
			(obj, isActive) => { obj.SetActive (isActive); },
			16
		);

		mBulletTable.Add (_key, _pool);			
	}
}
