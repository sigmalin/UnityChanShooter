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

		InitialCameraTable ();

		InitialRequestQueue ();

		InitialUpdater ();
	}

	public void OnUnRegister ()
	{
		ReleaseUpdater ();

		ReleaseRequestQueue ();

		BroadcastCommand (CameraInst.CAMERA_UNREGISTER);

		ClearCameraTable ();

		GameCore.UnRegisterCommand (CommandGroup.GROUP_CAMERA);

		GameCore.UnRegisterParam (ParamGroup.GROUP_CAMERA);
	}
		
	protected override void BatchCommand (uint _inst, params System.Object[] _params)
	{
		switch (_inst) 
		{
		case CameraInst.CAMERA_REGISTER:
			RegisterGameCamera ((GameCamera)_params[0]);
			break;

		case CameraInst.CAMERA_UNREGISTER:
			UnRegisterGameCamera ((uint)_params[0]);
			break;

		case CameraInst.MAIN_CAMERA:
			mMainCamera = (uint)_params [0];
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
			GameCamera cam = GetGameCamera ((uint)_params[0]);
			if(cam != null)
				output = (System.Object)(cam.gameObject);
			break;
		}

		return output;
	}

	void TransCommand(uint _ID, uint _inst, params System.Object[] _params)
	{
		GameCamera cam = GetGameCamera (_ID);
		if (cam != null) 
			cam.ExecCommand (_inst, _params);
	}

	void BroadcastCommand (uint _inst, params System.Object[] _params)
	{
		GameCamera[] cameras = GetAllGameCamera ();

		for (int Indx = 0; Indx < cameras.Length; ++Indx) 
		{
			GameCamera cam = cameras[Indx];
			if (cam != null) 
				cam.ExecCommand (_inst, _params);
		}
	}
}
