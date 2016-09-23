using UnityEngine;
using System.Collections;

public class ActorController
{
	PlayerActor mRefActor = null;
	protected PlayerActor Owner { get { return mRefActor; } }

	IMotion mCurMotion = null;

	// Use this for initialization
	public virtual void Initial (PlayerActor _actor)
	{
		mRefActor = _actor;
	}

	// Update is called once per frame
	public virtual void ExecCommand (uint _inst, params System.Object[] _params) 
	{
	}

	public virtual void OnUpdate()
	{
		if (mCurMotion != null)
			mCurMotion.UpdateMotion (mRefActor);
	}

	public void OnAnimMove()
	{
		if (mCurMotion != null)
			mCurMotion.AnimMoveMotion (mRefActor);
	}

	public void OnAnimIK()
	{
		if (mCurMotion != null)
			mCurMotion.AnimIKMotion (mRefActor);
	}

	protected void SetMotion(IMotion _motion)
	{
		if (mCurMotion == _motion)
			return;

		if (mCurMotion != null)
			mCurMotion.LeaveMotion (mRefActor);

		mCurMotion = _motion;

		if (mCurMotion != null)
			mCurMotion.EnterMotion (mRefActor);
	}

	public virtual void Clear()
	{
		mRefActor = null;
	}
}
