using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Actor : DisplayBehaviour
{
	[System.Serializable]
	public class BodyPoint
	{
		public Transform RightHand;
		public Transform LeftHand;
	}

	[System.Serializable]
	public class ActorData
	{
		public Transform Root;
		public Animator Anim;
		public Collider Col;
		public Rigidbody Rigid;
		public BodyPoint BodyPt;
	}

	[SerializeField]
	ActorData mActorData;
	public ActorData Actordata { get { return mActorData; } }

	PlayerManager.PlayerData mRefPlayerData = null;
	public PlayerManager.PlayerData PlayerData { get { return mRefPlayerData; }}

	WeaponLauncher mLauncher;
	public WeaponLauncher Launcher { get { return mLauncher; } }

	ActorController mActorController = null;

	// Use this for initialization
	void Start () 
	{
	}

	void OnDestroy()
	{
		RemoveAll ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (mActorController == null)
			return;

		mActorController.OnUpdate ();
	}

	void OnAnimatorMove()
	{
		if (mActorController == null)
			return;
		
		mActorController.OnAnimMove ();
	}

	void OnAnimatorIK()
	{
		if (mActorController == null)
			return;

		mActorController.OnAnimIK ();
	}

	void SetActorController(ActorController _actorCtrl)
	{
		mActorController = _actorCtrl;

		mActorController.Initial (this);
	}

	public void ExecCommand(uint _inst, params System.Object[] _params)
	{
		switch(_inst)
		{
		case PlayerInst.CREATE_PLAYER:
			mRefPlayerData = (PlayerManager.PlayerData)_params [0];

			mRefPlayerData.LookAt = this.transform.position + this.transform.up * 0.8f + this.transform.forward;
			break;

		case PlayerInst.MAIN_PLAYER:
			if (mLauncher != null) 
			{
				GameCore.PushInput (mLauncher.InputDevice);
			}
			break;

		case PlayerInst.SET_CONTAINER:
			
			RecycleContainer ();

			GameObject container = (GameObject)_params [0];
			mActorData.Root = container.transform;
			mActorData.Col = container.GetComponent<Collider> ();
			mActorData.Rigid = container.GetComponent<Rigidbody> ();

			this.transform.parent = mActorData.Root;
			this.transform.localPosition = Vector3.zero;
			this.transform.localRotation = Quaternion.identity;
			this.transform.localScale = Vector3.one;
			break;

		case PlayerInst.SET_WEAPON:

			RecycleWeaponModel ();

			GameObject launcherGO = ((GameObject)_params [0]);
			mLauncher = launcherGO.GetComponent<WeaponLauncher> ();

			if (mLauncher != null) 
			{
				if (mLauncher.RightHand != null) 
				{
					mLauncher.RightHand.transform.parent = mActorData.BodyPt.RightHand;
					mLauncher.RightHand.transform.localPosition = Vector3.zero;
					mLauncher.RightHand.transform.localRotation = Quaternion.identity;
				}

				if (mLauncher.LeftHand != null) 
				{
					mLauncher.LeftHand.transform.parent = mActorData.BodyPt.LeftHand;
					mLauncher.LeftHand.transform.localPosition = Vector3.zero;
					mLauncher.LeftHand.transform.localRotation = Quaternion.identity;
				}

				SetActorController (mLauncher.GetController());
			}
			break;

		case PlayerInst.REMOVE_PLAYER:
			
			RemoveAll ();

			RecycleResource (this.gameObject);
			break;

		default:
			if (mActorController != null) 
				mActorController.ExecCommand (_inst, _params);
			break;
		}
	}

	void RemoveAll()
	{
		mRefPlayerData = null;

		if (mActorController != null)
			mActorController.Clear ();

		mActorController = null;

		RecycleWeaponModel ();

		RecycleContainer ();
	}

	void RecycleResource(GameObject _resGO)
	{
		GameCoreResRecycle recycle = _resGO.GetComponent<GameCoreResRecycle> ();
		if (recycle != null)
			recycle.Recycle ();
		else
			GameObject.Destroy (_resGO);
	}

	void RecycleWeaponModel()
	{
		if (mLauncher == null)
			return;

		if (mLauncher.RightHand != null)
			mLauncher.RightHand.transform.parent = mLauncher.transform;

		if (mLauncher.LeftHand != null)
			mLauncher.LeftHand.transform.parent = mLauncher.transform;

		RecycleResource (mLauncher.gameObject);

		mLauncher = null;
	}

	void RecycleContainer()
	{
		if (mActorData.Root == null)
			return;

		GameObject container = mActorData.Root.gameObject;

		mActorData.Root = null;
		mActorData.Col = null;
		mActorData.Rigid = null;

		RecycleResource (container);
	}
}
