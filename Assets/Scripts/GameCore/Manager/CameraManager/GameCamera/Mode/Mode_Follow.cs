using UnityEngine;
using System.Collections;

public sealed class Mode_Follow : IMode 
{
	float mCurAngle = 0F;

	float mCurHeight = 1.2F;


	float mAngle = 0F;

	float mHeight = 1.2F;


	float mDistance = -1.2F;

	float mHeightScale = 1F;


	const float CAMERA_HEIGHT_MAX = 2.4F;
	const float CAMERA_HEIGHT_MIN = 0F;

	public void EnterMode(CameraManager.CameraData _cameraData)
	{
		if (_cameraData.RefCamera == null)
			return;

		mCurAngle = _cameraData.RefCamera.transform.rotation.y;

		mAngle = GetTargetTrans(_cameraData).rotation.y;

		mCurHeight = 0.8f;

		mHeight = 0.8F;

		mDistance = -2F;

		mHeightScale = (CAMERA_HEIGHT_MAX - CAMERA_HEIGHT_MIN) / (Screen.height);
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

		float weight = Time.deltaTime * 2f;

		mCurAngle = Mathf.LerpAngle (mCurAngle, mAngle, weight);
		mCurHeight = Mathf.Lerp (mCurHeight, mHeight, weight);

		Vector3 dir = Quaternion.Euler (new Vector3 (0F, mCurAngle, 0F)) * Vector3.forward;
		
		Vector3 lookAt = transTarget.position + dir * 4F + transTarget.up * mCurHeight; 

		Vector3 cameraAt = transTarget.position + dir * mDistance + transTarget.up * 1.2F;

		_cameraData.RefCamera.transform.position = cameraAt;
		_cameraData.RefCamera.transform.LookAt (lookAt, transTarget.up);

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_FOCUS, _cameraData.TargetID, lookAt);
	}

	public void ExecCommand(uint _inst, params System.Object[] _params)
	{
		switch(_inst)
		{
		case CameraInst.CAMERA_MOVEMENT:
			mAngle += (float)_params [0];
			float varHeight = ((float)_params [1]) * mHeightScale;
			mHeight = Mathf.Clamp (mHeight + varHeight, CAMERA_HEIGHT_MIN, CAMERA_HEIGHT_MAX);
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
