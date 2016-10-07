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

	uint mTargetID = 0u;


	const float CAMERA_HEIGHT_MAX = 2.4F;
	const float CAMERA_HEIGHT_MIN = 0F;

	public Mode_Follow(uint _targetID)
	{
		mTargetID = _targetID;
	}

	public void EnterMode(GameCamera _camera)
	{
		if (_camera == null)
			return;

		mCurAngle = _camera.transform.rotation.y;

		Transform target = GetTargetTrans ();
		if (target != null)
			mAngle = target.rotation.y;

		mCurHeight = 0.8f;

		mHeight = 0.8F;

		mDistance = -2F;

		mHeightScale = (CAMERA_HEIGHT_MAX - CAMERA_HEIGHT_MIN) / (Screen.height);
	}

	public void LeaveMode(GameCamera _camera)
	{
	}

	public void UpdateMode(GameCamera _camera)
	{
		if (_camera == null)
			return;

		Transform transTarget = GetTargetTrans ();
		if (transTarget == null)
			return;

		float weight = Time.deltaTime * 2f;

		mCurAngle = Mathf.LerpAngle (mCurAngle, mAngle, weight);
		mCurHeight = Mathf.Lerp (mCurHeight, mHeight, weight);

		Vector3 dir = Quaternion.Euler (new Vector3 (0F, mCurAngle, 0F)) * Vector3.forward;
		
		Vector3 lookAt = transTarget.position + dir * 4F + transTarget.up * mCurHeight; 

		Vector3 cameraAt = transTarget.position + dir * mDistance + transTarget.up * 1.2F;

		_camera.transform.position = cameraAt;
		_camera.transform.LookAt (lookAt, transTarget.up);

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_FOCUS, mTargetID, lookAt);
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

	Transform GetTargetTrans()
	{
		return (Transform)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_TRANSFORM, mTargetID);
	}
}
