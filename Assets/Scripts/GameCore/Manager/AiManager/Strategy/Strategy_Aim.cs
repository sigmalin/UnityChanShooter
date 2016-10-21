using UnityEngine;
using System.Collections;

public class Strategy_Aim : StrategyBase
{
	float mAimStay = 0f;

	// Use this for initialization
	public override void Exec (IAi _owner, System.Action _onCompleted = null) 
	{
		base.Exec (_owner, _onCompleted);

		mAimStay = 3f;
	}

	// Update is called once per frame
	public override bool Observe (IAi _owner) 
	{
		mAimStay -= Time.deltaTime;

		if (0f < mAimStay) 
		{
			GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_AIM, _owner.ActorID, true);
		} 
		else 
		{
			GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_AIM, _owner.ActorID, false);
			return base.Observe (_owner);
		}

		return false;
	}
}