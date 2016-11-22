using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public sealed partial class WeaponManager
{
	void AddFireDamage(uint _victimsID, uint _murdererID, uint _damage, Vector3 _hitPt)
	{
		WeaponActor victimsActor = GetWeaponActor (_victimsID);
		if (victimsActor != null && 
			(victimsActor.Flag.Value & Flags.INVINCIBLE) == Flags.NONE &&
			victimsActor.IsDead.Value == false) 
		{
			victimsActor.Murderer.Value = _murdererID;
			victimsActor.HitPt = _hitPt;
			victimsActor.HP.Value = victimsActor.HP.Value < _damage ? 0u : victimsActor.HP.Value - _damage;
		}

		DamageActor (_victimsID, _murdererID);
	}

	void DamageActor(uint _victimsID, uint _murdererID)
	{
		WeaponActor victimsActor = GetWeaponActor (_victimsID);
		if (victimsActor != null && 
			(victimsActor.Flag.Value & Flags.INVINCIBLE) == Flags.NONE &&
			victimsActor.IsDead.Value == false) 
		{
			WeaponActor murdererActor = GetWeaponActor (_murdererID);
			if (murdererActor == null)
				return;

			victimsActor.Stamina.Value -= murdererActor.Impact;

			murdererActor.Victims.OnNext(_victimsID);
		}
	}

	void HealActor(uint _actorID, uint _heal)
	{
		WeaponActor actor = GetWeaponActor (_actorID);
		if (actor != null && actor.IsDead.Value == false) 
		{
			uint life = actor.HP.Value + _heal;
			actor.HP.Value = life < actor.MaxHP ? life : actor.MaxHP;
		}
	}

	void ActorDead(uint _victimsID, uint _murdererID)
	{
		WeaponActor victimsActor = GetWeaponActor (_victimsID);
		if (victimsActor == null)
			return;

		WeaponActor murdererActor = GetWeaponActor (_murdererID);
		if (murdererActor == null)
			return;

		if (murdererActor.IsDead.Value == false) 
		{
			murdererActor.Victims.OnNext(_victimsID);
		}

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_DEAD, _victimsID, _murdererID, murdererActor.Impact, victimsActor.HitPt);
		GameCore.SendCommand (CommandGroup.GROUP_AI, AiInst.REMOVE_AI, _victimsID);

		if (_victimsID == MainActorID)
			GameCore.SendFlowEvent (FlowEvent.MAIN_ACTOR_DEAD);
		else
			GameCore.SendFlowEvent (FlowEvent.ACTOR_DEAD);

		if (GetActiveHostilityCount () == 0)
			GameCore.SendFlowEvent (FlowEvent.ALL_ENEMY_DEAD);

		RemoveActor (_victimsID);
	}
}
