using UnityEngine;
using UnityEditor;
using System.IO;

public class ScriptableObjectUtility
{
	public static void CreateAsset<T>() where T : ScriptableObject
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

		string assetPath = AssetDatabase.GenerateUniqueAssetPath (path + "/ New " + typeof(T).ToString() + ".asset");

		AssetDatabase.CreateAsset (asset, assetPath);

		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh ();
		EditorUtility.FocusProjectWindow ();

		Selection.activeObject = asset;
	}
}
