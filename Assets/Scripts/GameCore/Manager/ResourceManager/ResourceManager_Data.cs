using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class ResourceManager
{
	System.Object GetWeaponData()
	{
		ScriptableObject res = (ScriptableObject)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_WEAPON_DATA, false);

		return (System.Object)res;
	}
}
