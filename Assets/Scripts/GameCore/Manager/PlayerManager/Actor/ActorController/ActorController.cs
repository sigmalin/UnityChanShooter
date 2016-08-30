using UnityEngine;
using System.Collections;

public class ActorController
{
	Actor mActor = null;
	protected Actor Owner { get { return mActor; } }

	IMotion mCurMotion = null;

	// Use this for initialization
	public virtual void Initial (Actor _actor)
	{
		mActor = _actor;
	}

	// Update is called once per frame
	public virtual void ExecCommand (uint _inst, params System.Object[] _params) 
	{
	}

	public void OnUpdate()
	{
		if (mCurMotion != null && mActor.PlayerData != null)
			mCurMotion.UpdateMotion (mActor.PlayerData);
	}

	public void OnAnimMove()
	{
		if (mCurMotion != null && mActor.PlayerData != null)
			mCurMotion.AnimMoveMotion (mActor.PlayerData);
	}

	public void OnAnimIK()
	{
		if (mCurMotion != null && mActor.PlayerData != null)
			mCurMotion.AnimIKMotion (mActor.PlayerData);
	}

	protected void SetMotion(IMotion _motion)
	{
		if (mCurMotion == _motion)
			return;

		if (mCurMotion != null)
			mCurMotion.LeaveMotion (mActor.PlayerData);

		mCurMotion = _motion;

		if (mCurMotion != null)
			mCurMotion.EnterMotion (mActor.PlayerData);
	}

	public virtual void Clear()
	{
		mActor = null;
	}
}
