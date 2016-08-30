using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class GameCore
{
	Dictionary<uint, IParam> mParamDict = null;

	void InitialParam()
	{
		if (mParamDict == null) 
			mParamDict = new Dictionary<uint, IParam> ();
	}

	System.Object TransParameter(uint _group, uint _inst, params System.Object[] _params)
	{
		if (mParamDict == null)
			return default(System.Object);

		if (mParamDict.ContainsKey (_group) == false)
			return default(System.Object);

		return mParamDict[_group].GetParameter (_inst, _params);
	}

	static public void RegisterParam(uint _group, IParam _param)
	{
		if (_param == null)
			return;

		if (Instance == null)
			return;

		if (Instance.mParamDict == null)
			return;

		if (Instance.mParamDict.ContainsKey (_group) == true)
			Instance.mParamDict.Remove (_group);

		Instance.mParamDict.Add (_group, _param);
	}

	static public void UnRegisterParam(uint _group)
	{
		if (Instance == null)
			return;

		if (Instance.mParamDict == null)
			return;

		if (Instance.mParamDict.ContainsKey (_group) == true)
			Instance.mParamDict.Remove (_group);
	}

	static public System.Object GetParameter(uint _group, uint _inst, params System.Object[] _params)
	{
		if (Instance == null)
			return default(System.Object);

		return Instance.TransParameter (_group, _inst, _params);
	}
}
