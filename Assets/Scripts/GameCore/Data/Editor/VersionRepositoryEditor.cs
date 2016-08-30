using UnityEngine;
using UnityEditor;

public class VersionRepositoryEditor
{
	[MenuItem("Assets/Game Core/Create Version Info")]
	static void CreateWeaponData()
	{
		ScriptableObjectUtility.CreateAsset<VersionRepository> ();
	}
}
