using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class LocalizationRepositoryEditor
{
	[MenuItem("Assets/Game Core/Localization/Tradition Chinese")]
	static void GenerateLocalizationTc()
	{
		GenerateLocalization ("Tc");
	}

	[MenuItem("Assets/Game Core/Localization/English")]
	static void GenerateLocalizationEn()
	{
		GenerateLocalization ("En");
	}

	[MenuItem("Assets/Game Core/Localization/Japon")]
	static void GenerateLocalizationJp()
	{
		GenerateLocalization ("Jp");
	}

	static void GenerateLocalization(string _language)
	{
		if (UnityEditor.Selection.activeObject is TextAsset) 
		{
			TextAsset source = UnityEditor.Selection.activeObject as TextAsset;

			string[][] grid = CsvParser2.Parse(source.text);
			if (grid.Length < 1)
				return;

			int gruopRow = grid [0]
				.Select((_value, _index) => new {index = _index, value = _value})
				.Where(_ => string.Equals ("Group", _.value))
				.Select(_ => _.index).First();

			int keyRow = grid [0]
				.Select((_value, _index) => new {index = _index, value = _value})
				.Where(_ => string.Equals ("Key", _.value))
				.Select(_ => _.index).First();

			int languageRow = grid [0]
				.Select((_value, _index) => new {index = _index, value = _value})
				.Where(_ => string.Equals (_language, _.value))
				.Select(_ => _.index).First();

			CreateAsset (GetPath(_language), CreateLocalization(grid, gruopRow, keyRow, languageRow));
		}
	}

	static string GetPath(string _language)
	{
		return string.Format ("LocalizationRepository_{0}", _language);
	}

	static LocalizationRepository.GroupData[] CreateLocalization(string[][] _source, int _group, int _key, int _context)
	{
		Dictionary<string, List<LocalizationRepository.TextData>> table = new Dictionary<string, List<LocalizationRepository.TextData>>();

		for (int row = 1; row < _source.Length; ++row) 
		{
			string group = _source [row] [_group];
			string key = _source [row] [_key];

			if (string.IsNullOrEmpty (group) == true)
				continue;

			if (string.IsNullOrEmpty (key) == true)
				continue;

			int res;

			if (int.TryParse (key, out res) == false)
				continue;

			if (table.ContainsKey(group) == false)
				table.Add(group, new List<LocalizationRepository.TextData>());

			table [group].Add (new LocalizationRepository.TextData () { Key = res, Context = _source [row] [_context] });
		}



		return table.Select (_ => new LocalizationRepository.GroupData () {
			Group = _.Key,
			List = _.Value.ToArray ()
		}).ToArray();
	}

	static void CreateAsset(string _path, LocalizationRepository.GroupData[] _source)
	{Debug.Log (_path);
		LocalizationRepository asset = ScriptableObjectUtility.CreateAsset<LocalizationRepository> (_path);

		asset.Source = _source;

		Debug.Log (_source.Length);

		//AssetDatabase.CreateAsset (asset, _path);
		EditorUtility.SetDirty(asset);
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh ();
	}
}
