using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniRx;
using UniRx.Triggers;

public partial class CacheManager 
{
	struct VersionData
	{
		public string LocalPath;
		public string LinkPath;
		public int Version;
	}

	Dictionary<string, VersionData> mVersionTable = null;

	void InitialVersionTable()
	{
		ReleaseVersionTable ();

		mVersionTable = new Dictionary<string, VersionData> ();
	}

	void ReleaseVersionTable()
	{
		if (mVersionTable != null) 
		{
			mVersionTable.Clear ();

			mVersionTable = null;
		}
	}

	void LoadVersionList()
	{		
		ScheduledNotifier<float> progressNotifier = new ScheduledNotifier<float>();
		progressNotifier.Subscribe(_ => UpdateLoadingProgress(_));

		UpdateLoadingMessage (string.Format("0/1"));

		ShowLoadingProgress ();

		ObservableWWW.GetWWW (SERVER_PATH + LINK_VERSION_LIST_BUNDLE + DOWNLOAD_ASSET_BUNDLE_EXTENSION, progress: progressNotifier)
			.Subscribe 
			(
				_ => 
				{
					HideLoadingProgress();
					VersionVerify(_);
				},
				_ex => GameCore.SendFlowEvent(FlowEvent.CONNECT_FAILURED)
			);
	}

	void VersionVerify(WWW _version)
	{		
		VersionRepository data = (VersionRepository)_version.assetBundle.LoadAsset (ASSET_PATH + VERSION_LIST_ASSET + ASSET_TYPE_SCRIPTABLE_OBJECT_EXTENSION);

		if (Compatibility (data.Version) == true) 
		{
			UpdateVersionTable (data);

			UpdateCacheCatalogue (data);

			VersionData[] downloadList = GetDownLoadList (data);

			if (downloadList.Length != 0)
				DownLoadAndWrite2Stream (downloadList, 
					() => GameCore.SendFlowEvent(FlowEvent.VERSION_VERFITY_COMPLETED),
					_ex => GameCore.SendFlowEvent(FlowEvent.VERSION_VERFITY_FAILURE));
			else
				GameCore.SendFlowEvent (FlowEvent.VERSION_VERFITY_COMPLETED);
		} 
		else 
		{
			GameCore.SendFlowEvent (FlowEvent.VERSION_INCOMPATIBLE);
		}

		_version.assetBundle.Unload (false);
	}

	bool Compatibility(VersionRepository.GameVersion _ver)
	{
		return !(GameCore.MAIN_VERSION != _ver.MainVersion || GameCore.SUB_VERSION != _ver.SubVersion);
	}

	VersionData[] GetDownLoadList(VersionRepository _data)
	{
		List<VersionData> downloadList = new List<VersionData> ();

		for (int catalogueIndx = 0; catalogueIndx < _data.CacheCatalogue.Length; ++catalogueIndx) 
		{
			VersionRepository.Catalogue catalogue = _data.CacheCatalogue[catalogueIndx];

			for (int Indx = 0; Indx < catalogue.List.Length; ++Indx) 
			{
				string path = string.Format (catalogue.PathIndex, catalogue.List[Indx].DataPath);

				bool localCacheExist = IsLocalCacheExist (path);

				if (catalogue.List [Indx].IsForceDownLoad == true || localCacheExist == true) 
				{
					int curVersion = PlayerPrefs.GetInt (path, 0);

					if (curVersion != catalogue.List[Indx].Version || localCacheExist == false) 
					{
						downloadList.Add (new VersionData { LocalPath = path, LinkPath = catalogue.List[Indx].LinkPath, Version = catalogue.List[Indx].Version });
					}
				}
			}
		}

		return downloadList.ToArray();
	}

	void UpdateVersionTable(VersionRepository _data)
	{
		if (mVersionTable == null)
			return;

		for (int catalogueIndx = 0; catalogueIndx < _data.CacheCatalogue.Length; ++catalogueIndx) 
		{
			VersionRepository.Catalogue catalogue = _data.CacheCatalogue [catalogueIndx];

			for (int Indx = 0; Indx < catalogue.List.Length; ++Indx) 
			{
				string path = string.Format (catalogue.PathIndex, catalogue.List [Indx].DataPath);

				if (mVersionTable.ContainsKey (path) == true)
					mVersionTable.Remove (path);

				mVersionTable.Add (path, new VersionData { LocalPath = path, LinkPath = catalogue.List [Indx].LinkPath, Version = catalogue.List [Indx].Version });
			}
		}
	}

	void UpdateCacheCatalogue(VersionRepository _data)
	{
		for (int Indx = 0; Indx < _data.CacheCatalogue.Length; ++Indx) 
		{
			AddCacheCatalogue (_data.CacheCatalogue [Indx].Key, _data.CacheCatalogue [Indx].PathIndex, _data.CacheCatalogue [Indx].AssetIndex);
		}
	}

	VersionData GetVersionData(string _path)
	{
		if (mVersionTable == null)
			return default(VersionData);

		return mVersionTable.ContainsKey (_path) ? mVersionTable[_path] : default(VersionData);
	}

	string GetDownLoadPath(string _link)
	{
		return string.Format ("{0}{1}{2}", SERVER_PATH, _link, DOWNLOAD_ASSET_BUNDLE_EXTENSION);
	}

	string GetWritePath(string _path)
	{
		return GetCachePath (_path);
	}

	bool IsLocalCacheExist(string _path)
	{
		string localpath = GetWritePath(_path);
		bool res = File.Exists (localpath);

		Debug.Log (string.Format("{0}:{1}",localpath,res));
		return res;
	}

	void DownLoadAndWrite2StreamAsync(VersionData[] _list, System.Action _onCompleted, System.Action<System.Exception> _onError)
	{
		ScheduledNotifier<float> progressNotifier = new ScheduledNotifier<float>();
		progressNotifier.Subscribe(x => Debug.Log(x));

		IObservable<byte[]>[] downloads = new IObservable<byte[]>[_list.Length]; 

		for (int Indx = 0; Indx < _list.Length; ++Indx) 
		{
			downloads [Indx] = ObservableWWW.GetAndGetBytes (GetDownLoadPath(_list[Indx].LinkPath), progress: progressNotifier);
		}

		Observable.WhenAll (downloads)
			.Subscribe 
			(
				_ => {
					
					for (int Indx = 0; Indx < _.Length; ++Indx)
					{
						WriteStreamAndUpdate(_list[Indx], _ [Indx]);
					}

					_onCompleted();
				},
				ex => {
					Debug.Log ("Failure = " + ex.ToString ());
					_onError(ex);
				}
			);
	}

	void DownLoadAndWrite2Stream(VersionData[] _list, System.Action _onCompleted, System.Action<System.Exception> _onError)
	{
		ScheduledNotifier<float> progressNotifier = new ScheduledNotifier<float>();
		progressNotifier.Subscribe(x => UpdateLoadingProgress(x));

		UpdateLoadingMessage (string.Format("0/{0}",_list.Length));

		ShowLoadingProgress ();

		_list
			.Select 
			(
				(loadfile, Index) =>
				{
					string msg = string.Format("{0}/{1}", Index+1, _list.Length);
					return Observable.Defer<byte[]> (() => ObservableWWW.GetAndGetBytes (GetDownLoadPath(loadfile.LinkPath), progress: progressNotifier))
						.Do<byte[]>(bytes => WriteStreamAndUpdate(loadfile, bytes))
						.Do(_ => UpdateLoadingMessage (msg));
				}
			)
			.Aggregate ((pre, cur) => pre.SelectMany (cur))
			.Subscribe (
				_ => {
					HideLoadingProgress();
					_onCompleted();
				},
				ex => _onError(ex));
	}

	void WriteStreamAndUpdate(VersionData _info, byte[] _bytes)
	{
		string path = GetWritePath(_info.LocalPath);

		Debug.Log ("Write File = " + path);

		string directoryPath = Path.GetDirectoryName(path);

		if (Directory.Exists (directoryPath) == false) 
		{
			Directory.CreateDirectory (directoryPath);
		}

		using (FileStream file = new FileStream (path, FileMode.Create)) 
		{
			file.Write (_bytes, 0, _bytes.Length);
			file.Close ();
		}

		PlayerPrefs.SetInt (_info.LocalPath, _info.Version);
	}
}
