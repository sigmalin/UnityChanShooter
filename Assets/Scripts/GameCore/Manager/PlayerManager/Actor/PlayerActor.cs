using UnityEngine;
using System.Collections;
using UniRx;

public partial class PlayerActor : MonoBehaviour, ITarget 
{
	uint mActorID;
	public uint ActorID { get { return mActorID; } }

	// Update is called once per frame
	public void FrameMove () 
	{
		if (Controller == null)
			return;
		
		Controller.OnUpdate ();
	}

	void OnDestroy()
	{
		ReleaseAll ();
	}

	public void ExecCommand(uint _inst, params System.Object[] _params)
	{
		switch(_inst)
		{
		case PlayerInst.CREATE_PLAYER:
			mActorID = (uint)_params [0];
			InitialFlag();
			break;

		case PlayerInst.SET_MODEL:
			SetRoleModel((uint)_params [0]);
			break;

		case PlayerInst.SET_WEAPON:
			SetWeaponModel((uint)_params [0]);
			break;

		case PlayerInst.REMOVE_PLAYER:
			ReleaseAll ();
			break;

		case PlayerInst.PUSH_ACTOR_CONTROLLER:
			PushActorController ((ActorController)_params[0]);
			break;

		case PlayerInst.POP_ACTOR_CONTROLLER:
			PopActorController ((ActorController)_params[0]);
			break;

		case PlayerInst.SET_POSITION:
			this.transform.position = (Vector3)_params [0];
			break;

		case PlayerInst.SET_DIRECTION:

			this.transform.rotation = (Quaternion)_params [0];
			break;

		case PlayerInst.SET_LAYER:
			this.gameObject.layer = (int)_params [0];
			break;

		case PlayerInst.PLAYER_DEAD:

			PlayerDead ((uint)_params [0], (float)_params [1], (Vector3)_params [2]);
			break;

		case PlayerInst.CAMERA_FOCUS:

			mMotionData.CameraFocusOn = (Vector3)_params [0];
			break;

		case PlayerInst.PLAYER_LOCK:

			mMotionData.LockActor = (uint)_params [0];
			break;

		case PlayerInst.AGENT_GOTO:

			InitialNavMeshAgent ();

			if (Actordata.Agent != null) 
			{
				Actordata.Agent.SetDestination ((Vector3)_params [0]);
				Actordata.Agent.speed = (float)_params [1];
				Actordata.Agent.stoppingDistance = (float)_params [2];
				Actordata.Agent.Resume ();
			}
			break;

		case PlayerInst.AGENT_STOP:

			InitialNavMeshAgent ();

			if (Actordata.Agent != null) 
			{
				Actordata.Agent.Stop ();
				Actordata.Agent.speed = 0f;

				Actordata.Agent.ResetPath ();
			}

			if (Controller != null) 
				Controller.ExecCommand (PlayerInst.PLAYER_IDLE);
			break;

		case PlayerInst.PLAYER_STUN:
			SetStun ((bool)_params [0]);
			break;

		case PlayerInst.PLAYER_FORM_CHANGE:
			SetFormChange ((bool)_params [0]);
			break;

		case PlayerInst.PLAYER_DAMAGE:
			if (Actordata.Anim != null) 
			{
				Actordata.Anim.ResetTrigger(GameCore.AnimID_triggerShock);
				Actordata.Anim.SetTrigger(GameCore.AnimID_triggerShock);
			}
			break;

		default:
			if (Controller != null) 
				Controller.ExecCommand (_inst, _params);
			break;
		}
	}
}
