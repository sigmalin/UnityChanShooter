using UnityEngine;
using System.Collections;
using System.Linq;
using UniRx;
using UniRx.Triggers;

public sealed partial class InputShootgun : WeaponUiBehavior
{
	public ReactiveProperty<bool> CameraOperState { get; private set; }

	Camera mMainCamera;
	public Camera MainCamera
	{
		get 
		{
			if (mMainCamera == null) 
			{
				uint mainCameraID = (uint)GameCore.GetParameter (ParamGroup.GROUP_CAMERA, CameraParam.MAIN_CAMERA);;
				GameObject mainCameraGO = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_CAMERA, CameraParam.CAMERA_OBJECT, mainCameraID);
				mMainCamera = mainCameraGO.GetComponent<Camera>();
			}
			return mMainCamera;
		}
	}

	[SerializeField]
	WeaponUiButton mBtnFire;

	[SerializeField]
	WeaponUiButton mBtnJump;

	[SerializeField]
	WeaponUiButton mBtnTeleport;

	[SerializeField]
	WeaponUiButton mBtnConcentrate;

	[SerializeField]
	UiTracer[] mCrossHair;

	[SerializeField]
	Status mStatus;

	System.IDisposable mTrackDisposable;

	// Use this for initialization
	public override void Start () 
	{
		base.Start ();

		float moveSpeed = (float)GameCore.GetParameter (ParamGroup.GROUP_WEAPON, WeaponParam.WEAPON_ACTOR_SPEED, PlayerID);

		float attackRange = (float)GameCore.GetParameter (ParamGroup.GROUP_WEAPON, WeaponParam.WEAPON_ACTOR_RANGE, PlayerID);

		// move
		Device.Vec3JoyStickMoved
			.Where (_ => _ != Vector3.zero)
			.Subscribe ( _ =>
				{
					Vector3 dir = MainCamera.transform.TransformDirection(_);
					dir = new Vector3(dir.x, 0f, dir.z);
					dir = dir.normalized;
					GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_MOVE, PlayerID, dir, moveSpeed);
					GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_ROTATE, PlayerID, dir);
				});

		// idle
		Device.IsJoyStickUsed
			.Where (_ => _ == false)
			.Subscribe (_ => {
				GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_IDLE, PlayerID);
			});
		
		// Fire
		mBtnFire.DownTrigger.OnPointerDownAsObservable()
			.Subscribe (_ => GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_AIM, PlayerID, true));

		mBtnFire.UpTrigger.OnPointerUpAsObservable()
			.Subscribe (_ => GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_AIM, PlayerID, false));

		// Jump
		mBtnJump.UpTrigger.OnPointerUpAsObservable()
			.Subscribe (_ => GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_JUMP, PlayerID));

		// Teleport
		mBtnTeleport.UpTrigger.OnPointerUpAsObservable()
			.Subscribe (_ => GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.USE_ABILITY, PlayerID, AbilitySerial.TELEPORT));

		//Concentrate
		mBtnConcentrate.UpTrigger.OnPointerUpAsObservable()
			.Subscribe (_ => GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.USE_ABILITY, PlayerID, AbilitySerial.CONCENTRATE));

		// Tracking
		mTrackDisposable = InputStream
			.Buffer (System.TimeSpan.FromSeconds (0.5f))
			.Where (_ => MainCamera != null)
			.Subscribe (_ => Tracking(attackRange));

		//OperatorForStanealone ();
		OperatorForSmartphone();
	}

	void OnDestroy()
	{
		if (mTrackDisposable != null) 
		{
			mTrackDisposable.Dispose ();
			mTrackDisposable = null;
		}
	}

	public override void Hide()
	{
		base.Hide ();

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_IDLE, PlayerID);
		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_AIM, PlayerID, false);
	}

	public override void Localization()
	{
		base.Localization ();

		mStatus.Localization ();
	}

	public override void Clear()
	{
		Device.Clear ();

		//ClearForStanealone ();
		ClearForSmartphone();
	}

	public override void Operation(uint _inst, params System.Object[] _params)
	{
		base.Operation (_inst, _params);

		switch (_inst) 
		{
		case InstSet.SET_ACTOR_ID:
			mStatus.Initial (PlayerID);
			break;
		}
	}

	void Tracking(float _attackRange)
	{
		uint[] enemyIDs = (uint[])GameCore.GetParameter (ParamGroup.GROUP_WEAPON, WeaponParam.GET_HOSTILITY_LIST, PlayerID);

		if (enemyIDs == null || enemyIDs.Length == 0) 
		{
			SetCrossHair (mCrossHair[0], null);
			SetCrossHair (mCrossHair[1], null);
			return;
		}

		PlayerActor mineActor = (PlayerActor)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_DATA, PlayerID);

		if (mineActor == null)
			return;

		if (mineActor.HasFlag (PlayerActor.Flags.STUN) == true) 
		{
			GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_LOCK, PlayerID, 0u); 
			GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_SUB_LOCK, PlayerID, 0u); 

			SetCrossHair (mCrossHair[0], null);
			SetCrossHair (mCrossHair[1], null);
		} 
		else 
		{
			Vector3 minePos = mineActor.transform.position;
			Vector3 mineEye = mineActor.PlayerRole.BodyPt.Eye.position;

			PlayerActor[] targetActor = enemyIDs.Select (_ => (PlayerActor)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_DATA, _))
				.Where (_ => _ != null)
				.Select (_ => new { Actor = _, Distance = Vector3.Distance (minePos, _.transform.position) })
				.Where (_ => _.Distance < _attackRange)
				.OrderBy (_ => _.Distance)
				.Select (_ => _.Actor)
				.Where (_ => MainCamera.transform.IsPointAhead (_.PlayerRole.BodyPt.AimPt.position))
				.Where (_ => Physics.Raycast (mineEye, (_.PlayerRole.BodyPt.AimPt.position - mineEye).normalized, _attackRange, GameCore.GetRaycastLayer (GameCore.LAYER_DEFAULT)) == false)
				.Take (2).ToArray ();

			if (targetActor.Length == 0) 
			{
				GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_LOCK, PlayerID, 0u); 
				GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_SUB_LOCK, PlayerID, 0u); 

				SetCrossHair (mCrossHair[0], null);
				SetCrossHair (mCrossHair[1], null);
			} 
			else 
			{
				GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_LOCK, PlayerID, targetActor[0].ActorID); 

				SetCrossHair (mCrossHair[0], targetActor[0]);

				if (1 < targetActor.Length && mineActor.HasFlag(PlayerActor.Flags.FORM_CHANGE)) 
				{
					GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_SUB_LOCK, PlayerID, targetActor [1].ActorID); 

					SetCrossHair (mCrossHair [1], targetActor [1]);

					Vector3 dir = (targetActor [0].transform.position + targetActor [1].transform.position) * 0.5f - minePos;

					GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.CAMERA_DIRECT, 
						(uint)GameCore.GetParameter (ParamGroup.GROUP_CAMERA, CameraParam.MAIN_CAMERA), 
						new Vector3(dir.x,0f,dir.z));
				} 
				else 
				{
					GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_SUB_LOCK, PlayerID, targetActor [0].ActorID); 

					SetCrossHair (mCrossHair [1], null);

					Vector3 dir = targetActor [0].transform.position - minePos;

					GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.CAMERA_DIRECT, 
						(uint)GameCore.GetParameter (ParamGroup.GROUP_CAMERA, CameraParam.MAIN_CAMERA), 
						new Vector3(dir.x,0f,dir.z));
				}
			}
		}
	}

	void SetCrossHair(UiTracer _crossHair, PlayerActor _actor)
	{
		if (_crossHair == null)
			return;

		if (_actor == null) 
		{
			_crossHair.Target = null;
			_crossHair.MainCamera = null;

			if (_crossHair.gameObject.activeSelf == true)
				_crossHair.gameObject.SetActive (false);
		} 
		else 
		{
			_crossHair.Target = _actor.PlayerRole.BodyPt.AimPt;
			_crossHair.MainCamera = MainCamera;

			_crossHair.UpdateScreenPoint ();

			if (_crossHair.gameObject.activeSelf == false)
				_crossHair.gameObject.SetActive (true);
		}
	}
}
