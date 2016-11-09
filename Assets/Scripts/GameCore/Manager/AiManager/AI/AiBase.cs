using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AiBase : IAi 
{
	Queue<IStrategy> mStrategyQueue;

	uint mActorID;
	public uint ActorID { get { return mActorID; } }

	public virtual void Initial (uint _actorID)
	{
		Release ();

		mActorID = _actorID;

		if (mStrategyQueue == null)
			mStrategyQueue = new Queue<IStrategy> ();
	}

	public virtual void Release ()
	{
		mActorID = 0;

		if (mStrategyQueue != null) 
		{
			while (HasStrategy() == true)
				RecycleStrategy (mStrategyQueue.Dequeue ());

			mStrategyQueue = null;
		}
	}

	public virtual void Think()
	{
	}

	public void Freeze(bool _isEnable)
	{
		if (mStrategyQueue != null) 
		{
			while (HasStrategy() == true)
				RecycleStrategy (mStrategyQueue.Dequeue ());
		}

		if (_isEnable == true) 
		{
			AddStrategy ((IStrategy)GameCore.GetParameter(ParamGroup.GROUP_AI, AiParam.GET_STRATEGY_FREEZE));
		}
	}

	protected void AddStrategy (IStrategy _strategy, System.Action _onCompleted = null)
	{
		if (_strategy == null)
			return;

		_strategy.Exec (this, _onCompleted);

		mStrategyQueue.Enqueue (_strategy);
	}

	protected bool HasStrategy()
	{
		if (mStrategyQueue == null)
			return false;
		
		return mStrategyQueue.Count != 0;
	}

	protected void RunStrategy()
	{
		if (HasStrategy () == false)
			return;

		if (mStrategyQueue.Peek ().Observe (this) == true)
			RecycleStrategy (mStrategyQueue.Dequeue ());
	}

	void RecycleStrategy(IStrategy _strategy)
	{
		if (_strategy == null)
			return;

		GameCore.SendCommand (CommandGroup.GROUP_AI, AiInst.RECYCLE_STRATEGY, _strategy);
	}
}
