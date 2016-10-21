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
			.Where (_ => Input.touchCount != 0)
			.SelectMany (_ => 
				Input.touches.ToObservable ()
				.Where (_touch => GameCore.IsTouchInterface (_touch.position, JoyStickName))
				.First ()
			)
			.Subscribe (_ => {
				IsJoyStickUsed.Value = true;
				SetJoyStickPosition(_.position);
				mUsedFingerID = _.fingerId;
			});

		DisposeList [1] = _observable.Where (_ => IsJoyStickUsed.Value == true)
			.SelectMany (_ => 
				Input.touches.ToObservable ()
				.Where (_touch => _touch.fingerId == mUsedFingerID)
				.First ()
			)
			.Do (_ => {
			Vector2 dir = _.position - mJoyStickBoard.anchoredPosition;
			float dis = Mathf.Min (50f, dir.magnitude);
			dir = dir.normalized;

			Vec3JoyStickMoved.Value = new Vector3 (dir.x, 0f, dir.y);
			mJoyStickNob.anchoredPosition = JoyStickBoardPos + (dir * dis);
			})
			.Where (_ => _.phase == TouchPhase.Canceled || _.phase == TouchPhase.Ended)
			.Subscribe (_ => ClearForSmartphone ());
	}

	void ClearForSmartphone()
	{
		IsJoyStickUsed.Value = false;
		ResetJoyStickPosition();
		mUsedFingerID = -1;
	}
}
