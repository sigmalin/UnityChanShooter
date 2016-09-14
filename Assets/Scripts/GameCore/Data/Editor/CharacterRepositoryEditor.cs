using UnityEngine;
using UnityEditor;

public class CharacterRepositoryEditor : MonoBehaviour 
{
	[MenuItem("Assets/Game Core/Create Character Data")]
	static void CreateWeaponData()
	{
		ScriptableObjectUtility.CreateAsset<CharacterRepository> ();
	}
}
