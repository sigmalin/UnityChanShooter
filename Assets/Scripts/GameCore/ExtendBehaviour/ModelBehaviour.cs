using UnityEngine;
using System.Collections;

public class ModelBehaviour : MonoBehaviour 
{
	[System.Serializable]
	public class BodyPoint
	{
		public Transform Eye;
		public Transform RightHand;
		public Transform LeftHand;
		public Transform AimPt;
	}

	[SerializeField]
	BodyPoint mBodyPoint;
	public BodyPoint BodyPt { get { return mBodyPoint; } }

	[SerializeField]
	Transform[] mBones;
	public Transform[] Bones 
	{ 
		get { return mBones; } 
		#if UNITY_EDITOR
		set { mBones = value; } 
		#endif
	}
}
