using UnityEngine;
using System.Collections;

public partial class PlayerActor
{
	void PlayerDead(uint _murdererID, float _impact, Vector3 _hitPt)
	{
		SetRagdoll (_impact, _hitPt);

		ClearActorController ();

		ClearWeaponModel ();

		ClearRoleModel ();

		StopNavMeshAgent ();

		ResetMotionData ();

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.REMOVE_PLAYER, ActorID);
	}

	void SetRagdoll(float _impact, Vector3 _hitPt)
	{
		GameObject ragdollGO = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.RAGDOLL_MODEL, ModelID);
		if (ragdollGO == null)
			return;

		Ragdoll ragdoll = ragdollGO.GetComponent<Ragdoll> ();
		if (ragdoll == null)
			return;

		ragdoll.CopyPose (PlayerRole);
		ragdoll.AddImpact (_impact, _hitPt);
	}
}
