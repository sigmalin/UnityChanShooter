using UnityEngine;
using System.Collections;

public class WeaponDataRepository : ScriptableObject
{
	[SerializeField]
	WeaponManager.WeaponData[] mWeaponDataList;

	public WeaponManager.WeaponData[] WeaponDataList { get { return mWeaponDataList; } }
}