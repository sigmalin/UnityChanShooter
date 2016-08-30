using UnityEngine;
using System.Collections;

public interface IMode
{
	void EnterMode(CameraManager.CameraData _cameraData);

	void LeaveMode(CameraManager.CameraData _cameraData);

	void UpdateMode(CameraManager.CameraData _cameraData);

	void ExecCommand(uint _inst, params System.Object[] _params);
}
