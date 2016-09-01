using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class CommandBehaviour : MonoBehaviour, ICommand
{
	public struct InstructionSet
	{
		public uint Inst;
		public System.Object[] Parameters;

		public InstructionSet(uint _inst, System.Object[] _params)
		{
			Inst = _inst;

			Parameters = _params;
		}
	}

	ObservableQueue<InstructionSet> mRequest = null;

	Subject<float> mUpdateSubject = null;
	protected IObservable<float> UpdateObservable { get { return mUpdateSubject == null ? null : mUpdateSubject.AsObservable(); } }

	Subject<float> mLateUpdateSubject = null;
	protected IObservable<float> LateUpdateObservable { get { return mLateUpdateSubject == null ? null : mLateUpdateSubject.AsObservable(); } }

	public virtual void OnDestroy()
	{
		ReleaseRequestQueue ();
	}

	protected void InitialRequestQueue()
	{
		ReleaseRequestQueue ();

		mRequest = new ObservableQueue<InstructionSet> ();	

		mUpdateSubject = new Subject<float> ();

		mLateUpdateSubject = new Subject<float> ();

		mRequest.Initial (UpdateObservable, _ => BatchRequest (_));
	}

	protected void ReleaseRequestQueue()
	{
		if (mRequest != null) 
		{
			mRequest.Release ();

			mRequest = null;
		}

		if (mUpdateSubject != null) 
		{
			mUpdateSubject.Dispose ();

			mUpdateSubject = null;
		}

		if (mLateUpdateSubject != null) 
		{
			mLateUpdateSubject.Dispose ();

			mLateUpdateSubject = null;
		}
	}

	public void ExecCommand (uint _inst, params System.Object[] _params)
	{
		if(mRequest != null)
			mRequest.Enqueue (new InstructionSet (_inst, _params));
	}

	public void BatchUpdate (float _delta)
	{
		if (mUpdateSubject != null)
			mUpdateSubject.OnNext (_delta);
	}

	public void BatchLateUpdate (float _delta)
	{
		if (mLateUpdateSubject != null)
			mLateUpdateSubject.OnNext (_delta);
	}

	protected void BatchRequest(RequestQueue<InstructionSet> _request)
	{
		while(_request.IsEmpty () == false)
		{
			InstructionSet request = _request.Dequeue();
			
			BatchCommand(request.Inst, request.Parameters);
		}
	}

	protected virtual void BatchCommand (uint _inst, params System.Object[] _params)
	{
	}
}
