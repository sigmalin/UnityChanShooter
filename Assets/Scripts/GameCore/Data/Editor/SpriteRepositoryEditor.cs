using UnityEngine;
using UnityEditor;

public class SpriteRepositoryEditor
{
	[MenuItem("Assets/Game Core/Create Sprite Database")]
	static void CreateSpriteDatabase()
	{
		ScriptableObjectUtility.CreateAsset<SpriteRepository> ();
	}
}
