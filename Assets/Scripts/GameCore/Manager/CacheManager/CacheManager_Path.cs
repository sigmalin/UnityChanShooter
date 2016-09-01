using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class CacheManager
{
	public struct CacheCatalogue
	{
		public string PathIndex;

		public string AssetIndex;

		public CacheCatalogue(string _path, string _asset)
		{
			PathIndex = _path;

			AssetIndex = _asset;
		}
	}

	Dictionary<uint, CacheCatalogue> mCatalogueTable = null;

	void InitialCatalogueData()
	{
		ReleaseCatalogueData ();

		mCatalogueTable = new Dictionary<uint, CacheCatalogue> ();
	}

	void ReleaseCatalogueData()
	{
		if (mCatalogueTable != null) 
		{
			mCatalogueTable.Clear ();

			mCatalogueTable = null;
		}
	}

	void AddCacheCatalogue(uint _type, string _path, string _asset)
	{
		if (mCatalogueTable == null)
			return;

		if (mCatalogueTable.ContainsKey (_type) == true)
			mCatalogueTable.Remove (_type);

		mCatalogueTable.Add (_type, new CacheCatalogue (_path, _asset));
	}

	string GetCachePath(uint _type, string _key)
	{
		if (mCatalogueTable == null)
			return string.Empty;

		if (mCatalogueTable.ContainsKey (_type) == false)
			return string.Empty;

		return string.Format (mCatalogueTable[_type].PathIndex, _key);
	}

	string GetCacheAsset(uint _type, string _key)
	{
		if (mCatalogueTable == null)
			return string.Empty;

		if (mCatalogueTable.ContainsKey (_type) == false)
			return string.Empty;

		return string.Format (mCatalogueTable[_type].AssetIndex, _key);
	}
}
