using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Camera))]
public class GameCamera : MonoBehaviour 
{
	[SerializeField]
	uint mCameraID;
	public uint CameraID { get { return mCameraID; } }

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

		if (curMode != null)
			curMode.UpdateMode (this);	
	}

	void PushCameraMode (IMode _mode)
	{
		if (_mode == null)
			return;

		if (mModeStack.Contains (_mode) == true)
			return;

		if (GetCurrentMode() != null)
			GetCurrentMode().LeaveMode (this);

		mModeStack.Push (_mode);

		_mode.EnterMode (this);
	}

	void PopCameraMode(IMode _mode)
	{
		if (GetCurrentMode () == _mode) 
		{
			mModeStack.Pop ();
			_mode.LeaveMode (this);

			if (GetCurrentMode () != null)
				GetCurrentMode ().EnterMode (this);
		} 
		else 
		{
			Stack<IMode> tempStack = new Stack<IMode> ();

			while (mModeStack.Count != 0) 
			{
				IMode mode = mModeStack.Pop ();

				if (_mode != mode)
					tempStack.Push (mode);
			}

			while (tempStack.Count != 0)
			{
				mModeStack.Push (tempStack.Pop());
			}
		}
	}

	public void ExecCommand(uint _inst, params System.Object[] _params)
	{
		switch(_inst)
		{
		case CameraInst.CAMERA_ACTIVE:
			this.gameObject.GetComponent<Camera> ().enabled = (bool)_params [0];
			break;

		case CameraInst.SET_CAMERA_MODE:
			PushCameraMode ((IMode)_params[0]);
			break;

		case CameraInst.REMOVE_CAMERA_MODE:
			PopCameraMode ((IMode)_params[0]);
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

	public void ClearStack()
	{
		if (GetCurrentMode () != null) 
		{
			GetCurrentMode ().LeaveMode (this);
			mModeStack.Clear ();
		}
	}
}
