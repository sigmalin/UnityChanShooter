using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public sealed class InputShootgun : WeaponUiBehavior
{
	public ReactiveProperty<bool> CameraOperState { get; private set; }

	Transform mTransMainCamera;
	public Transform TransMainCamera
	{
		get 
		{
			if (mTransMainCamera == null) 
			{
				uint mainCameraID = (uint)GameCore.GetParameter (ParamGroup.GROUP_CAMERA, CameraParam.MAIN_CAMERA);;
				GameObject mainCameraGO = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_CAMERA, CameraParam.CAMERA_OBJECT, mainCameraID);
				mTransMainCamera = mainCameraGO.transform;
			}
			return mTransMainCamera;
		}
	}

	System.IDisposable mCameraOperDisposable;

	[SerializeField]
	WeaponUiButton mBtnFire;

	[SerializeField]
	WeaponUiButton mBtnJump;

	// Use this for initialization
	public override void Start () 
	{
		base.Start ();

		// move
		Device.Vec3JoyStickMoved
			.Subscribe ( _ =>
				{
					Vector3 dir = TransMainCamera.TransformDirection(_);
					dir = new Vector3(dir.x, 0f, dir.z);
					dir = dir.normalized;
					GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_MOVE, PlayerID, dir);
					GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_ROTATE, PlayerID, dir);
				});

		// idle
		Device.IsJoyStickUsed
			.Where (_ => _ == false)
			.Subscribe (_ => {
				GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_IDLE, PlayerID);
			});

		// Move CrossHair
		CameraOperState = new ReactiveProperty<bool>(false);

		InputStream.Where(_ => CameraOperState.Value == true)
			.Select(_ => Input.mousePosition)
			.Scan((_pre, _cur) => {
				Vector3 dir = (_cur - _pre) * 0.5f;
				GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.CAMERA_MOVEMENT, 
					(uint)GameCore.GetParameter (ParamGroup.GROUP_CAMERA, CameraParam.MAIN_CAMERA), 
					dir.x, dir.y, Input.GetAxis("Mouse ScrollWheel"));

				return _cur;
			})
			.Subscribe (_ => {});

		InputStream
			.Where (_ => Input.GetMouseButtonDown (0) == true && GameCore.IsTouchInterface (Input.mousePosition) == false)
			.Subscribe (_ => CameraOperState.Value = true);

		InputStream
			.Where(_ => Input.GetMouseButtonUp(0) == true)
			.Subscribe (_ => CameraOperState.Value = false);

		// Fire
		mBtnFire.DownTrigger.OnPointerDownAsObservable()
			.Subscribe (_ => GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_AIM, PlayerID, true));

		mBtnFire.UpTrigger.OnPointerUpAsObservable()
			.Subscribe (_ => GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_AIM, PlayerID, false));

		// Jump
		mBtnJump.UpTrigger.OnPointerUpAsObservable()
			.Subscribe (_ => GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_JUMP, PlayerID));

		// Tracking
		//InputStream
		//	.Buffer (System.TimeSpan.FromSeconds (0.5f))
		//	.Subscribe (_ => Tracking());
	}

	public override void Hide()
	{
		//Cursor.lockState = CursorLockMode.None;
		//Cursor.visible = true;
	}

	void Tracking()
	{
		Transform transPlayer = (Transform)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_TRANSFORM, PlayerID);

		Collider[] cols = Physics.OverlapSphere (transPlayer.position, 5F, GameCore.GetRaycastLayer (GameCore.LAYER_ENEMY));
	}
}
