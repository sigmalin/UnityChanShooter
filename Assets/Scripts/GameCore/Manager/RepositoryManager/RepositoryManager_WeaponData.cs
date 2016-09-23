using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public partial class RepositoryManager
{
	Dictionary<uint, WeaponDataRepository.WeaponData> mWeaponTable = null;

	void InitialWeaponData()
	{
		mWeaponTable = new Dictionary<uint, WeaponDataRepository.WeaponData> ();
	}

	void ReleaseWeaponData()
	{
		if (mWeaponTable != null) 
		{
			mWeaponTable.Clear ();

			mWeaponTable = null;
		}
	}

	void AddWeaponData(WeaponDataRepository.WeaponData _data)
	{
		if (mWeaponTable == null)
			return;

		if (mWeaponTable.ContainsKey (_data.WeaponID) == false)
			mWeaponTable.Add (_data.WeaponID, _data);
		else
			mWeaponTable [_data.WeaponID] = _data;
	}

	void LoadWeaponData(WeaponDataRepository _weaponData)
	{
		if (_weaponData == null)
			return;

		_weaponData.WeaponDataList.ToObservable ()
			.Subscribe (_ => AddWeaponData (_));
	}

	System.Object GetWeaponData(uint _id)
	{
		System.Object res = (System.Object)(default(WeaponDataRepository.WeaponData));

		if (mWeaponTable == null)
			return res;

		if (mWeaponTable.ContainsKey(_id) == false)
			return res;

		res = mWeaponTable[_id];

		return res;
	}
}
