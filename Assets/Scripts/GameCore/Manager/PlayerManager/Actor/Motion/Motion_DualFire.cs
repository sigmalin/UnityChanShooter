using UnityEngine;
using System.Collections;

public class Motion_DualFire : IMotion 
{
	float mFireStay = 0f;
	float mFireRemain = 0f;

	Vector3 mMainTarget = Vector3.zero;
	Vector3 mSubTarget = Vector3.zero;
	Vector3 mLookAt = Vector3.zero;

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

		mMainTarget = GetAimPoint(_owner.MotionData.LockActor, _owner.MotionData.CameraFocusOn);
		mSubTarget = GetAimPoint(_owner.MotionData.SubLockActor, mMainTarget);
		mLookAt = (mMainTarget + mSubTarget) * 0.5f;
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

		_owner.Actordata.Anim.SetLookAtPosition (mLookAt);

		_owner.Actordata.Anim.SetIKPositionWeight (AvatarIKGoal.RightHand, mWeightIK);
		_owner.Actordata.Anim.SetIKPositionWeight (AvatarIKGoal.LeftHand, mWeightIK);

		bool isMainAtRightSide = _owner.transform.IsPointRightSide (mMainTarget);

		if (isMainAtRightSide == true) 
		{
			_owner.Actordata.Anim.SetIKPosition (AvatarIKGoal.RightHand, mMainTarget);
			_owner.Actordata.Anim.SetIKPosition (AvatarIKGoal.LeftHand, mSubTarget);
		} 
		else
		{
			_owner.Actordata.Anim.SetIKPosition (AvatarIKGoal.RightHand, mSubTarget);
			_owner.Actordata.Anim.SetIKPosition (AvatarIKGoal.LeftHand, mMainTarget);
		}

		if (mFireRemain <= 0f && 0.9f < mWeightIK) 
		{
			mFireRemain = mFireStay;
			GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.ARM_FIRE, _owner.ActorID, _owner.Launcher.RightArm);
			GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.ARM_FIRE, _owner.ActorID, _owner.Launcher.LeftArm);
		}
	}

	public void AnimEventMotion(PlayerActor _owner, string _event)
	{
	}

	void Movement(PlayerActor _owner)
	{
		Vector3 face = mLookAt - _owner.transform.position;
		face = new Vector3 (face.x, 0f, face.z);
		_owner.transform.rotation = Quaternion.LookRotation (face.normalized, Vector3.up);

		_owner.Actordata.Rigid.velocity = new Vector3 (_owner.MotionData.Move.x * _owner.MotionData.Speed, _owner.Actordata.Rigid.velocity.y, _owner.MotionData.Move.z * _owner.MotionData.Speed);
	}

	Vector3 GetAimPoint(uint _actorID, Vector3 _default)
	{
		PlayerActor actor = (PlayerActor)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_DATA, _actorID);
		if (actor == null || actor.PlayerRole == null)
			return _default;

		return actor.PlayerRole.BodyPt.AimPt.position;
	}
}
