using UnityEngine;
using System.Collections;

public partial class PlayerManager
{
	uint GetLockActorID(uint _actorID)
	{
		PlayerActor actor = GetPlayerData (_actorID);
		return actor == null ? 0u : actor.MotionData.LockActor;
	}

	float CalcDistanceBetweenLockActor(uint _actorID)
	{
		PlayerActor actor = GetPlayerData (_actorID);
		if (actor == null)
			return -1f;

		PlayerActor target = GetPlayerData (actor.MotionData.LockActor);
		if (target == null)
			return -1f;

		return Vector3.Distance (actor.transform.position, target.transform.position);
	}
}
