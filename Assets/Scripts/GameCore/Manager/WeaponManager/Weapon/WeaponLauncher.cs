using UnityEngine;
using System.Collections;

public partial class WeaponLauncher : DisplayBehaviour 
{
	[System.Serializable]
	public class Equipment
	{
		public GameObject RightHandArmGO;
		public GameObject LeftHandArmGO;
	}

	[SerializeField]
	Equipment mEquipment;


	public GameObject RightHand { get { return mEquipment.RightHandArmGO; } }
	public GameObject LeftHand  { get { return mEquipment.LeftHandArmGO; } }

	IArm mRightArm;
	public IArm RightArm 
	{
		get 
		{
			if (mRightArm == null && RightHand != null) 
				mRightArm = RightHand.GetComponent<IArm> ();
			
			return mRightArm;
		}
	}

	IArm mLeftArm;
	public IArm LeftArm 
	{
		get 
		{
			if (mLeftArm == null && LeftHand != null) 
				mLeftArm = LeftHand.GetComponent<IArm> ();
			
			return mLeftArm;
		}
	}
}
