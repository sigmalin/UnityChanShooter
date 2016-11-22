using UnityEngine;
using System.Collections;
using UniRx;

public partial class JoyStick
{
	int mUsedFingerID = -1;
	
	void OperatorForSmartphone(IObservable<Unit> _observable)
	{
		DisposeList = new System.IDisposable[2];

		DisposeList [0] = _observable.Where (_ => IsJoyStickUsed.Value == false)
			.Where (_ => 
				{
					for (int Indx = 0; Indx < Input.touchCount; ++Indx) 
					{
						Touch touch = Input.touches [Indx];

						if (touch.phase == TouchPhase.Began && GameCore.IsTouchInterface (touch.position, JoyStickName)) 
						{
							SetJoyStickPosition (touch.position);
							mUsedFingerID = touch.fingerId;
							return true;
						}
					}
					return false;
				}
			)
			.Subscribe (_ => IsJoyStickUsed.Value = true);

		DisposeList [1] = _observable.Where (_ => IsJoyStickUsed.Value == true)
			.Select (_ =>
				{
					for (int Indx = 0; Indx < Input.touchCount; ++Indx) 
					{
						Touch touch = Input.touches [Indx];

						if (touch.fingerId == mUsedFingerID) 
						{
							Vector2 dir = touch.position - mJoyStickBoard.anchoredPosition;
							float dis = Mathf.Min (50f, dir.magnitude);
							dir = dir.normalized;

							Vec3JoyStickMoved.Value = new Vector3 (dir.x, 0f, dir.y);
							mJoyStickNob.anchoredPosition = JoyStickBoardPos + (dir * dis);

							return touch.phase;
						}
					}

					return TouchPhase.Canceled;
				}
			)
			.Where (_ => _ == TouchPhase.Canceled || _ == TouchPhase.Ended)
			.Subscribe (_ => ClearForSmartphone ());
	}

	void ClearForSmartphone()
	{
		IsJoyStickUsed.Value = false;
		ResetJoyStickPosition();
		mUsedFingerID = -1;
	}
}
