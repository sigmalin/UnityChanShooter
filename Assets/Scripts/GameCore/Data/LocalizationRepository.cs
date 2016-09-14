using UnityEngine;
using System.Collections;

public class LocalizationRepository : ScriptableObject 
{
	[System.Serializable]
	public struct TextData
	{
		public int Key;
		public string Context;
	}

	[System.Serializable]
	public struct GroupData
	{
		public string Group;
		public TextData[] List;
	}

	[SerializeField]
	GroupData[] mGroupDataList;

	public GroupData[] Source 
	{ 
		get { return mGroupDataList; } 

#if UNITY_EDITOR
		set { mGroupDataList = value; } 
#endif
	}
}
