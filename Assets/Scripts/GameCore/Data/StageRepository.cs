using UnityEngine;
using System.Collections;

public class StageRepository : ScriptableObject 
{
	[System.Serializable]
	public struct StageData
	{
		public string ScemeName;

		public uint[] characterIDs;

		public int Difficult;

		public string Title;

		public string Introduction;
	}
}
