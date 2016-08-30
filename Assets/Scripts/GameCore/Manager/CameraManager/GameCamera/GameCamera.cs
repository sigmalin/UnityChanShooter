using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class GameCamera : MonoBehaviour 
{
	CameraManager.CameraData mRefCameraData = null;
	public CameraManager.CameraData CameraData { get { return mRefCameraData; } }

	IMode mCameraMode = null;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		if (mCameraMode != null && CameraData != null)
			mCameraMode.UpdateMode (CameraData);	
	}

	void SetCameraMode (IMode _mode)
	{
		if (mCameraMode != null)
			mCameraMode.LeaveMode (CameraData);

		mCameraMode = _mode;

		if (mCameraMode != null)
			mCameraMode.EnterMode (CameraData);
	}

	public void ExecCommand(uint _inst, params System.Object[] _params)
	{
		switch(_inst)
		{
		case CameraInst.CAMERA_REGISTER:
			mRefCameraData = (CameraManager.CameraData)_params[0];
			break;

		case CameraInst.CAMERA_UNREGISTER:
			mRefCameraData = null;
			SetCameraMode (null);
			break;

		case CameraInst.SET_CAMERA_MODE:
			SetCameraMode ((IMode)_params[0]);
			break;

		default:
			if (mCameraMode != null)
				mCameraMode.ExecCommand (_inst, _params);
			break;
		}
	}
}
