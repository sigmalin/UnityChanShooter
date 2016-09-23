using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class CacheManager
{
	Dictionary<string, AssetBundle> mCacheTable = null;

	void InitialCacheData()
	{
		mCacheTable = new Dictionary<string, AssetBundle> ();
	}

	void ReleaseAllCacheData()
	{
		string[] keys = mCacheTable.Keys.ToArray ();

		for (int Indx = 0; Indx < keys.Length; ++Indx) 
		{
			if (mCacheTable [keys[Indx]] != null) 
			{
				mCacheTable [keys[Indx]].Unload (true);
				mCacheTable [keys[Indx]] = null;
			}
		}

		mCacheTable.Clear ();
	}

	void ReleaseCache(string _key)
	{
		if (mCacheTable.ContainsKey (_key) == true) 
		{
			mCacheTable [_key].Unload (true);
			mCacheTable [_key] = null;

			mCacheTable.Remove (_key);
		}
	}

	void AddCache(string _key, AssetBundle _cache)
	{
		if (mCacheTable.ContainsKey (_key) == true) 
		{
			mCacheTable [_key].Unload (true);
			mCacheTable.Remove (_key);
		}

		mCacheTable.Add (_key, _cache);
	}

	System.Object GetCache(string _key, string _assetPath, bool _instantiate)
	{
		if (mCacheTable.ContainsKey (_key) == false)
			return null;

		if (mCacheTable [_key] == null)
			return null;

		System.Object asset = mCacheTable [_key].LoadAsset (ASSET_PATH + _assetPath);
		if (asset == null)
			return null;

		return _instantiate ? UnityEngine.Object.Instantiate ((UnityEngine.Object)asset) : asset;
	}

	bool IsCacheLoad(string _key)
	{
		if (mCacheTable == null)
			return false;

		return mCacheTable.ContainsKey (_key);
	}
}
