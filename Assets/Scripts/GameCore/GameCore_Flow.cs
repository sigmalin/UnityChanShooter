using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public partial class GameCore 
{
	IFlow mFlow = null;

	Queue<IFlow> mFlowQueue = null;

	Subject<uint> mFlowSubject = null;
	protected IObservable<uint> FlowObservable { get { return mFlowSubject == null ? null : mFlowSubject.AsObservable(); } }

	void InitialFlow()
	{
		ReleaseFlow ();

		mFlowQueue = new Queue<IFlow> ();

		mFlowSubject = new Subject<uint> ();

		FlowObservable
			.Where (_ => mFlow != null)
			.Subscribe (_ => mFlow.Event(_));
	}

	void ReleaseFlow()
	{
		mFlow = null;

		if (mFlowQueue != null) 
		{
			mFlowQueue.Clear ();

			mFlowQueue = null;
		}

		if (mFlowSubject != null) 
		{
			mFlowSubject.Dispose ();

			mFlowSubject = null;
		}
	}

	void LateUpdateFlow()
	{
		if (mFlowQueue == null)
			return;

		if (mFlowQueue.Count == 0)
			return;

		if (mFlow != null)
			mFlow.Exit ();

		mFlow = mFlowQueue.Dequeue();

		if (mFlow != null)
			mFlow.Enter ();
	}

	static public void SetNextFlow(IFlow _flow)
	{
		if (Instance == null)
			return;

		Instance.mFlowQueue.Enqueue(_flow);
	}

	static public void SendFlowEvent(uint _eventID)
	{
		if (Instance == null || Instance.mFlowSubject == null)
			return;

		Instance.mFlowSubject.OnNext (_eventID);
	}
}
