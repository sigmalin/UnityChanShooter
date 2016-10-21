using UnityEngine;
using System.Collections;

public class ActorController
{
	PlayerActor mRefActor = null;
	protected PlayerActor Owner { get { return mRefActor; } }

	IMotion mCurMotion = null;
	IMotion mNextMotion = null;

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
		UpdateMotionWeight ();

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

	public void OnAnimEvent(string _event)
	{
		if (mCurMotion != null)
			mCurMotion.AnimEventMotion (mRefActor, _event);
	}

	protected void UpdateMotionWeight()
	{
		if (mCurMotion == null) 
		{
			mCurMotion = mNextMotion;
		} 
		else 
		{
			if (mNextMotion != null)
			{
				if (mCurMotion.Weight == 0f) 
				{
					mCurMotion.LeaveMotion (mRefActor);
					mCurMotion = mNextMotion;
					mNextMotion = null;
					mCurMotion.EnterMotion (mRefActor);
				} 
				else 
				{
					mCurMotion.UpdateWeight (Time.deltaTime * -5f);
				}
			} 
			else 
			{
				mCurMotion.UpdateWeight (Time.deltaTime * 5f);
			}
		}
	}

	protected void SetMotion(IMotion _motion)
	{
		if (mCurMotion == _motion || mNextMotion == _motion)
			return;

		mNextMotion = _motion;
	}

	public virtual void Clear()
	{
		mRefActor = null;
	}
}
