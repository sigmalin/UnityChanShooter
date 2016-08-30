using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(WeaponLauncher))]
public class WeaponLauncherEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		WeaponLauncher launcher = (WeaponLauncher)target;

		base.DrawDefaultInspector ();

		GUILayout.Space (8F);

		Color defaultColor = GUI.color;

		GUI.color = Color.green;

		if (GUILayout.Button ("Collect Renderers")) 
		{
			launcher.CollectRenderers ();
		}

		GUI.color = defaultColor;
	}
}
