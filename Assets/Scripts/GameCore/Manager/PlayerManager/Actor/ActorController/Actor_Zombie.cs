using UnityEngine;
using System.Collections;
using UniRx;

public sealed class Actor_Zombie : ActorController 
{
	IMotion mStandard;
	IMotion mFire;

	// Use this for initialization
	public override void Initial (PlayerActor _actor)
	{
		base.Initial (_actor);

		mStandard = new Motion_Zombie ();
		mFire = new Motion_ZombieFire ();

		SetMotion (mStandard);
	}

	public override void OnUpdate()
	{
		base.OnUpdate ();
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

		case PlayerInst.PLAYER_ROTATE:

			if (Owner.HasFlag (PlayerActor.Flags.STUN) == false) 
				Owner.transform.rotation = Quaternion.LookRotation ((Vector3)_params [0], Vector3.up);
			break;

		case PlayerInst.PLAYER_AIM:

			if (Owner.HasFlag (PlayerActor.Flags.STUN) == false) 
			{
				bool isAim = (bool)_params [0];

				if (isAim) SetMotion (mFire);
				else       SetMotion (mStandard);		
			}				
			break;
		}	
	}
}

