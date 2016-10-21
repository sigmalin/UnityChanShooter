using UnityEngine;
using System.Collections;
using UniRx;

public partial class Flow_GamePlay
{
	void GotoCloseUp()
	{
		GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.POP_MAIN_WEAPON_INTERFACE);

		SetCloseUpLastEnemyCameraMode ();

		Observable.Timer (System.TimeSpan.FromSeconds (1f))
			.Subscribe (_ => GotoVictory());
	}

	void GotoVictory()
	{
		SetVictoryUI ();
		SetVictoryCameraMode ();

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_SALUTE, MAIN_PLAYER_ID, true);
		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_FACE, MAIN_PLAYER_ID, ProtraitDefine.PROTRAIT_KEY_SMILE);
	}

	void GotoFailure()
	{
		GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.POP_MAIN_WEAPON_INTERFACE);

		SetFailureUI ();
	}
}
