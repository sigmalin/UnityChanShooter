using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniRx;
using UniRx.Triggers;

public partial class CacheManager 
{
	void LoadVersionList()
	{
		ScheduledNotifier<float> progressNotifier = new ScheduledNotifier<float>();
		progressNotifier.Subscribe(x => Debug.Log(x));

		ObservableWWW.GetWWW (SERVER_PATH + VERSION_LIST_BUNDLE + ASSET_BUNDLE_EXTENSION, progress: progressNotifier)
			.Subscribe 
			(
				_ => VersionVerify(_),
				_ex => Debug.LogError(_ex)
			);
	}

	void VersionVerify(WWW _version)
	{		
		VersionRepository data = (VersionRepository)_version.assetBundle.LoadAsset (ASSET_PATH + VERSION_LIST_ASSET + ASSET_TYPE_SCRIPTABLE_OBJECT_EXTENSION);

		if (Compatibility (data.Version) == true) 
		{
			UpdateCacheCatalogue (data);

			VersionRepository.VersionInfo[] downloadList = GetDownLoadList (data);

			if (downloadList.Length != 0)
				DownLoadAndWrite2Stream (downloadList);
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

	VersionRepository.VersionInfo[] GetDownLoadList(VersionRepository _data)
	{
		List<VersionRepository.VersionInfo> downloadList = new List<VersionRepository.VersionInfo> ();

		for (int catalogueIndx = 0; catalogueIndx < _data.CacheCatalogue.Length; ++catalogueIndx) 
		{
			VersionRepository.Catalogue catalogue = _data.CacheCatalogue[catalogueIndx];

			for (int Indx = 0; Indx < catalogue.List.Length; ++Indx) 
			{
				string path = string.Format (catalogue.PathIndex, catalogue.List[Indx].DataPath);

				int curVersion = PlayerPrefs.GetInt (path, 0);

				if (curVersion != catalogue.List[Indx].Version || IsLocalCacheExist (path) == false) 
				{
					downloadList.Add (new VersionRepository.VersionInfo(path, catalogue.List[Indx].Version));
				}
			}
		}

		return downloadList.ToArray();
	}

	void UpdateCacheCatalogue(VersionRepository _data)
	{
		for (int Indx = 0; Indx < _data.CacheCatalogue.Length; ++Indx) 
		{
			AddCacheCatalogue (_data.CacheCatalogue [Indx].Key, _data.CacheCatalogue [Indx].PathIndex, _data.CacheCatalogue [Indx].AssetIndex);
		}
	}

	string GetDownLoadPath(string _path)
	{
		return string.Format ("{0}{1}{2}", SERVER_PATH, _path, ASSET_BUNDLE_EXTENSION);
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

	void DownLoadAndWrite2StreamAsync(VersionRepository.VersionInfo[] _list)
	{
		ScheduledNotifier<float> progressNotifier = new ScheduledNotifier<float>();
		progressNotifier.Subscribe(x => Debug.Log(x));

		IObservable<byte[]>[] downloads = new IObservable<byte[]>[_list.Length]; 

		for (int Indx = 0; Indx < _list.Length; ++Indx) 
		{
			downloads [Indx] = ObservableWWW.GetAndGetBytes (GetDownLoadPath(_list[Indx].DataPath), progress: progressNotifier);
		}

		Observable.WhenAll (downloads)
			.Subscribe 
			(
				_ => {
					
					for (int Indx = 0; Indx < _.Length; ++Indx)
					{
						WriteStreamAndUpdate(_list[Indx], _ [Indx]);
					}

					GameCore.SendFlowEvent(FlowEvent.VERSION_VERFITY_COMPLETED);
				},
				ex => {
					Debug.Log ("Failure = " + ex.ToString ());
					GameCore.SendFlowEvent(FlowEvent.VERSION_VERFITY_FAILURE);
				}
			);
	}

	void DownLoadAndWrite2Stream(VersionRepository.VersionInfo[] _list)
	{
		ScheduledNotifier<float> progressNotifier = new ScheduledNotifier<float>();
		progressNotifier.Subscribe(x => Debug.Log(x));

		_list
			.Select 
			(
				loadfile =>
				{
					return Observable.Defer<byte[]> (() => ObservableWWW.GetAndGetBytes (GetDownLoadPath(loadfile.DataPath), progress: progressNotifier))
						.Do<byte[]>(bytes => WriteStreamAndUpdate(loadfile, bytes));
				}
			)
			.Aggregate ((pre, cur) => pre.SelectMany (cur))
			.Subscribe (_ => GameCore.SendFlowEvent(FlowEvent.VERSION_VERFITY_COMPLETED),
				ex => {
					Debug.Log ("Failure = " + ex.ToString ());
					GameCore.SendFlowEvent(FlowEvent.VERSION_VERFITY_FAILURE);
				});
	}

	void WriteStreamAndUpdate(VersionRepository.VersionInfo _info, byte[] _bytes)
	{
		string path = GetWritePath(_info.DataPath);

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

		PlayerPrefs.SetInt (_info.DataPath, _info.Version);
	}
}
