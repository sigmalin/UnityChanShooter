using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Camera))]
public partial class GameCamera : MonoBehaviour 
{
	[SerializeField]
	uint mCameraID;
	public uint CameraID { get { return mCameraID; } }

	// Use this for initialization
	void Start () 
	{
		InitialMode ();

		InitialPostEffect ();
	}

	void OnDestroy()
	{
		ReleaseMode ();

		ReleasePostEffect ();
	}

	public void Clear()
	{
		ClearStack ();

		ClearPostEffect ();
	}
	
	// Update is called once per frame
	public void FrameMove () 
	{
		IMode curMode = GetCurrentMode ();

		if (curMode != null)
			curMode.UpdateMode (this);	
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

		case CameraInst.SET_CAMERA_POST_EFFECT:
			AddPostEffect ((IPostEffect)_params[0]);
			break;

		case CameraInst.REMOVE_CAMERA_POST_EFFECT:
			RemovePostEffect ((IPostEffect)_params[0]);
			break;

		default:
			if (GetCurrentMode() != null)
				GetCurrentMode().ExecCommand (_inst, _params);
			break;
		}
	}
}
