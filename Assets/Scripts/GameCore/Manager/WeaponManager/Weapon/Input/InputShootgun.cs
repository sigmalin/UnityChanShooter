using UnityEngine;
using System.Collections;
using UniRx;

public class InputShootgun : MonoBehaviour, IInput 
{
	Transform mGameCamera = null;
	Transform GameCamera
	{
		get 
		{
			if (mGameCamera == null) 
			{
				CameraManager.CameraData data = (CameraManager.CameraData)GameCore.GetParameter (ParamGroup.GROUP_CAMERA, CameraParam.MAIN_CAMERA_DATA);
				if (data.RefCamera != null) mGameCamera = data.RefCamera.transform;
			}
			return mGameCamera;
		}
	}

	public IUserInterface UserInterface { get { return null; } }

	#region uniRx
	Subject<Unit> mOnHandleInputSubject = new Subject<Unit>();
	#endregion

	// Use this for initialization
	void Start () 
	{
		IObservable<Unit> inputStream = mOnHandleInputSubject.AsObservable ();

		//IObservable<Unit> inputStream = mOnHandleInputSubject.Publish().RefCount();

		ControlPlayer (
			inputStream.Select (_ => (uint)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.MAIN_PLAYER)).Publish ().RefCount(),
			inputStream);

		ControlCamera (
			inputStream.Select (_ => (uint)GameCore.GetParameter (ParamGroup.GROUP_CAMERA, CameraParam.MAIN_CAMERA)).Publish ().RefCount(),
			inputStream);
	}
	
	// Update is called once per frame
	public bool HandleInput ()
	{
		mOnHandleInputSubject.OnNext (Unit.Default);

		return false;
	}

	void ControlPlayer(IObservable<uint> _playerStream, IObservable<Unit> _inputStream)
	{
		// move
		_inputStream
			.Where (_ => Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D))
			.Select (_ => new Vector3 (Input.GetAxis ("Horizontal"), 0F, Input.GetAxis ("Vertical")))
			.Where (_ => 0F < _.magnitude)
			.Zip (_playerStream, (_mov, _id) => 
				new {
					mov = _mov.normalized, id = _id
				})
			.First ()
			.Repeat ()
			.Subscribe ( _ =>
				{
					GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_MOVE, _.id, _.mov);
					GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_ROTATE, _.id, _.mov);
				});

		_inputStream
			.Select(_ => Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D))
			.DistinctUntilChanged()
			.Where(_ => !_)
			.Zip (_playerStream, (_, _id) =>
				new {
					id = _id
				})
			.First ()
			.Repeat ()
			.Subscribe (_ => GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_IDLE, _.id));

		// jump
		_inputStream
			.Where (_ => Input.GetKeyUp (KeyCode.Space))
			.Zip (_playerStream, (_, _id) =>
				new {
					id = _id
				})
			.First ()
			.Repeat ()
			.Subscribe (_ => GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_JUMP, _.id));

		// aim
		_inputStream
			.Select (_ => Input.GetMouseButton(0))
			.DistinctUntilChanged()
			.Zip (_playerStream, (_, _id) =>
				new {
					id = _id, aim = _
				})
			.First ()
			.Repeat ()
			.Subscribe (_ => GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_AIM, _.id, _.aim));
	}

	void ControlCamera(IObservable<uint> _cameraStream, IObservable<Unit> _inputStream)
	{
		// Lock Mouse
		_inputStream
			.Where (_ => Cursor.lockState != CursorLockMode.Locked)
			.Subscribe (
				_ =>
				{
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;
				},
				() => 
				{
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
				}
			);
		
		// Move CrossHair
		_cameraStream
			.Subscribe (_ => 
				{
					GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.CAMERA_MOVEMENT, _, Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"), Input.GetAxis("Mouse ScrollWheel"));
				});
	}
}
