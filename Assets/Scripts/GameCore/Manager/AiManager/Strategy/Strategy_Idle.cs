using UnityEngine;
using System.Collections;

public class Strategy_Idle : StrategyBase 
{
	float mIdleStay = 0f;

	// Use this for initialization
	public override void Exec (IAi _owner, System.Action _onCompleted = null) 
	{
		base.Exec (_owner, _onCompleted);

		mIdleStay = 2f;

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.AGENT_STOP, _owner.ActorID);
	}
	
	// Update is called once per frame
	public override bool Observe (IAi _owner) 
	{
		mIdleStay -= Time.deltaTime;

		if (mIdleStay <= 0f) 
		{
			return base.Observe (_owner);
		}

		return false;	
	}
}
