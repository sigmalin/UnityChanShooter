using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Camera))]
public class GameCamera : MonoBehaviour 
{
	CameraManager.CameraData mRefCameraData = null;

	Stack<IMode> mModeStack;

	// Use this for initialization
	void Start () 
	{
		mModeStack = new Stack<IMode> ();
	}

	void OnDestroy()
	{
		ClearStack ();

		mModeStack = null;
	}
	
	// Update is called once per frame
	public void FrameMove () 
	{
		IMode curMode = GetCurrentMode ();

		if (curMode != null && mRefCameraData != null)
			curMode.UpdateMode (mRefCameraData);	
	}

	void PushCameraMode (IMode _mode)
	{
		if (_mode == null)
			return;

		if (GetCurrentMode() != null)
			GetCurrentMode().LeaveMode (mRefCameraData);

		Stack<IMode> tempStack = new Stack<IMode> ();

		while (mModeStack.Count != 0) 
		{
			if (mModeStack.Peek () != _mode)
				tempStack.Push (mModeStack.Pop());
		}

		while (tempStack.Count != 0) 
		{
			mModeStack.Push (tempStack.Pop());
		}

		mModeStack.Push (_mode);

		_mode.EnterMode (mRefCameraData);
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
			ClearStack ();
			break;

		case CameraInst.SET_CAMERA_MODE:
			PushCameraMode ((IMode)_params[0]);
			break;

		default:
			if (GetCurrentMode() != null)
				GetCurrentMode().ExecCommand (_inst, _params);
			break;
		}
	}

	IMode GetCurrentMode()
	{
		if (mModeStack == null)
			return null;

		if (mModeStack.Count == 0)
			return null;
		
		return mModeStack.Peek ();
	}

	void ClearStack()
	{
		if (mModeStack != null)
			mModeStack.Clear ();
	}
}
