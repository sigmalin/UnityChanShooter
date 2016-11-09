using UnityEngine;
using System.Collections;

public class Strategy_Freeze : StrategyBase 
{
	// Use this for initialization
	public override void Exec (IAi _owner, System.Action _onCompleted = null) 
	{
		base.Exec (_owner, _onCompleted);

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.AGENT_STOP, _owner.ActorID);
	}

	// Update is called once per frame
	public override bool Observe (IAi _owner) 
	{
		return false;	
	}
}
