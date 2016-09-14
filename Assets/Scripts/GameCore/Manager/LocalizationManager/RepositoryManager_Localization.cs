using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public partial class RepositoryManager
{
	Dictionary<string, Dictionary<int, string>> mLocalizationTable = null;

	void InitialLocalizationData()
	{
		ReleaseLocalizationData ();

		mLocalizationTable = new Dictionary<string, Dictionary<int, string>> ();
	}

	void ReleaseLocalizationData()
	{
		if (mLocalizationTable != null) 
		{
			string[] keys = mLocalizationTable.Keys.ToArray();

			keys.ToObservable ()
				.Where (_ => mLocalizationTable.ContainsKey (_))
				.Subscribe (_ => mLocalizationTable [_].Clear ());

			mLocalizationTable.Clear ();

			mLocalizationTable = null;
		}
	}

	void AddLocalization(LocalizationRepository.GroupData _data)
	{
		if (mLocalizationTable == null)
			return;

		if (mLocalizationTable.ContainsKey (_data.Group) == false)
			mLocalizationTable.Add (_data.Group, new Dictionary<int, string> ());

		Dictionary<int, string> table = mLocalizationTable[_data.Group];

		_data.List.ToObservable ()
			.Subscribe (_ => {
				if(table.ContainsKey(_.Key) == false)
					table.Add(_.Key, _.Context);
				else
					table[_.Key] = _.Context;
		});
	}

	void LoadLocalization(LocalizationRepository _localization)
	{
		if (mLocalizationTable == null || _localization == null)
			return;

		_localization.Source.ToObservable ()
			.Subscribe (_ => AddLocalization (_));

		GameCore.Localization();
	}

	System.Object GetLocalization(string _group, int _key)
	{
		System.Object res = (System.Object)(string.Empty);

		if (mLocalizationTable == null)
			return res;

		if (mLocalizationTable.ContainsKey (_group) == false)
			return res;

		if (mLocalizationTable [_group] == null || mLocalizationTable [_group].ContainsKey (_key) == false)
			return res;

		res = (System.Object)mLocalizationTable [_group][_key];

		return res;
	}
}
