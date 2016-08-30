using UnityEngine;
using UnityEditor;

public class WeaponDataRepositoryEditor
{
	[MenuItem("Assets/Game Core/Create Weapon Data")]
	static void CreateWeaponData()
	{
		ScriptableObjectUtility.CreateAsset<WeaponDataRepository> ();
	}

}
