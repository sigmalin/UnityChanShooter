using UnityEngine;
using System.Collections;
using System.Linq;

public class Strategy_HurtMainPlayer : StrategyBase 
{
	// Use this for initialization
	public override void Exec (IAi _owner, System.Action _onCompleted = null) 
	{
		base.Exec (_owner, _onCompleted);

		PlayerActor mineActor = (PlayerActor)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_DATA, _owner.ActorID);

		if (mineActor == null)
		{
			GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.AGENT_STOP, _owner.ActorID);
			return;
		}

		uint[] enemyIDs = (uint[])GameCore.GetParameter (ParamGroup.GROUP_WEAPON, WeaponParam.GET_HOSTILITY_LIST, _owner.ActorID);

		if (enemyIDs == null || enemyIDs.Length == 0) 
		{
			GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.AGENT_STOP, _owner.ActorID);
			return;
		}

		Vector3 minePos = mineActor.transform.position;
		Vector3 mineEye = mineActor.PlayerRole.BodyPt.Eye.position;

		PlayerActor targetActor = enemyIDs.Select (_ => (PlayerActor)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_DATA, _))
			.Where (_ => _ != null)
			.Select (_ => new { Actor = _, Distance = Vector3.Distance (minePos, _.transform.position) })
			.OrderBy (_ => _.Distance)
			.Select (_ => _.Actor)
			.FirstOrDefault ();

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_LOCK, _owner.ActorID, targetActor == null ? 0u : targetActor.ActorID); 

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.AGENT_GOTO, _owner.ActorID, targetActor == null ? minePos : targetActor.transform.position, 0.5f, 1f);
	}

	// Update is called once per frame
	public override bool Observe (IAi _owner) 
	{
		PlayerActor mineActor = (PlayerActor)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_DATA, _owner.ActorID);
		if (mineActor == null)
			return true;

		if (mineActor.Actordata.Agent == null)
			return true;

		Vector3 move = mineActor.Actordata.Agent.nextPosition - mineActor.transform.position;

		move = new Vector3 (move.x, 0f, move.z);

		Vector3 dir = move.normalized;

		float speed = move.magnitude;

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_MOVE, _owner.ActorID, dir, speed);
		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_ROTATE, _owner.ActorID, dir);

		if (mineActor.Actordata.Agent.IsArrived () == true || IsCloseTarget(_owner) == true) 
		{
			GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.AGENT_STOP, _owner.ActorID);

			return base.Observe (_owner);
		}

		return false;
	}

	bool IsCloseTarget(IAi _owner)
	{
		float distance = (float)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_DISTANCE_WITH_LOCK_ACTOR, _owner.ActorID);

		return distance < 5f;
	}
}
