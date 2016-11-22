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
			.Select (_ =>
				{
					for (int Indx = 0; Indx < Input.touchCount; ++Indx) 
					{
						Touch touch = Input.touches [Indx];

						if (touch.phase == TouchPhase.Began && GameCore.IsTouchInterface (touch.position) == false) 
						{
							return touch.fingerId;
						}
					}
					return -1;
				}
			)
			.Subscribe (_ => {
				mUsedFingerID = _;
			});

		IObservable<Touch> cameraObservable = InputStream.Where (_ => mUsedFingerID != -1)
			.Select(_ =>
				{
					for (int Indx = 0; Indx < Input.touchCount; ++Indx) 
					{
						Touch touch = Input.touches [Indx];

						if (touch.fingerId == mUsedFingerID) 
						{
							return touch;
						}
					}

					return default(Touch);
				}
			).Publish ().RefCount ();

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
			.Subscribe (_ => ClearForSmartphone());
	}

	void ClearForSmartphone()
	{
		mUsedFingerID = -1;
	}
}
