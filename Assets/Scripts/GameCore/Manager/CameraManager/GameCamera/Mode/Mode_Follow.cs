using UnityEngine;
using System.Collections;

public sealed class Mode_Follow : IMode 
{
	float mAngle = 0F;

	float mHeight = 1.2F;

	float mDistance = -1.2F;

	public void EnterMode(CameraManager.CameraData _cameraData)
	{
		if (_cameraData.RefCamera == null)
			return;

		mAngle = GetTargetTrans(_cameraData).rotation.y;

		mHeight = 1.2F;

		mDistance = -1.2F;
	}

	public void LeaveMode(CameraManager.CameraData _cameraData)
	{
	}

	public void UpdateMode(CameraManager.CameraData _cameraData)
	{
		if (_cameraData.RefCamera == null)
			return;

		Transform transTarget = GetTargetTrans (_cameraData);
		if (transTarget == null)
			return;

		Vector3 dir = Quaternion.Euler (new Vector3 (0F, mAngle, 0F)) * Vector3.forward;
		
		Vector3 lookAt = transTarget.position + dir * 4F + transTarget.up * mHeight; 

		Vector3 cameraAt = transTarget.position + dir * mDistance + transTarget.up * 1.2F;

		_cameraData.RefCamera.transform.position = cameraAt;
		_cameraData.RefCamera.transform.LookAt (lookAt, transTarget.up);

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_LOOKAT, _cameraData.TargetID, lookAt);
	}

	public void ExecCommand(uint _inst, params System.Object[] _params)
	{
		switch(_inst)
		{
		case CameraInst.CAMERA_MOVEMENT:
			mAngle += (float)_params [0];
			mHeight = Mathf.Clamp (mHeight + (float)_params [1], 0F, 2.4F);
			mDistance = Mathf.Clamp (mDistance + (float)_params [2], -2.4F, -1.2F);
			break;
		}
	}

	Transform GetTargetTrans(CameraManager.CameraData _cameraData)
	{
		if (_cameraData == null)
			return null;

		return (Transform)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_TRANSFORM, _cameraData.TargetID);
	}
}
