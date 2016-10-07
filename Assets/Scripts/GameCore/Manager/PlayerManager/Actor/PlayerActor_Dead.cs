using UnityEngine;
using System.Collections;

public partial class PlayerActor
{
	void PlayerDead()
	{
		SetRagdoll ();

		ClearWeaponModel ();

		ClearRoleModel ();

		SetActorController (null);

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.REMOVE_PLAYER, ActorID);
	}

	void SetRagdoll()
	{
		GameObject ragdollGO = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.RAGDOLL_MODEL, ModelID);
		if (ragdollGO == null)
			return;

		Ragdoll ragdoll = ragdollGO.GetComponent<Ragdoll> ();
		if (ragdoll == null)
			return;

		ragdoll.CopyPose (PlayerRole);
	}
}
