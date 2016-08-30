using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public sealed partial class WeaponManager
{
	[System.Serializable]
	public struct WeaponData
	{
		public uint WeaponID;
		public uint ModelID;
		public uint BulletID;

		public uint ShootRayCount;
		public uint AmmoCount;

		public uint HP;
		public uint ATK;

		public float ReloadTime;
		public float ShootFreq;
	}

	Dictionary<uint, WeaponData> mWeaponTable = null;

	void InitialWeaponData()
	{
		mWeaponTable = new Dictionary<uint, WeaponData> ();
	}

	void LoadWeaponData(WeaponDataRepository _Repository)
	{
		if (mWeaponTable == null)
			return;

		mWeaponTable.Clear ();

		if (_Repository == null)
			return;

		for (int Indx = 0; Indx < _Repository.WeaponDataList.Length; ++Indx) 
		{
			if (mWeaponTable.ContainsKey (_Repository.WeaponDataList [Indx].WeaponID) == true)
				continue;

			mWeaponTable.Add (_Repository.WeaponDataList [Indx].WeaponID, _Repository.WeaponDataList [Indx]);
		}
	}

	WeaponData GetWeaponData(uint _weaponID)
	{
		if (mWeaponTable == null)
			return default(WeaponData);

		if (mWeaponTable.ContainsKey(_weaponID) == false)
			return default(WeaponData);

		return mWeaponTable[_weaponID];
	}
}
