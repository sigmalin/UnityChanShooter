using UnityEngine;
using System.Collections;

public sealed partial class CameraManager : CommandBehaviour, IParam, IRegister
{
	uint mMainCamera = 0;

	// Use this for initialization
	void Start () 
	{
		
	}

	public void OnRegister ()
	{
		GameCore.RegisterCommand (CommandGroup.GROUP_CAMERA, this);		

		GameCore.RegisterParam (ParamGroup.GROUP_CAMERA, this);

		InitialCameraData ();

		InitialRequestQueue ();

		InitialUpdater ();
	}

	public void OnUnRegister ()
	{
		ReleaseUpdater ();

		ReleaseRequestQueue ();

		BroadcastCommand (CameraInst.CAMERA_UNREGISTER);

		ClearCameraData ();

		GameCore.UnRegisterCommand (CommandGroup.GROUP_CAMERA);

		GameCore.UnRegisterParam (ParamGroup.GROUP_CAMERA);
	}
		
	protected override void BatchCommand (uint _inst, params System.Object[] _params)
	{
		switch (_inst) 
		{
		case CameraInst.CAMERA_REGISTER:
			RegisterNewCamera ((uint)_params[0], (GameCamera)_params[1]);
			TransCommand((uint)_params[0], CameraInst.CAMERA_REGISTER, GetCameraData ((uint)_params[0]));
			break;

		case CameraInst.MAIN_CAMERA:
			mMainCamera = (uint)_params [0];
			break;

		case CameraInst.CAMERA_TARGET:
			CameraData camera = GetCameraData ((uint)_params [0]);
			if (camera.RefCamera != null)
				camera.TargetID = (uint)_params [1];
			break;

		default:
			System.Object[] objects = new System.Object[_params.Length - 1];
			System.Array.Copy (_params, 1, objects, 0, _params.Length - 1);
			TransCommand ((uint)_params[0], _inst, objects);
			break;
		}
	}

	public System.Object GetParameter (uint _inst, params System.Object[] _params)
	{
		System.Object output = default(System.Object);

		switch (_inst) 
		{
		case CameraParam.MAIN_CAMERA:
			output = (System.Object)mMainCamera;
			break;

		case CameraParam.CAMERA_OBJECT:
			output = (System.Object)GetCameraObject ((uint)_params[0]);
			break;
		}

		return output;
	}

	void TransCommand(uint _ID, uint _inst, params System.Object[] _params)
	{
		CameraData camera = GetCameraData (_ID);
		if (camera != null && camera.RefCamera != null) 
			camera.RefCamera.ExecCommand (_inst, _params);
	}

	void BroadcastCommand (uint _inst, params System.Object[] _params)
	{
		CameraData[] cameras = GetAllCameraData ();

		for (int Indx = 0; Indx < cameras.Length; ++Indx) 
		{
			CameraData camera = cameras[Indx];
			if (camera != null && camera.RefCamera != null) 
				camera.RefCamera.ExecCommand (_inst, _params);
		}
	}
}
