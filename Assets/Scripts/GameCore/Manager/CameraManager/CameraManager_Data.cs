using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public partial class CameraManager
{
	Dictionary<uint, GameCamera> mCameraTable = null;

	void InitialCameraTable()
	{
		mCameraTable = new Dictionary<uint, GameCamera> ();
	}

	void RegisterGameCamera(GameCamera _camera)
	{
		if (_camera == null)
			return;

		if (mCameraTable.ContainsKey (_camera.CameraID) == true) 
		{
			mCameraTable [_camera.CameraID].Clear ();
			mCameraTable.Remove (_camera.CameraID);
		}

		mCameraTable.Add (_camera.CameraID, _camera);
	}

	void UnRegisterGameCamera(uint _cameraID)
	{
		if (mCameraTable.ContainsKey (_cameraID) == false)
			return;

		mCameraTable [_cameraID].Clear ();
		mCameraTable.Remove (_cameraID);
	}

	GameCamera GetGameCamera(uint _cameraID)
	{
		if (mCameraTable.ContainsKey (_cameraID) == false)
			return null;

		return mCameraTable[_cameraID];
	}

	uint[] GetAllCameraID()
	{
		return mCameraTable.Keys.ToArray ();
	}

	GameCamera[] GetAllGameCamera()
	{
		return mCameraTable.Values.ToArray ();
	}

	void ClearCameraTable()
	{
		foreach (GameCamera data in mCameraTable.Values) 
		{
			if (data != null)
				data.Clear ();
		}

		mCameraTable.Clear ();
	}
}
