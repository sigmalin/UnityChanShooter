using UnityEngine;
using System.Collections;
using UniRx;

public sealed partial class InputShootgun
{
	int mUsedFingerID = -1;

	void OperatorForSmartphone()
	{
		// Move CrossHair
		InputStream.Where (_ => mUsedFingerID == -1)
			.Where (_ => Input.touchCount != 0)
			.SelectMany (_ => Input.touches.ToObservable ()
				.Where (_touch => GameCore.IsTouchInterface (_touch.position) == false)
				.First ()
			)
			.Subscribe (_ => {
				mUsedFingerID = _.fingerId;
			});

		IObservable<Touch> cameraObservable = InputStream.Where (_ => mUsedFingerID != -1)
			.SelectMany (_ => Input.touches.ToObservable ()
				.Where (_touch => mUsedFingerID == _touch.fingerId)
				.First ()
			)
			.Publish ().RefCount ();

		cameraObservable.Where(_ => _.phase != TouchPhase.Canceled && _.phase != TouchPhase.Ended)
			.Select(_ => _.position)
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

		cameraObservable.Where(_ => _.phase == TouchPhase.Canceled || _.phase == TouchPhase.Ended)
			.Subscribe (_ => {
				mUsedFingerID = -1;
			});
	}
}
