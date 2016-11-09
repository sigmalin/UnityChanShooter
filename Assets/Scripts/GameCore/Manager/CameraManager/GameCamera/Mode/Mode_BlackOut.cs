using UnityEngine;
using System.Collections;

public sealed class Mode_BlackOut : IMode 
{
	CameraClearFlags mClearFlag;

	int mCullingMask;

	Color mBackgroundCoor;

	public void EnterMode(GameCamera _camera)
	{
		if (_camera == null)
			return;

		Camera cam = _camera.gameObject.GetComponent<Camera> ();
		if (cam == null)
			return;

		mClearFlag = cam.clearFlags;

		mCullingMask = cam.cullingMask;

		mBackgroundCoor = cam.backgroundColor;

		cam.clearFlags = CameraClearFlags.SolidColor;

		cam.cullingMask = 0;

		cam.backgroundColor = Color.black;
	}

	public void LeaveMode(GameCamera _camera)
	{
		if (_camera == null)
			return;

		Camera cam = _camera.gameObject.GetComponent<Camera> ();
		if (cam == null)
			return;

		cam.clearFlags = mClearFlag;

		cam.cullingMask = mCullingMask;

		cam.backgroundColor = mBackgroundCoor;
	}

	public void UpdateMode(GameCamera _camera)
	{
	}

	public void ExecCommand(uint _inst, params System.Object[] _params)
	{
	}
}
