using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class AssetbundleBuilder
{
	const string ASSET_FILTER_ALL = "t:Texture2D t:AudioClip t:TextAsset t:AnimationClip t:Material";
	const string ASSET_FILTER_PREFAB = "t:Prefab";
	const string ASSET_FILTER_GAMECORE = "t:Prefab t:ScriptableObject t:SceneAsset t:Texture2D t:AudioClip";

	const string ASSET_SOURCE_PATH = "Assets/OutputAsset/Editor/";
	const string ASSET_OUTPUT_PATH = "../../GameCoreAssetBundles/";

	private static void ClearBuildFolder(string _path)
	{
		if (Directory.Exists (_path) == true) 
		{
			Directory.Delete (_path, true);
		}

		Directory.CreateDirectory (_path);
	}

	private static string[] GetAssetBundleFilesByDirectory(string _filter, string[] _directories) 
	{
		string[] files = AssetDatabase.FindAssets (_filter, _directories);

		string[] assetbundleList = new string[files.Length];

		for (int Indx = 0; Indx < files.Length; ++Indx) 
		{
			assetbundleList [Indx] = AssetDatabase.GUIDToAssetPath(files[Indx]);

			Debug.Log (assetbundleList [Indx]);
		}

		return assetbundleList;
	}

	static void BuildAssetBundles(string _path)
	{
		string output = ASSET_OUTPUT_PATH + _path;

		ClearBuildFolder (output);

		UnityEditor.AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

		buildMap [0].assetBundleName = Path.GetFileName (_path) + ".unity3d";
		buildMap [0].assetNames = GetAssetBundleFilesByDirectory(ASSET_FILTER_GAMECORE, new string[] {_path} );

		if (buildMap [0].assetNames.Length != 0)
			BuildPipeline.BuildAssetBundles (output, buildMap, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
	}

	[MenuItem("Assets/Game Core/Build All AssetBundles")]
	static void BuildAllAssetBundles()
	{
		string[] directories = Directory.GetDirectories (ASSET_SOURCE_PATH, "*", SearchOption.AllDirectories);

		for (int Indx = 0; Indx < directories.Length; ++Indx) 
		{
			BuildAssetBundles (directories[Indx]);
		}
	}
}
