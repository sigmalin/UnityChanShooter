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
	}

	[System.Serializable]
	public struct Camp
	{
		public Transform[] SpawnPtList;
		public Services[] ServicesList;
	}

	[SerializeField]
	Camp mFriendly;

	[SerializeField]
	Camp mEnemy;
	#endregion

	#region Camera
	[System.Serializable]
	public struct CameraUnit
	{
		public GameCamera CameraOB;
		public uint CameraID;
	}

	[System.Serializable]
	public struct CameraList
	{
		public CameraUnit MainCamera;
	}

	[SerializeField]
	CameraList mCameraList;
	#endregion

	const uint MAIN_PLAYER_ID = 1;

	const uint NORMAL_CONTAINER = 1;

	public override void Enter()
	{
		base.Enter ();

		RunFlowStep ();
	}

	public override void Exit ()
	{
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
		}
	}

	void RunFlowStep()
	{
		InitialInput ();

		Observable.NextFrame (FrameCountType.Update)
			.Do (_ => {
				RegisterCamera ();
				RegisterAllPlayer ();
			})
			.SelectMany(Observable.NextFrame (FrameCountType.Update))
			.Subscribe (_ => 
				{
					GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.MAIN_ACTOR, MAIN_PLAYER_ID);
					GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.GAME_START);
				});
	}

	#region Player
	void RegisterAllPlayer()
	{
		// register mine
		RegisterPlayer (MAIN_PLAYER_ID, GameCore.UserProfile.MainCharacterID, false, mFriendly.SpawnPtList);

		// register enemy
		if (mEnemy.ServicesList.Length != 0) 
		{
			mEnemy.ServicesList.ToObservable()
				.Subscribe(_ => RegisterPlayer(_.PlayerID, _.CharacterID, true, mEnemy.SpawnPtList));
		}
	}

	void RegisterPlayer(uint _playerID, uint _characterID, bool _isEnemy, Transform[] _spawn)
	{
		GameObject character = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.CHARACTER_MODEL, _characterID);

		Transform spawn = _spawn [Random.Range (0, _spawn.Length)];

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.CREATE_PLAYER, _playerID, NORMAL_CONTAINER);
		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.SET_POSITION, _playerID, spawn.position);
		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.SET_DIRECTION, _playerID, spawn.rotation);
		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.SET_LAYER, _playerID, _isEnemy ? GameCore.LAYER_ENEMY : GameCore.LAYER_PLAYER);

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.SET_MODEL, _playerID, character);

		uint weaponID = GetWeaponID (_characterID);

		GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.REGISTER_ACTOR, _playerID, weaponID);
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
		GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.CAMERA_REGISTER, mCameraList.MainCamera.CameraID, mCameraList.MainCamera.CameraOB);
		GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.MAIN_CAMERA, mCameraList.MainCamera.CameraID);
	}
	#endregion
}
