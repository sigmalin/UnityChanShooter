using UnityEngine;
using UnityEditor;
using System.IO;

public class ScriptableObjectUtility
{
	public static T CreateAsset<T>() where T : ScriptableObject
	{
		return CreateAsset<T>(typeof(T).ToString());
	}

	public static T CreateAsset<T>(string _filename) where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T> ();

		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (string.IsNullOrEmpty (path) == true) 
		{
			path = "Assets";
		} 
		else if (string.IsNullOrEmpty (Path.GetExtension (path)) == false)
		{
			path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
		}

		string assetPath = AssetDatabase.GenerateUniqueAssetPath (path + "/" + _filename + ".asset");

		AssetDatabase.CreateAsset (asset, assetPath);

		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh ();
		//EditorUtility.FocusProjectWindow ();

		//Selection.activeObject = asset;
		return asset;
	}
}
