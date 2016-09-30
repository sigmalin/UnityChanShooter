using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public partial class CameraManager
{
	public class CameraData
	{
		public uint CameraID;
		public uint TargetID;

		public GameCamera RefCamera;

		public void Clear()
		{
			RefCamera = null;
		}
	}

	Dictionary<uint, CameraData> mCameraTable = null;

	void InitialCameraData()
	{
		mCameraTable = new Dictionary<uint, CameraData> ();
	}

	void RegisterNewCamera(uint _cameraID, GameCamera _camera)
	{
		if (mCameraTable.ContainsKey (_cameraID) == true) 
		{
			mCameraTable [_cameraID].Clear ();
			mCameraTable.Remove (_cameraID);
		}

		CameraData data = new CameraData ();

		data.CameraID = _cameraID;
		data.RefCamera = _camera;

		mCameraTable.Add (_cameraID, data);
	}

	CameraData GetCameraData(uint _cameraID)
	{
		if (mCameraTable.ContainsKey (_cameraID) == false)
			return null;

		return mCameraTable[_cameraID];
	}

	uint[] GetAllCameraID()
	{
		return mCameraTable.Keys.ToArray ();
	}

	CameraData[] GetAllCameraData()
	{
		return mCameraTable.Values.ToArray ();
	}

	void ClearCameraData()
	{
		foreach (CameraData data in mCameraTable.Values) 
		{
			if (data != null)
				data.Clear ();
		}

		mCameraTable.Clear ();
	}

	GameObject GetCameraObject(uint _cameraID)
	{
		CameraData data = GetCameraData (_cameraID);
		if (data == null)
			return null;

		return data.RefCamera.gameObject;
	}
}
