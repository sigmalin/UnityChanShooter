using UnityEngine;
using System.Collections;

public sealed class Motion_Standard : IMotion 
{
	public float Weight { get { return 0f; } }

	public void UpdateWeight(float _varWeight)
	{
	}

	public void EnterMotion(PlayerActor _owner)
	{
	}

	public void LeaveMotion(PlayerActor _owner)
	{
	}

	public void UpdateMotion(PlayerActor _owner)
	{
		float speed = _owner.Actordata.Anim.GetFloat(GameCore.AnimID_fSpeed);

		speed = Mathf.Lerp (speed, _owner.MotionData.Speed, Time.deltaTime * 5F);

		_owner.Actordata.Anim.SetFloat(GameCore.AnimID_fSpeed, speed);
	}

	public void AnimMoveMotion(PlayerActor _owner)
	{
		Movement (_owner);

		_owner.Actordata.Anim.SetFloat (GameCore.AnimID_fJumpForce, _owner.Actordata.Rigid.velocity.y);
		_owner.Actordata.Anim.SetBool (GameCore.AnimID_isGroundID, _owner.Actordata.Col.IsGround());
	}

	public void AnimIKMotion(PlayerActor _owner)
	{
		/*
		_owner.Actordata.Anim.SetLookAtWeight ( 1F, 	// weight 
												0.3F,	// body
												0.4F,	// head
												0.2F,	// eye
												0.5F);	// clamp

		_owner.Actordata.Anim.SetLookAtPosition (_owner.MotionData.LookAt);
		*/
	}

	public void AnimEventMotion(PlayerActor _owner, string _event)
	{
	}

	void Movement(PlayerActor _owner)
	{
		_owner.Actordata.Rigid.velocity = new Vector3 (_owner.MotionData.Move.x * _owner.MotionData.Speed, _owner.Actordata.Rigid.velocity.y, _owner.MotionData.Move.z * _owner.MotionData.Speed);
	}
}
