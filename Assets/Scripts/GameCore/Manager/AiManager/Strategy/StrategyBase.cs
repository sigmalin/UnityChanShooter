using UnityEngine;
using System.Collections;

public class StrategyBase : IStrategy 
{
	uint mStrategyID;
	public uint StrategyID { get { return mStrategyID; } set { mStrategyID = value; } }

	System.Action mCompletedCallback;

	// Use this for initialization
	public virtual void Exec (IAi _owner, System.Action _onCompleted = null) 
	{
		mCompletedCallback = _onCompleted;
	}

	// Update is called once per frame
	public virtual bool Observe (IAi _owner) 
	{
		if (mCompletedCallback != null)
			mCompletedCallback ();

		mCompletedCallback = null;
		
		return true;
	}
}
