using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class GameCore
{
	Queue<System.Action> mQueueGL;

	void InitialGL()
	{
		ReleaseGL ();

		mQueueGL = new Queue<System.Action> ();
	}

	void ReleaseGL()
	{
		if (mQueueGL != null) 
		{
			mQueueGL.Clear ();

			mQueueGL = null;
		}
	}

	void ClearGL()
	{
		if (mQueueGL != null) 
			mQueueGL.Clear ();
	}

	void ExecGL()
	{
		if (mQueueGL == null)
			return;
		
		while (mQueueGL.Count != 0) 
		{
			System.Action oper = mQueueGL.Dequeue ();
			if (oper != null)
				oper ();
		}
	}

	void EnqueueGL(System.Action _execGL)
	{
		if (mQueueGL == null)
			return;

		mQueueGL.Enqueue (_execGL);
	}

	static public void AddGL(System.Action _execGL)
	{
		if (Instance == null || _execGL == null)
			return;

		Instance.EnqueueGL (_execGL);
	}
}
