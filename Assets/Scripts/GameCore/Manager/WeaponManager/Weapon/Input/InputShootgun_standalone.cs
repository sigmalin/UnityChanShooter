using UnityEngine;
using System.Collections;
using UniRx;

public sealed partial class InputShootgun
{
	void OperatorForStanealone()
	{
		// Move CrossHair
		InputStream.Where (_ => Input.GetMouseButton (0) == true)
			.Where (_ => GameCore.IsTouchInterface(Input.mousePosition) == false)
			.Where (_ => Device.IsJoyStickUsed.Value == false)
			.Select (_ => Input.mousePosition)
			.Buffer (InputStream.ThrottleFirstFrame(3))
			.Where(_ => 1 < _.Count)
			.Subscribe (_ => {
				Vector3 first = _[0];
				Vector3 last = _[_.Count - 1];
				Vector3 dir = (last - first) * 0.5f;

				GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.CAMERA_MOVEMENT, 
					(uint)GameCore.GetParameter (ParamGroup.GROUP_CAMERA, CameraParam.MAIN_CAMERA), 
					dir.x, dir.y, Input.GetAxis("Mouse ScrollWheel"));
			});
	}

	void ClearForStanealone()
	{
	}
}
