using UnityEngine;
using System.Collections;

public class Strategy_FaceLockTarget : StrategyBase
{
	// Use this for initialization
	public override void Exec (IAi _owner, System.Action _onCompleted = null) 
	{
		base.Exec (_owner, _onCompleted);

		PlayerActor mineActor = (PlayerActor)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_DATA, _owner.ActorID);
		if (mineActor == null)
			return;

		PlayerActor targetActor = (PlayerActor)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_DATA, mineActor.MotionData.LockActor);
		if (mineActor == null)
			return;

		Vector3 dir = targetActor.transform.position - mineActor.transform.position;
		dir = new Vector3 (dir.x, 0f, dir.z);
		dir = dir.normalized;
		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_ROTATE, _owner.ActorID, dir);
	}
}
