using UnityEngine;
using System.Collections;
using UniRx;

public partial class PlayerActor : MonoBehaviour 
{
	[System.Serializable]
	public class ActorData
	{
		public Animator Anim;
		public Collider Col;
		public Rigidbody Rigid;
	}

	[SerializeField]
	ActorData mActorData;
	public ActorData Actordata { get { return mActorData; } }

	uint mActorID;
	public uint ActorID { get { return mActorID; } }

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
			SetRoleModel((GameObject)_params [0]);
			break;

		case PlayerInst.SET_WEAPON:
			SetWeaponModel((GameObject)_params [0]);
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

		default:
			if (mActorController != null) 
				mActorController.ExecCommand (_inst, _params);
			break;
		}
	}

	void RecycleResource(GameObject _resGO)
	{
		GameCoreResRecycle recycle = _resGO.GetComponent<GameCoreResRecycle> ();
		if (recycle != null)
			recycle.Recycle ();
		else
			GameObject.Destroy (_resGO);
	}
}
