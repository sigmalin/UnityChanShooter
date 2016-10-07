using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;

[CustomEditor(typeof(Ragdoll))]
public class RagdollInspector : Editor 
{
	public override void OnInspectorGUI()
	{
		base.DrawDefaultInspector ();

		if (GUILayout.Button ("Scan Ragdoll")) 
		{
			ScaneBone ();

			ScaneRigidbody ();

			ScanBodyPart ();
		}
	}

	void ScaneBone()
	{
		Ragdoll ragdoll = (Ragdoll)target;

		ragdoll.Bones = ragdoll.gameObject.GetComponentsInChildren<Transform> ();
	}

	void ScaneRigidbody()
	{
		Ragdoll ragdoll = (Ragdoll)target;

		ragdoll.Rigidbodies = ragdoll.gameObject.GetComponentsInChildren<Rigidbody> ();
	}

	void ScanBodyPart()
	{
		Ragdoll ragdoll = (Ragdoll)target;

		ragdoll.BodyPt.AimPt = ragdoll.Bones.Where(_ => string.Equals("Aim", _.name)).FirstOrDefault();
		ragdoll.BodyPt.Eye = ragdoll.Bones.Where(_ => string.Equals("Eye", _.name)).FirstOrDefault();
		ragdoll.BodyPt.LeftHand = ragdoll.Bones.Where(_ => string.Equals("LeftHangle", _.name)).FirstOrDefault();
		ragdoll.BodyPt.RightHand = ragdoll.Bones.Where(_ => string.Equals("RightHangle", _.name)).FirstOrDefault();
	}
}
