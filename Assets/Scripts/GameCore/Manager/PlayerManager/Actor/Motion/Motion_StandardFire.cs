using UnityEngine;
using System.Collections;

public sealed class Motion_StandardFire : IMotion 
{
	float mFireStay = 0f;
	float mFireRemain = 0f;

	float mWeightIK = 0f;
	public float Weight { get { return mWeightIK; } }

	public void UpdateWeight(float _varWeight)
	{
		mWeightIK = Mathf.Clamp01 (mWeightIK + _varWeight);
	}

	public void EnterMotion(PlayerActor _owner)
	{
		mFireStay = (float)GameCore.GetParameter(ParamGroup.GROUP_WEAPON, WeaponParam.WEAPON_ACTOR_FREQ, _owner.ActorID);
		mFireRemain = 0f;

		mWeightIK = 1f;
	}

	public void LeaveMotion(PlayerActor _owner)
	{
	}

	public void UpdateMotion(PlayerActor _owner)
	{
		float speed = _owner.Actordata.Anim.GetFloat(GameCore.AnimID_fSpeed);

		speed = Mathf.Lerp (speed, _owner.MotionData.Speed, Time.deltaTime * 5F);

		_owner.Actordata.Anim.SetFloat(GameCore.AnimID_fSpeed, speed);

		mFireRemain -= Time.deltaTime;
	}

	public void AnimMoveMotion(PlayerActor _owner)
	{
		Movement (_owner);

		_owner.Actordata.Anim.SetFloat (GameCore.AnimID_fJumpForce, _owner.Actordata.Rigid.velocity.y);
		_owner.Actordata.Anim.SetBool (GameCore.AnimID_isGroundID, _owner.Actordata.Col.IsGround());
	}

	public void AnimIKMotion(PlayerActor _owner)
	{
		_owner.Actordata.Anim.SetLookAtWeight (mWeightIK, 	// weight 
			0.3F,	// body
			0.4F,	// head
			0.2F,	// eye
			0.5F);	// clamp


		Vector3 aimAt = GetAimAt (_owner.MotionData);
		bool isAhead = _owner.transform.IsPointAhead (aimAt);
		bool isRightSide = _owner.transform.IsPointRightSide (aimAt);
		AvatarIKGoal goal = isRightSide ? AvatarIKGoal.RightHand : AvatarIKGoal.LeftHand;
		IArm arm = isRightSide ? _owner.Launcher.RightArm : _owner.Launcher.LeftArm;

		Vector3 lookat = isAhead ? aimAt : _owner.PlayerRole.BodyPt.Eye.position + ((isRightSide ? 1f : -1f) * _owner.transform.right);

		_owner.Actordata.Anim.SetIKPositionWeight (goal, mWeightIK);

		//_owner.Actordata.Anim.SetLookAtPosition (aimAt);
		_owner.Actordata.Anim.SetLookAtPosition (lookat);

		_owner.Actordata.Anim.SetIKPosition (goal, aimAt);

		if (mFireRemain <= 0f && 0.9f < mWeightIK) 
		{
			mFireRemain = mFireStay;
			GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.ARM_FIRE, _owner.ActorID, arm);
		}
	}

	public void AnimEventMotion(PlayerActor _owner, string _event)
	{
	}

	void Movement(PlayerActor _owner)
	{
		_owner.Actordata.Rigid.velocity = new Vector3 (_owner.MotionData.Move.x * _owner.MotionData.Speed, _owner.Actordata.Rigid.velocity.y, _owner.MotionData.Move.z * _owner.MotionData.Speed);
	}

	Vector3 GetAimAt(PlayerActor.PlayerMotionData _motionData)
	{
		PlayerActor actor = (PlayerActor)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_DATA, _motionData.LockActor);
		if (actor == null || actor.PlayerRole == null)
			return _motionData.CameraFocusOn;
		else
			return actor.PlayerRole.BodyPt.AimPt.position;
	}
}
