using UnityEngine;
using System.Collections;
using UniRx;

public sealed partial class Flow_GamePlay : FlowBehaviour, IInput, IUserInterface
{
	#region Camp
	[System.Serializable]
	public struct Services
	{
		public uint PlayerID;
		public uint CharacterID;
		public int AiID;
		public Transform SpawnPt;
	}

	[SerializeField]
	Transform mMainPlayerSpawnPt;

	[SerializeField]
	Services[] mHostility;
	#endregion

	#region Camera
	[System.Serializable]
	public struct CameraList
	{
		public GameCamera MainCamera;
	}

	[SerializeField]
	CameraList mCameraList;
	#endregion

	const uint MAIN_PLAYER_ID = 1;

	public override void Enter()
	{
		base.Enter ();

		InitialCameraMode ();

		RunFlowStep ();
	}

	public override void Exit ()
	{
		ClearResultUI ();
		ReleaseInput ();

		base.Exit ();

		GameCore.ChangeScene ("Scene/Lobby");
	}

	public override void Event (uint _eventID)
	{
		switch(_eventID)
		{
		case FlowEvent.READ_CACHE_COMPLETED:
			break;

		case FlowEvent.READ_CACHE_FAILURE:
			Debug.Log ("READ_CACHE_FAILURE");
			break;

		case FlowEvent.ALL_ENEMY_DEAD:
			GotoCloseUp ();
			break;
		}
	}

	void RunFlowStep()
	{
		InitialInput ();

		Observable.NextFrame (FrameCountType.Update)
			.Do (_ => {
				RegisterCamera ();
				RegisterAllPlayer ();
				SetMainCameraMode ();
			})
			.SelectMany(Observable.NextFrame (FrameCountType.Update))
			.Subscribe (_ => 
				{					
					GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.GAME_START);
				});
	}

	#region Player
	void RegisterAllPlayer()
	{
		// register mine
		RegisterPlayer (MAIN_PLAYER_ID, GameCore.UserProfile.MainCharacterID, 0, 1, mMainPlayerSpawnPt);

		// register enemy
		if (mHostility.Length != 0) 
		{
			mHostility.ToObservable()
				//.Subscribe(_ => RegisterPlayer(_.PlayerID, _.CharacterID, _.AiID, 2, _.SpawnPt));
				.Subscribe(_ => RegisterPlayer(_.PlayerID, GameCore.UserProfile.MainCharacterID, _.AiID, 2, _.SpawnPt));
		}

		// register main actor
		GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.MAIN_ACTOR, MAIN_PLAYER_ID);
	}

	void RegisterPlayer(uint _playerID, uint _characterID, int _aiID, int _team, Transform _spawn)
	{
		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.CREATE_PLAYER, _playerID, _playerID == MAIN_PLAYER_ID ? ContainerDefine.CONTAINER_PLAYER : ContainerDefine.CONTAINER_AI);
		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.SET_POSITION, _playerID, _spawn.position);
		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.SET_DIRECTION, _playerID, _spawn.rotation);


		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.SET_MODEL, _playerID, _characterID);

		uint weaponID = GetWeaponID (_characterID);

		GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.REGISTER_ACTOR, _playerID, _characterID, weaponID);
		GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.SET_TEAM, _playerID, _team);

		if (_aiID != 0)
			GameCore.SendCommand (CommandGroup.GROUP_AI, AiInst.REGISTER_AI, _playerID, _aiID);
	}

	uint GetWeaponID(uint _characterID)
	{
		CharacterRepository.CharacterData character = (CharacterRepository.CharacterData)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY, RepositoryParam.GET_CHARACTER_DATA, _characterID);
		return character.weaponID;
	}
	#endregion

	#region Camera
	void RegisterCamera()
	{
		GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.CAMERA_REGISTER, mCameraList.MainCamera);
		GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.MAIN_CAMERA, mCameraList.MainCamera.CameraID);
	}
	#endregion
}
