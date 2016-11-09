using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public sealed partial class WeaponManager
{
	WeaponDataRepository.WeaponData GetWeaponData(uint _weaponID)
	{
		return (WeaponDataRepository.WeaponData)GameCore.GetParameter(ParamGroup.GROUP_REPOSITORY, RepositoryParam.GET_WEAPON_DATA, _weaponID);
	}

	CharacterRepository.CharacterData GetCharacterData(uint _characterID)
	{
		return (CharacterRepository.CharacterData)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY, RepositoryParam.GET_CHARACTER_DATA, _characterID);
	}
}
