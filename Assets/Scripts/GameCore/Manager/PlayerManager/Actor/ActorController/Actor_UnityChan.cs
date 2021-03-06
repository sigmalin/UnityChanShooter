﻿using UnityEngine;
using System.Collections;
using UniRx;

public sealed class Actor_UnityChan : ActorController 
{
	float mNextJumpWait = 0F;

	IMotion mStandard;
	IMotion mFire;
	IMotion mDualFire;

	// Use this for initialization
	public override void Initial (PlayerActor _actor)
	{
		base.Initial (_actor);

		mNextJumpWait = 0F;

		mStandard = new Motion_Standard ();
		mFire = new Motion_StandardFire ();
		mDualFire = new Motion_DualFire ();

		SetMotion (mStandard);
		Owner.Actordata.Anim.SetBool (GameCore.AnimID_isSaluteID, false);
	}

	public override void OnUpdate()
	{
		base.OnUpdate ();

		if (IsGround () == true) 
		{
			if (0F < mNextJumpWait)
				mNextJumpWait -= Time.deltaTime;
		}
		else
			mNextJumpWait = 1F;
	}

	// Update is called once per frame
	public override void ExecCommand (uint _inst, params System.Object[] _params) 
	{
		switch(_inst)
		{
		case PlayerInst.PLAYER_IDLE:

			Owner.MotionData.Speed = 0F;
			Owner.Actordata.Anim.SetBool (GameCore.AnimID_isMoveID, false);
			break;

		case PlayerInst.PLAYER_MOVE:

			if (Owner.HasFlag (PlayerActor.Flags.STUN) == false) 
			{
				Owner.MotionData.Move = (Vector3)_params [0];
				Owner.MotionData.Speed = (float)_params [1];
				Owner.Actordata.Anim.SetBool (GameCore.AnimID_isMoveID, true);
			}
			break;

		case PlayerInst.PLAYER_JUMP:

			if (IsGround () == true && mNextJumpWait <= 0F && Owner.HasFlag (PlayerActor.Flags.STUN) == false) 
				Owner.Actordata.Rigid.velocity = new Vector3 (Owner.Actordata.Rigid.velocity.x, 5F, Owner.Actordata.Rigid.velocity.z);			
			break;

		case PlayerInst.PLAYER_SALUTE:

			Owner.Actordata.Anim.SetBool (GameCore.AnimID_isSaluteID, (bool)_params [0]);
			break;

		case PlayerInst.PLAYER_FACE:
			
			Owner.Actordata.Anim.SetInteger (GameCore.AnimID_iFaceID, (int)_params [0]);
			break;

		case PlayerInst.PLAYER_ROTATE:

			if (Owner.HasFlag (PlayerActor.Flags.STUN) == false) 
				Owner.transform.rotation = Quaternion.LookRotation ((Vector3)_params [0], Vector3.up);
			break;

		case PlayerInst.PLAYER_AIM:

			if (Owner.HasFlag (PlayerActor.Flags.STUN) == false) 
			{
				bool isAim = (bool)_params [0];

				if (isAim) SetMotion (Owner.HasFlag(PlayerActor.Flags.FORM_CHANGE) ? mDualFire : mFire);
				else       SetMotion (mStandard);	
			}					
			break;
		}	
	}

	bool IsGround ()
	{
		return Owner.Actordata.Anim.GetBool (GameCore.AnimID_isGroundID);
	}
}
