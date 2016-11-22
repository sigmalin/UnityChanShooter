using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class GameCamera 
{
	Stack<IMode> mModeStack;

	void InitialMode()
	{
		mModeStack = new Stack<IMode> ();
	}

	void ReleaseMode()
	{
		ClearStack ();

		mModeStack = null;
	}

	void PushCameraMode (IMode _mode)
	{
		if (_mode == null)
			return;

		if (mModeStack.Contains (_mode) == true)
			return;

		if (GetCurrentMode() != null)
			GetCurrentMode().LeaveMode (this);

		mModeStack.Push (_mode);

		_mode.EnterMode (this);
	}

	void PopCameraMode(IMode _mode)
	{
		if (GetCurrentMode () == _mode) 
		{
			mModeStack.Pop ();
			_mode.LeaveMode (this);

			if (GetCurrentMode () != null)
				GetCurrentMode ().EnterMode (this);
		} 
		else 
		{
			Stack<IMode> tempStack = new Stack<IMode> ();

			while (mModeStack.Count != 0) 
			{
				IMode mode = mModeStack.Pop ();

				if (_mode != mode)
					tempStack.Push (mode);
			}

			while (tempStack.Count != 0)
			{
				mModeStack.Push (tempStack.Pop());
			}
		}
	}

	IMode GetCurrentMode()
	{
		if (mModeStack == null)
			return null;

		if (mModeStack.Count == 0)
			return null;

		return mModeStack.Peek ();
	}

	public void ClearStack()
	{
		if (GetCurrentMode () != null) 
		{
			GetCurrentMode ().LeaveMode (this);
			mModeStack.Clear ();
		}
	}
}
