using UnityEngine;
using System.Collections;

public sealed partial class CacheManager : CommandBehaviour, IParam, IRegister
{
	public struct CacheRequest
	{
		public string Path;

		public CacheRequest(string _Path)
		{
			Path = _Path;
		}
	}

	public class CacheState
	{
		public bool NeedReport { get; set; }
		public bool HasFailure { get; set; }
		public System.IDisposable Disposable { get; set; }
	}

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

		case CacheInst.REPORT_LOAD_STATE:
			ReportDownLoadState ();
			break;

		case CacheInst.REPORT_READ_STATE:
			ReportReadState ();
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
			output = (System.Object)GetCachePath (_inst, _params);
			break;

		case CacheParam.GET_CHARACTER_PATH:
			output = (System.Object)GetCachePath (_inst, _params);
			break;

		case CacheParam.GET_CONTAINER_PATH:
			output = (System.Object)GetCachePath (_inst, _params);
			break;

		case CacheParam.GET_WEAPON_DATA_PATH:
			output = (System.Object)GetCachePath (_inst);
			break;

		case CacheParam.GET_WEAPON_PATH:
			if ((uint)_params [0] == 0u) output = (System.Object)(string.Empty);
			else                         output = (System.Object)GetCachePath (_inst, _params);
			break;

		case CacheParam.GET_BULLET_PATH:
			if ((uint)_params [0] == 0u) output = (System.Object)(string.Empty);
			else                         output = (System.Object)GetCachePath (_inst, _params);
			break;

		case CacheParam.GET_PORTRAIT_PATH:
			output = (System.Object)GetCachePath (_inst, _params);
			break;

		case CacheParam.GET_LOCALIZATION_PATH:
			output = (System.Object)GetCachePath (_inst, _params);
			break;

		case CacheParam.GET_CHARACTER_DATA_PATH:
			output = (System.Object)GetCachePath (_inst, _params);
			break;

		case CacheParam.GET_CHAPTER_DATA_PATH:
			output = (System.Object)GetCachePath (_inst, _params);
			break;

		case CacheParam.GET_CHAPTER_IMAGE_PATH:
			output = (System.Object)GetCachePath (_inst, _params);
			break;

		case CacheParam.GET_STAGE_IMAGE_PATH:
			output = (System.Object)GetCachePath (_inst, _params);
			break;

		case CacheParam.GET_INSTANT_RESOURCE_UI_PATH:
			output = (System.Object)GetCachePath (_inst);
			break;

		case CacheParam.GET_INSTANT_RESOURCE_INPUT_PATH:
			output = (System.Object)GetCachePath (_inst);
			break;

		case CacheParam.GET_CHARACTER:
			{
				output = GetCache (
					GetCachePath (CacheParam.GET_CHARACTER_PATH, _params), 
					GetCacheAsset (CacheParam.GET_CHARACTER_PATH, _params), 
					(bool)_params [2]);
			}
			break;

		case CacheParam.GET_CONTAINER:
			{
				output = GetCache (
					GetCachePath (CacheParam.GET_CONTAINER_PATH, _params), 
					GetCacheAsset (CacheParam.GET_CONTAINER_PATH, _params), 
					(bool)_params [1]);
			}
			break;

		case CacheParam.GET_WEAPON_DATA:

			output = GetCache (
				GetCachePath (CacheParam.GET_WEAPON_DATA_PATH), 
				GetCacheAsset (CacheParam.GET_WEAPON_DATA_PATH), 
				(bool)_params [0]);
			break;

		case CacheParam.GET_WEAPON:
			{
				output = GetCache (
					GetCachePath (CacheParam.GET_WEAPON_PATH, _params), 
					GetCacheAsset (CacheParam.GET_WEAPON_PATH, _params), 
					(bool)_params [1]);
			}
			break;

		case CacheParam.GET_BULLET:
			{
				output = GetCache (
					GetCachePath (CacheParam.GET_BULLET_PATH, _params), 
					GetCacheAsset (CacheParam.GET_BULLET_PATH, _params), 
					(bool)_params [1]);
			}
			break;

		case CacheParam.GET_PORTRAIT:

			output = GetCache (
				GetCachePath (CacheParam.GET_PORTRAIT_PATH, _params), 
				GetCacheAsset (CacheParam.GET_PORTRAIT_PATH, _params), 
				(bool)_params [2]);
			break;

		case CacheParam.GET_LOCALIZATION:
			output = GetCache (
				GetCachePath (CacheParam.GET_LOCALIZATION_PATH, _params), 
				GetCacheAsset (CacheParam.GET_LOCALIZATION_PATH, _params), 
				(bool)_params [1]);
			break;

		case CacheParam.GET_CHARACTER_DATA:
			output = GetCache (
				GetCachePath (CacheParam.GET_CHARACTER_DATA_PATH, _params), 
				GetCacheAsset (CacheParam.GET_CHARACTER_DATA_PATH, _params), 
				(bool)_params [0]);
			break;

		case CacheParam.GET_CHAPTER_DATA:
			output = GetCache (
				GetCachePath (CacheParam.GET_CHAPTER_DATA_PATH, _params), 
				GetCacheAsset (CacheParam.GET_CHAPTER_DATA_PATH, _params), 
				(bool)_params [0]);
			break;

		case CacheParam.GET_CHAPTER_IMAGE:
			output = GetCache (
				GetCachePath (CacheParam.GET_CHAPTER_IMAGE_PATH, _params), 
				GetCacheAsset (CacheParam.GET_CHAPTER_IMAGE_PATH, _params), 
				(bool)_params [1]);
			break;

		case CacheParam.GET_STAGE_IMAGE:
			output = GetCache (
				GetCachePath (CacheParam.GET_STAGE_IMAGE_PATH, _params), 
				GetCacheAsset (CacheParam.GET_STAGE_IMAGE_PATH, _params), 
				(bool)_params [2]);
			break;

		case CacheParam.GET_INSTANT_RESOURCE_UI:
			{
				output = GetCache (
					GetCachePath (CacheParam.GET_INSTANT_RESOURCE_UI_PATH, _params), 
					GetCacheAsset (CacheParam.GET_INSTANT_RESOURCE_UI_PATH, _params), 
					(bool)_params [1]);
			}
			break;

		case CacheParam.GET_INSTANT_RESOURCE_INPUT:
			{
				output = GetCache (
					GetCachePath (CacheParam.GET_INSTANT_RESOURCE_INPUT_PATH, _params), 
					GetCacheAsset (CacheParam.GET_INSTANT_RESOURCE_INPUT_PATH, _params), 
					(bool)_params [1]);
			}
			break;
		}

		return output;
	}
}
