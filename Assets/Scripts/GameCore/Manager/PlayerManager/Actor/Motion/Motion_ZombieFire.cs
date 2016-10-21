using UnityEngine;
using System.Collections;

public class Motion_ZombieFire : IMotion 
{
	public float Weight { get { return 0f; } }

	public void UpdateWeight(float _varWeight)
	{
	}

	public void EnterMotion(PlayerActor _owner)
	{
		_owner.Actordata.Anim.SetTrigger (GameCore.AnimID_triggerFire);
	}

	public void LeaveMotion(PlayerActor _owner)
	{
	}

	public void UpdateMotion(PlayerActor _owner)
	{
	}

	public void AnimMoveMotion(PlayerActor _owner)
	{
		Movement (_owner);
	}

	public void AnimIKMotion(PlayerActor _owner)
	{
	}

	public void AnimEventMotion(PlayerActor _owner, string _event)
	{
		if (string.Equals ("Fire", _event) == true)
			GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.ARM_FIRE, _owner.ActorID, _owner.Launcher.RightArm);
	}

	void Movement(PlayerActor _owner)
	{
		_owner.Actordata.Rigid.velocity = new Vector3 (_owner.MotionData.Move.x * _owner.MotionData.Speed, _owner.Actordata.Rigid.velocity.y, _owner.MotionData.Move.z * _owner.MotionData.Speed);
	}
}