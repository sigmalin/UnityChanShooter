using UnityEngine;
using System.Collections;

public class VersionRepository : ScriptableObject 
{
	[System.Serializable]
	public struct GameVersion
	{
		public int MainVersion;
		public int SubVersion;
		public int DetailVersion;
	}

	[System.Serializable]
	public struct VersionInfo
	{
		public string DataPath;
		public int Version;
		public bool IsForceDownLoad;

		public VersionInfo(string _path, int _ver)
		{
			DataPath = _path;

			Version = _ver;

			IsForceDownLoad = false;
		}
	}

	[System.Serializable]
	public struct Catalogue
	{
		public uint Key;

		public string PathIndex;

		public string AssetIndex;

		public VersionInfo[] List;
	}

	[SerializeField]
	GameVersion mGameVersion;

	[SerializeField]
	Catalogue[] mCatalogue;

	public GameVersion Version { get { return mGameVersion; } }
	public Catalogue[] CacheCatalogue { get { return mCatalogue; } }
}
