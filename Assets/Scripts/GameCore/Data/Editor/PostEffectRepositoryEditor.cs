using UnityEngine;
using UnityEditor;

public class PostEffectRepositoryEditor
{
	[MenuItem("Assets/Game Core/Create Post Effect Database")]
	static void CreateSpriteDatabase()
	{
		ScriptableObjectUtility.CreateAsset<PostEffectRepository> ();
	}
}
