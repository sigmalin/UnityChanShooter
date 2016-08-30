using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Actor))]
public class ActorEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		Actor actor = (Actor)target;

		base.DrawDefaultInspector ();

		GUILayout.Space (8F);

		Color defaultColor = GUI.color;

		GUI.color = Color.green;

		if (GUILayout.Button ("Collect Renderers")) 
		{
			actor.CollectRenderers ();
		}

		GUI.color = defaultColor;
	}
}
