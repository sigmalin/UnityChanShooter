using UnityEngine;
using System.Collections;
using UniRx;

public partial class Flow_WipeOut
{
	IMode mMainCameraMode;

	IMode mExitCameraMode;

	void InitialCameraMode()
	{
		mMainCameraMode = new Mode_Follow (MAIN_PLAYER_ID);

		mExitCameraMode = null;
	}

	void SetMainCameraMode()
	{
		GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.SET_CAMERA_MODE, mCameraList.MainCamera.CameraID, mMainCameraMode);
	}

	void SetVictoryCameraMode()
	{
		Transform playerTrans = (Transform)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_TRANSFORM, MAIN_PLAYER_ID);

		Vector3 cameraAt = playerTrans.position + (playerTrans.forward * 1f) + (playerTrans.up * 1f);

		Vector3 lookAt = playerTrans.position + (playerTrans.up * 0.5f);

		if (mExitCameraMode != null)
			GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.REMOVE_CAMERA_MODE, mCameraList.MainCamera.CameraID, mExitCameraMode);

		mExitCameraMode = new Mode_Fixed (cameraAt, lookAt);

		GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.SET_CAMERA_MODE, mCameraList.MainCamera.CameraID, mExitCameraMode);
	}

	void SetCloseUpLastEnemyCameraMode()
	{
		uint[] enemyIDs = (uint[])GameCore.GetParameter (ParamGroup.GROUP_WEAPON, WeaponParam.GET_HOSTILITY_LIST, MAIN_PLAYER_ID);

		if (enemyIDs.Length == 0)
			return;

		uint murdererID = (uint)GameCore.GetParameter (ParamGroup.GROUP_WEAPON, WeaponParam.GET_LASTEST_MURDERER, enemyIDs[0]);

		Transform murdererTrans = (Transform)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_TRANSFORM, murdererID);
		if (murdererTrans == null)
			return;

		Transform victimsTrans = (Transform)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_TRANSFORM, enemyIDs[0]);
		if (victimsTrans == null)
			return;

		Vector3 dir = murdererTrans.position - victimsTrans.position;

		Vector3 cameraAt = victimsTrans.position + (victimsTrans.right * dir.magnitude) + (victimsTrans.up * 1f);

		Vector3 lookAt = murdererTrans.position + (murdererTrans.up * 0.5f);

		if (mExitCameraMode != null)
			GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.REMOVE_CAMERA_MODE, mCameraList.MainCamera.CameraID, mExitCameraMode);

		mExitCameraMode = new Mode_CloseUp (cameraAt, lookAt, 0.2f);

		GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.SET_CAMERA_MODE, mCameraList.MainCamera.CameraID, mExitCameraMode);
	}
}
