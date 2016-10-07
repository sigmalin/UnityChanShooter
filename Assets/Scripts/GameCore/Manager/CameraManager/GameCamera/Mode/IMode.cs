using UnityEngine;
using System.Collections;

public interface IMode
{
	void EnterMode(GameCamera _cameraData);

	void LeaveMode(GameCamera _cameraData);

	void UpdateMode(GameCamera _cameraData);

	void ExecCommand(uint _inst, params System.Object[] _params);
}
