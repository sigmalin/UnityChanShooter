using UnityEngine;
using System.Collections;

public partial class ResourceManager
{
	System.Object GetInstantResInput(string _key)
	{
		System.Object res = null;

		if (string.IsNullOrEmpty(_key) == true)
			return res;

		res = (System.Object)LoadInstantRes (_key);

		return res;
	}

	GameObject LoadInstantRes(string _key)
	{
		GameObject output = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_INSTANT_RESOURCE_INPUT, _key, true);
		if (output == null)
			return null;

		output.name = string.Format ("InstantRes_{0}",_key);

		return output;
	}
}
