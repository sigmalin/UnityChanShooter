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
	}

	[SerializeField]
	GameVersion mGameVersion;

	[SerializeField]
	VersionInfo[] mVersionInfoList;

	public GameVersion Version { get { return mGameVersion; } }
	public VersionInfo[] VersionInfoList { get { return mVersionInfoList; } }
}
