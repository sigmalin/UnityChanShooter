using UnityEngine;
using System.Collections;

public class CharacterRepository : ScriptableObject 
{
	[System.Serializable]
	public struct CharacterData
	{
		public uint ID;

		public uint weaponID;

		public uint ability1ID;

		public uint ability2ID;

		public uint ability3ID;

		public uint skillID;
	}

	[SerializeField]
	CharacterData[] mCharacterDataList;

	public CharacterData[] CharacterDataList { get { return mCharacterDataList; } } 
}
