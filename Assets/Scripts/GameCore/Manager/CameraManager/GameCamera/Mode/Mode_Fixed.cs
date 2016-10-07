using UnityEngine;
using System.Collections;

public sealed class Mode_Fixed : IMode 
{
	Vector3 mCameraAt;

	Vector3 mLookAt;

	public Mode_Fixed(Vector3 _cameraAt, Vector3 _lookAt)
	{
		mCameraAt = _cameraAt;

		mLookAt = _lookAt;
	}

	public void EnterMode(GameCamera _camera)
	{
		_camera.transform.position = mCameraAt;

		_camera.transform.LookAt (mLookAt);
	}

	public void LeaveMode(GameCamera _camera)
	{
	}

	public void UpdateMode(GameCamera _camera)
	{
	}

	public void ExecCommand(uint _inst, params System.Object[] _params)
	{
	}
}
