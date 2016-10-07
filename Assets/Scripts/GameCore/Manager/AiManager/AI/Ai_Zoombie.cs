using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ai_Zoombie : AiBase 
{
	public override void Think()
	{
		if (HasStrategy() == false) 
		{
			OnStrategy ();
		} 
		else
		{
			RunStrategy ();
		}
	}

	void OnStrategy()
	{
		int rand = Random.Range (0, 100);

		if (rand < 60) 
		{
			Idle ();
		} 
		else 
		{
			GotoPlayer ();
		}
	}

	void Idle()
	{
		AddStrategy ((IStrategy)GameCore.GetParameter(ParamGroup.GROUP_AI, AiParam.GET_STRATEGY_IDLE));
	}

	void GotoPlayer()
	{
		AddStrategy (
			(IStrategy)GameCore.GetParameter(ParamGroup.GROUP_AI, AiParam.GET_STRATEGY_HURT_PLAYER),
			() => {
				float distance = (float)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_DISTANCE_WITH_LOCK_ACTOR, ActorID);
				if(0f <= distance && distance <= 5f) FirePlayer();
			}
		);
	}

	void FirePlayer()
	{
		AddStrategy ((IStrategy)GameCore.GetParameter(ParamGroup.GROUP_AI, AiParam.GET_STRATEGY_FACE_LOOK_TARGET));
		AddStrategy ((IStrategy)GameCore.GetParameter(ParamGroup.GROUP_AI, AiParam.GET_STRATEGY_AIM));
	}
}
