using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public partial class RepositoryManager
{
	Dictionary<uint, CharacterRepository.CharacterData> mCharacterTable = null;

	void InitialCharacterData()
	{
		ReleasCharacterData ();

		mCharacterTable = new Dictionary<uint, CharacterRepository.CharacterData> ();
	}

	void ReleasCharacterData()
	{
		if (mCharacterTable != null) 
		{
			mCharacterTable.Clear ();

			mCharacterTable = null;
		}
	}

	void AddCharacterData(CharacterRepository.CharacterData _data)
	{
		if (mCharacterTable == null)
			return;

		if (mCharacterTable.ContainsKey (_data.ID) == false)
			mCharacterTable.Add (_data.ID, _data);
		else
			mCharacterTable [_data.ID] = _data;
	}

	void LoadCharacterData(CharacterRepository _characterData)
	{
		if (_characterData == null)
			return;

		_characterData.CharacterDataList.ToObservable ()
			.Subscribe (_ => AddCharacterData (_));
	}

	System.Object GetCharacterData(uint _id)
	{
		System.Object res = (System.Object)(string.Empty);

		if (mCharacterTable == null)
			return res;

		if (mCharacterTable.ContainsKey(_id) == false)
			return res;

		res = mCharacterTable[_id];

		return res;
	}
}
