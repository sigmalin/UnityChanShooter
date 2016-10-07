using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;

[CustomEditor(typeof(Role))]
public class RoleInspector : Editor 
{
	public override void OnInspectorGUI()
	{
		base.DrawDefaultInspector ();

		if (GUILayout.Button ("Scan Role")) 
		{
			ScaneBone ();

			ScanBodyPart ();
		}
	}

	void ScaneBone()
	{
		Role ragdoll = (Role)target;

		ragdoll.Bones = ragdoll.gameObject.GetComponentsInChildren<Transform> ();
	}

	void ScanBodyPart()
	{
		Role ragdoll = (Role)target;

		ragdoll.BodyPt.AimPt = ragdoll.Bones.Where(_ => string.Equals("Aim", _.name)).FirstOrDefault();
		ragdoll.BodyPt.Eye = ragdoll.Bones.Where(_ => string.Equals("Eye", _.name)).FirstOrDefault();
		ragdoll.BodyPt.LeftHand = ragdoll.Bones.Where(_ => string.Equals("LeftHangle", _.name)).FirstOrDefault();
		ragdoll.BodyPt.RightHand = ragdoll.Bones.Where(_ => string.Equals("RightHangle", _.name)).FirstOrDefault();
	}
}
