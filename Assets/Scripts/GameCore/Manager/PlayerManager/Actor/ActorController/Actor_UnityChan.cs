using UnityEngine;
using System.Collections;

public sealed class Actor_UnityChan : ActorController 
{
	// Use this for initialization
	public override void Initial (Actor _actor)
	{
		base.Initial (_actor);
	}
	
	// Update is called once per frame
	public override void ExecCommand (uint _inst, params System.Object[] _params) 
	{
		switch(_inst)
		{
		case PlayerInst.GAME_START:

			SetMotion (new Motion_Standard());
			break;

		case PlayerInst.PLAYER_IDLE:
			
			Owner.PlayerData.Speed = 0F;
			Owner.Actordata.Anim.SetBool (GameCore.AnimID_isMoveID, false);
			break;

		case PlayerInst.PLAYER_MOVE:

			Owner.PlayerData.Move = (Vector3)_params [0];
			Owner.PlayerData.Speed = 1F;
			Owner.Actordata.Anim.SetBool (GameCore.AnimID_isMoveID, true);
			break;

		case PlayerInst.PLAYER_JUMP:

			if (IsGround () == true) 
				Owner.Actordata.Rigid.velocity = new Vector3 (Owner.Actordata.Rigid.velocity.x, 5F, Owner.Actordata.Rigid.velocity.z);			
			break;

		case PlayerInst.PLAYER_ROTATE:

			Owner.Actordata.Root.transform.rotation = Quaternion.LookRotation ((Vector3)_params [0], Vector3.up);
			break;

		case PlayerInst.PLAYER_AIM:

			bool isAim = (bool)_params [0];

			if (isAim) SetMotion (new Motion_StandardFire ());
			else       SetMotion (new Motion_Standard());			
			break;

		case PlayerInst.PLAYER_LOOKAT:

			Owner.PlayerData.LookAt = (Vector3)_params [0];
			break;
		}	
	}

	bool IsGround ()
	{
		return Owner.Actordata.Anim.GetBool (GameCore.AnimID_isGroundID);
	}
}
