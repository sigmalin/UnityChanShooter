using UnityEngine;
using UnityEditor;

public class ChapterRepositoryEditor
{
	[MenuItem("Assets/Game Core/Create Chapter Data")]
	static void CreateChapterData()
	{
		ScriptableObjectUtility.CreateAsset<ChapterRepository> ();
	}

}
