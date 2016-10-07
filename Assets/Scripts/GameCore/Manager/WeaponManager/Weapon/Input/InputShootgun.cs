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
	UiTracer mCrossHair;

	[SerializeField]
	Status mStatus;

	// Use this for initialization
	public override void Start () 
	{
		base.Start ();

		// move
		Device.Vec3JoyStickMoved
			.Where (_ => _ != Vector3.zero)
			.Subscribe ( _ =>
				{
					Vector3 dir = MainCamera.transform.TransformDirection(_);
					dir = new Vector3(dir.x, 0f, dir.z);
					dir = dir.normalized;
					GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_MOVE, PlayerID, dir, 1f);
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

		// Tracking
		InputStream
			.Buffer (System.TimeSpan.FromSeconds (0.5f))
			.Where (_ => MainCamera != null)
			.Subscribe (_ => Tracking());

		OperatorForStanealone ();
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

	void Tracking()
	{
		uint[] enemyIDs = (uint[])GameCore.GetParameter (ParamGroup.GROUP_WEAPON, WeaponParam.GET_HOSTILITY_LIST, PlayerID);

		if (enemyIDs == null || enemyIDs.Length == 0) 
		{
			SetCrossHair (null);
			return;
		}

		PlayerActor mineActor = (PlayerActor)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_DATA, PlayerID);

		if (mineActor == null)
			return;

		Vector3 minePos = mineActor.transform.position;
		Vector3 mineEye = mineActor.PlayerRole.BodyPt.Eye.position;

		PlayerActor targetActor = enemyIDs.Select (_ => (PlayerActor)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_DATA, _))
			.Where (_ => _ != null)
			.Select (_ => new { Actor = _, Distance = Vector3.Distance (minePos, _.transform.position) })
			.Where (_ => _.Distance < 5f)
			.OrderBy (_ => _.Distance)
			.Select (_ => _.Actor)
			.Where (_ => MainCamera.transform.IsPointAhead (_.PlayerRole.BodyPt.AimPt.position))
			.Where (_ => Physics.Raycast (mineEye, (_.PlayerRole.BodyPt.AimPt.position - mineEye).normalized, 5f, GameCore.GetRaycastLayer(GameCore.LAYER_DEFAULT)) == false)
			.FirstOrDefault ();

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_LOCK, PlayerID, targetActor == null ? 0u : targetActor.ActorID); 

		SetCrossHair (targetActor);
	}

	void SetCrossHair(PlayerActor _actor)
	{
		if (mCrossHair == null)
			return;

		if (_actor == null) 
		{
			mCrossHair.Target = null;
			mCrossHair.MainCamera = null;

			if (mCrossHair.gameObject.activeSelf == true)
				mCrossHair.gameObject.SetActive (false);
		} 
		else 
		{
			mCrossHair.Target = _actor.PlayerRole.BodyPt.AimPt;
			mCrossHair.MainCamera = MainCamera;

			mCrossHair.UpdateScreenPoint ();

			if (mCrossHair.gameObject.activeSelf == false)
				mCrossHair.gameObject.SetActive (true);
		}
	}
}
