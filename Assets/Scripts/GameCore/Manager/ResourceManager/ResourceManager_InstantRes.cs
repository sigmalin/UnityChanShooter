using UnityEngine;
using System.Collections;

public partial class ResourceManager
{
	System.Object GetInstantResInput(string _key)
	{
		System.Object res = null;

		if (string.IsNullOrEmpty(_key) == true)
			return res;

		res = (System.Object)LoadInstantRes (_key, GetInstantResInputPath(), GetInstantResInputAsset(_key));

		return res;
	}

	GameObject LoadInstantRes(string _key, string _path, string _asset)
	{
		GameObject output = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_CACHE, _path, _asset, true);
		if (output == null)
			return null;

		output.name = string.Format ("InstantRes_{0}",_key);

		return output;
	}
}
