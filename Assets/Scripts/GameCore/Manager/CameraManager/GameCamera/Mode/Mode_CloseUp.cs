using UnityEngine;
using System.Collections;

public sealed class Mode_CloseUp : IMode 
{
	Vector3 mCameraAt;

	Vector3 mLookAt;

	float mOriScaleTime;

	float mWantScaleTime;

	public Mode_CloseUp(Vector3 _cameraAt, Vector3 _lookAt, float _scaleTime)
	{
		mCameraAt = _cameraAt;

		mLookAt = _lookAt;

		mWantScaleTime = _scaleTime;
	}

	public void EnterMode(GameCamera _camera)
	{
		_camera.transform.position = mCameraAt;

		_camera.transform.LookAt (mLookAt);

		mOriScaleTime = Time.timeScale;

		Time.timeScale = mWantScaleTime;
	}

	public void LeaveMode(GameCamera _camera)
	{
		Time.timeScale = mOriScaleTime;
	}

	public void UpdateMode(GameCamera _camera)
	{
	}

	public void ExecCommand(uint _inst, params System.Object[] _params)
	{
	}
}
