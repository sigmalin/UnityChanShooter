using UnityEngine;
using System.Collections;
using UniRx;

public partial class PlayerActor : MonoBehaviour, ITarget 
{
	[System.Serializable]
	public class ActorData
	{
		public Animator Anim;
		public Collider Col;
		public Rigidbody Rigid;
		public NavMeshAgent Agent;
	}

	[SerializeField]
	ActorData mActorData;
	public ActorData Actordata { get { return mActorData; } }

	uint mActorID;
	public uint ActorID { get { return mActorID; } }

	// Use this for initialization
	void Start () 
	{		
		if (mActorData.Agent != null) 
		{
			mActorData.Agent.updatePosition = false;
			mActorData.Agent.updateRotation = false;
		}
	}

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

		case PlayerInst.SET_ACTOR_CONTROLLER:
			SetActorController ((ActorController)_params[0]);
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
			PlayerDead ();
			break;

		case PlayerInst.PLAYER_FOCUS:

			mMotionData.FocusOn = (Vector3)_params [0];
			break;

		case PlayerInst.PLAYER_LOCK:

			mMotionData.LockActor = (uint)_params [0];
			break;

		case PlayerInst.AGENT_GOTO:

			if (mActorData.Agent != null) 
			{
				mActorData.Agent.SetDestination ((Vector3)_params [0]);
				mActorData.Agent.speed = (float)_params [1];
				mActorData.Agent.stoppingDistance = (float)_params [2];
				mActorData.Agent.Resume ();
			}
			break;

		case PlayerInst.AGENT_STOP:

			if (mActorData.Agent != null) 
			{
				mActorData.Agent.Stop ();
				mActorData.Agent.speed = 0f;
			}

			if (mActorController != null) 
				mActorController.ExecCommand (PlayerInst.PLAYER_IDLE);
			break;

		default:
			if (mActorController != null) 
				mActorController.ExecCommand (_inst, _params);
			break;
		}
	}
}
