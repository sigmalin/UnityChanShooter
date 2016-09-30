using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public sealed partial class WeaponManager
{
	void AddFireDamage(uint _victimsID, uint _murdererID, uint _damage)
	{
		WeaponActor victimsActor = GetWeaponActor (_victimsID);
		if (victimsActor == null)
			return;

		if (victimsActor.IsDead.Value == true)
			return;

		victimsActor.HP.Value = victimsActor.HP.Value < _damage ? 0u : victimsActor.HP.Value - _damage;
		victimsActor.MurdererID = _murdererID;
	}

	void ActorDead(uint _victimsID, uint _murdererID)
	{
		WeaponActor victimsActor = GetWeaponActor (_victimsID);
		if (victimsActor == null)
			return;

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_DEAD, _victimsID, _murdererID);
	}
}
