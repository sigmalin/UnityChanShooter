using UnityEngine;
using System.Collections;

public sealed partial class CacheManager : CommandBehaviour, IParam, IRegister
{
	// Use this for initialization
	void Start () 
	{
		InitialCacheData ();
	}

	public void OnRegister ()
	{
		GameCore.RegisterCommand (CommandGroup.GROUP_CACHE, this);	

		GameCore.RegisterParam (ParamGroup.GROUP_CACHE, this);

		InitialRequestQueue ();

		InitialVersionTable ();

		InitialCatalogueData ();

		InitialReadRequest ();

		InitialDownLoadRequest ();
	}

	public void OnUnRegister ()
	{
		ReleaseDownLoadRequest ();

		ReleaselReadRequest ();

		ReleaseCatalogueData ();

		ReleaseVersionTable ();

		ReleaseRequestQueue ();

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

		case CacheInst.READ_CACHE:
			AddReadLoad ((string)_params[0]);
			break;

		case CacheInst.RELEASE_CACHE:
			ReleaseCache ((string)_params[0]);
			break;

		case CacheInst.REPORT_READ_STATE:
			ReportReadState (true);
			break;

		case CacheInst.REPORT_DOWN_LOAD_STATE:
			ReportDownLoadState (true);
			break;

		case CacheInst.DOWN_LOAD_CACHE:
			AddDownLoad ((string)_params[0]);
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

		case CacheParam.GET_SCENE_PATH:
			output = (System.Object)GetCachePath (_inst, (string)_params [0]);
			break;

		case CacheParam.GET_CHARACTER_PATH:
			output = (System.Object)GetCachePath (_inst, ((uint)_params [0]).ToString());
			break;

		case CacheParam.GET_CONTAINER_PATH:
			output = (System.Object)GetCachePath (_inst, ((uint)_params [0]).ToString());
			break;

		case CacheParam.GET_WEAPON_DATA_PATH:
			output = (System.Object)GetCachePath (_inst, string.Empty);
			break;

		case CacheParam.GET_WEAPON_PATH:
			output = (System.Object)GetCachePath (_inst, ((uint)_params [0]).ToString());
			break;

		case CacheParam.GET_BULLET_PATH:
			output = (System.Object)GetCachePath (_inst, ((uint)_params [0]).ToString());
			break;

		case CacheParam.GET_INSTANT_RESOURCE_INPUT_PATH:
			output = (System.Object)GetCachePath (_inst, string.Empty);
			break;

		case CacheParam.GET_CHARACTER:
			{
				uint characterKey = (uint)_params [0];

				output = GetCache (
					GetCachePath (CacheParam.GET_CHARACTER_PATH, characterKey.ToString()), 
					GetCacheAsset (CacheParam.GET_CHARACTER_PATH, characterKey.ToString()), 
					(bool)_params [1]);
			}
			break;

		case CacheParam.GET_CONTAINER:
			{
				uint containerKey = (uint)_params [0];

				output = GetCache (
					GetCachePath (CacheParam.GET_CONTAINER_PATH, containerKey.ToString()), 
					GetCacheAsset (CacheParam.GET_CONTAINER_PATH, containerKey.ToString()), 
					(bool)_params [1]);
			}
			break;

		case CacheParam.GET_WEAPON_DATA:

			output = GetCache (
				GetCachePath (CacheParam.GET_WEAPON_DATA_PATH, string.Empty), 
				GetCacheAsset (CacheParam.GET_WEAPON_DATA_PATH, string.Empty), 
				(bool)_params [0]);
			break;

		case CacheParam.GET_WEAPON:
			{
				uint weaponKey = (uint)_params [0];

				output = GetCache (
					GetCachePath (CacheParam.GET_WEAPON_PATH, weaponKey.ToString()), 
					GetCacheAsset (CacheParam.GET_WEAPON_PATH, weaponKey.ToString()), 
					(bool)_params [1]);
			}
			break;

		case CacheParam.GET_BULLET:
			{
				uint bulletKey = (uint)_params [0];

				output = GetCache (
					GetCachePath (CacheParam.GET_BULLET_PATH, bulletKey.ToString()), 
					GetCacheAsset (CacheParam.GET_BULLET_PATH, bulletKey.ToString()), 
					(bool)_params [1]);
			}
			break;

		case CacheParam.GET_INSTANT_RESOURCE_INPUT:
			{
				string inputKey = (string)_params [0];

				output = GetCache (
					GetCachePath (CacheParam.GET_INSTANT_RESOURCE_INPUT_PATH, inputKey.ToString()), 
					GetCacheAsset (CacheParam.GET_INSTANT_RESOURCE_INPUT_PATH, inputKey.ToString()), 
					(bool)_params [1]);
			}
			break;
		}

		return output;
	}
}
