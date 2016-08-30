using UnityEngine;
using System.Collections;

public sealed partial class CacheManager : CommandBehaviour, IParam, IRegister
{
	// Use this for initialization
	void Start () 
	{
		InitialRequestQueue ();

		InitialCacheData ();

		InitialLoadRequest ();
	}

	public void OnRegister ()
	{
		GameCore.RegisterCommand (CommandGroup.GROUP_CACHE, this);	

		GameCore.RegisterParam (ParamGroup.GROUP_CACHE, this);
	}

	public void OnUnRegister ()
	{
		ReleaseAllCacheData ();

		GameCore.UnRegisterCommand (CommandGroup.GROUP_CACHE);

		GameCore.UnRegisterParam (ParamGroup.GROUP_CACHE);
	}

	//public void ExecCommand (uint _inst, params System.Object[] _params)
	protected override void BatchCommand (uint _inst, params System.Object[] _params)
	{
		switch(_inst)
		{
		case CacheInst.VERSION_VERIFY:
			LoadVersionList ();
			break;

		case CacheInst.RELEASE_ALL_CACHE:
			ReleaseAllCacheData ();
			break;

		case CacheInst.LOAD_CACHE:
			AddLoad ((string)_params[0]);
			break;

		case CacheInst.RELEASE_CACHE:
			ReleaseCache ((string)_params[0]);
			break;
		}

	}

	public System.Object GetParameter (uint _inst, params System.Object[] _params)
	{
		System.Object output = default(System.Object);

		switch (_inst) 
		{
		case CacheParam.GET_CACHE:
			output = GetCache ((string)_params[0], (string)_params[1], (bool)_params[2]);
			break;
		}

		return output;
	}
}
