using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;

public class SceneEnvironmentEditor
{
	[MenuItem("Assets/Environment/Environment Setting")]
	static void EnvironmentSetting()
	{
		GameObject root = Selection.activeGameObject;
		if (root == null)
			return;

		Collider[] cols = root.GetComponentsInChildren<Collider> ();
		for (int Indx = 0; Indx < cols.Length; ++Indx)
			GameObject.DestroyImmediate (cols[Indx]);
		
		Renderer[] renders = root.GetComponentsInChildren<Renderer> ();
		for (int Indx = 0; Indx < renders.Length; ++Indx) 
		{
			renders [Indx].receiveShadows = false;
			renders [Indx].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			renders [Indx].useLightProbes = true;
			renders [Indx].reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
		}
	}
}
