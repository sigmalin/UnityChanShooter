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

	public virtual void OnDestroy()
	{
		ReleaseRequestQueue ();
	}

	protected void InitialRequestQueue()
	{
		mRequest = new ObservableQueue<InstructionSet> ();	

		mRequest.Initial (this.UpdateAsObservable (), _ => BatchRequest (_));
	}

	protected void ReleaseRequestQueue()
	{
		if (mRequest != null) 
		{
			mRequest.Clear ();

			mRequest = null;
		}
	}

	public void ExecCommand (uint _inst, params System.Object[] _params)
	{
		if(mRequest != null)
			mRequest.Enqueue (new InstructionSet (_inst, _params));
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
