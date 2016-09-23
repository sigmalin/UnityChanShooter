using UnityEngine;
using System.Collections;

public class ChapterRepository : ScriptableObject 
{
	[System.Serializable]
	public struct StageData
	{
		public string ScemeName;

		public uint[] characterIDs;

		public int Difficult;
	}

	[System.Serializable]
	public struct ChapterData
	{
		public int ChapterID;

		public StageData[] Stages;
	}

	[SerializeField]
	ChapterData[] mChapterDataList;

	public ChapterData[] ChapterDataList { get { return mChapterDataList; } } 
}
