using UnityEngine;
using System.Collections;
using UniRx;

public partial class JoyStick
{
	void OperatorForStandalone(IObservable<Unit> _observable)
	{
		DisposeList = new System.IDisposable[2];

		DisposeList[0] = _observable.Where(_ => IsJoyStickUsed.Value == false)
			.Where (_ => Input.GetMouseButtonDown (0) == true)
			.Where (_ => GameCore.IsTouchInterface (Input.mousePosition, JoyStickName))
			.Subscribe (_ => {
				IsJoyStickUsed.Value = true;
				SetJoyStickPosition(Input.mousePosition);
			});

		DisposeList [1] = _observable.Where (_ => IsJoyStickUsed.Value == true)
			.Do (_ => {
			Vector2 dir = new Vector2 (Input.mousePosition.x, Input.mousePosition.y) - mJoyStickBoard.anchoredPosition;
			float dis = Mathf.Min (50f, dir.magnitude);
			dir = dir.normalized;

			Vec3JoyStickMoved.Value = new Vector3 (dir.x, 0f, dir.y);
			mJoyStickNob.anchoredPosition = JoyStickBoardPos + (dir * dis);
			})
			.Where (_ => Input.GetMouseButtonUp (0) == true)
			.Subscribe (_ => ClearForStandalone ());
	}

	void ClearForStandalone()
	{
		IsJoyStickUsed.Value = false;
		ResetJoyStickPosition();
	}
}
