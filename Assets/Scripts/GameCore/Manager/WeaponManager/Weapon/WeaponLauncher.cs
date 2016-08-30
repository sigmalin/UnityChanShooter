using UnityEngine;
using System.Collections;

public partial class WeaponLauncher : DisplayBehaviour 
{
	public enum LauncherType
	{
		Shootgum,
	}

	[SerializeField]
	LauncherType mLauncherType;

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

	IInput mInputDevice;
	public IInput InputDevice 
	{
		get 
		{ 
			if (mInputDevice == null) 
			{
				GameObject device = (GameObject	)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.INSTANT_RESOURCE_INPUT, GetInputDevicePath ());
				if (device != null) mInputDevice = device.GetComponent<IInput> ();
			}

			return mInputDevice;
		} 
	}

	public ActorController GetController()
	{
		ActorController ctrl = null;

		switch(mLauncherType)
		{
		case LauncherType.Shootgum:
			ctrl = new Actor_UnityChan ();
			break;
		}

		return ctrl;
	}

	string GetInputDevicePath()
	{
		string resPath = string.Empty;

		switch(mLauncherType)
		{
		case LauncherType.Shootgum:
			resPath = "Shootgun";
			break;
		}

		return resPath;
	}
}
