using UnityEngine;
using System.Collections;
using UniRx;

public partial class GameCore 
{
	IFlow mFlow = null;

	Subject<uint> mFlowSubject = null;
	protected IObservable<uint> FlowObservable { get { return mFlowSubject == null ? null : mFlowSubject.AsObservable(); } }

	void InitialFlow()
	{
		ReleaseFlow ();

		mFlowSubject = new Subject<uint> ();

		FlowObservable
			.Where (_ => mFlow != null)
			.Subscribe (_ => mFlow.Event(_));
	}

	void ReleaseFlow()
	{
		mFlow = null;

		if (mFlowSubject != null) 
		{
			mFlowSubject.Dispose ();

			mFlowSubject = null;
		}
	}

	static public void SetFlow(IFlow _flow)
	{
		if (Instance == null)
			return;

		if (Instance.mFlow != null)
			Instance.mFlow.Exit ();

		Instance.mFlow = _flow;

		if (Instance.mFlow != null)
			Instance.mFlow.Enter ();
	}

	static public void SendFlowEvent(uint _eventID)
	{
		if (Instance == null || Instance.mFlowSubject == null)
			return;

		Instance.mFlowSubject.OnNext (_eventID);
	}
}
