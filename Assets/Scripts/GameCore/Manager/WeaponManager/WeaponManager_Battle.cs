using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public sealed partial class WeaponManager
{
	void AddFireDamage(uint _victimsID, uint _murdererID, uint _damage, Vector3 _hitPt)
	{
		WeaponActor victimsActor = GetWeaponActor (_victimsID);
		if (victimsActor == null)
			return;

		if (victimsActor.IsDead.Value == true)
			return;

		victimsActor.MurdererID = _murdererID;
		victimsActor.HitPt = _hitPt;
		victimsActor.HP.Value = victimsActor.HP.Value < _damage ? 0u : victimsActor.HP.Value - _damage;
	}

	void ActorDead(uint _victimsID, uint _murdererID)
	{
		WeaponActor victimsActor = GetWeaponActor (_victimsID);
		if (victimsActor == null)
			return;

		WeaponActor murdererActor = GetWeaponActor (_murdererID);
		if (murdererActor == null)
			return;

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_DEAD, _victimsID, _murdererID, murdererActor.Impact, victimsActor.HitPt);
		GameCore.SendCommand (CommandGroup.GROUP_AI, AiInst.REMOVE_AI, _victimsID);

		if (_victimsID == MainActorID)
			GameCore.SendFlowEvent (FlowEvent.MAIN_ACTOR_DEAD);

		if (GetActiveHostilityCount () == 0)
			GameCore.SendFlowEvent (FlowEvent.ALL_ENEMY_DEAD);

		RemoveActor (_victimsID);
	}
}
