﻿using UnityEngine;
using System.Collections;
using System.Linq;
using UniRx;

public sealed partial class Flow_WipeOut : FlowBehaviour, IInput, IUserInterface 
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
	#endregion

	#region Enemy
	[System.Serializable]
	public struct EnemyGroup
	{
		[HideInInspector]public bool IsTriggered;
		public Services[] ServicesList;
	}

	[SerializeField]
	EnemyGroup[] mEnemyGroup;
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

	[SerializeField]
	Transform mTombPt;

	const uint MAIN_PLAYER_ID = 1;

	public override void Enter()
	{
		base.Enter ();

		GameCore.SendCommand (CommandGroup.GROUP_SYSTEM, SystemInst.PLAY_BGM, "scene", true);

		GameCore.SendCommand (CommandGroup.GROUP_REPOSITORY, 
			RepositoryInst.LOAD_SPRITE_DATA, 
			RepositoryManager.SPRITE_DIGITS,
			(SpriteRepository)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_SPRITE, RepositoryManager.SPRITE_DIGITS, false));

		GameCore.SendCommand (CommandGroup.GROUP_REPOSITORY, 
			RepositoryInst.LOAD_SPRITE_DATA, 
			RepositoryManager.SPRITE_HITS,
			(SpriteRepository)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_SPRITE, RepositoryManager.SPRITE_HITS, false));

		GameCore.SendCommand (CommandGroup.GROUP_REPOSITORY, 
			RepositoryInst.LOAD_POST_EFFECT_DATA, 
			(PostEffectRepository)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_POST_EFFECT, false));

		InitialCameraMode ();

		PreLoad ();

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

		case FlowEvent.MAIN_ACTOR_DEAD:
			GotoFailure ();
			break;

		case FlowEvent.GAME_TRIGGER_EVENT_1:
			RegisterEnemyGroup (1);
			break;

		case FlowEvent.GAME_TRIGGER_EVENT_2:
			RegisterEnemyGroup (2);
			break;

		case FlowEvent.GAME_TRIGGER_EVENT_3:
			RegisterEnemyGroup (3);
			break;

		case FlowEvent.GAME_TRIGGER_EVENT_4:
			RegisterEnemyGroup (4);
			break;

		case FlowEvent.GAME_TRIGGER_EVENT_5:
			RegisterEnemyGroup (5);
			break;

		case FlowEvent.GAME_TRIGGER_EVENT_6:
			RegisterEnemyGroup (6);
			break;

		case FlowEvent.GAME_TRIGGER_EVENT_7:
			RegisterEnemyGroup (7);
			break;

		case FlowEvent.GAME_TRIGGER_EVENT_8:
			RegisterEnemyGroup (8);
			break;

		case FlowEvent.GAME_TRIGGER_EVENT_9:
			RegisterEnemyGroup (9);
			break;
		}
	}

	void RunFlowStep()
	{
		InitialInput ();

		Observable.NextFrame (FrameCountType.Update)
			.Do (_ => RegisterList ())
			.DelayFrame(1, FrameCountType.Update)
			.Subscribe (_ => {
				
				SetMainCameraMode ();

				#if UNITY_EDITOR
				//ResetShader();
				#endif
			});
	}

	#region Player
	void RegisterMainPlayer()
	{
		// register mine
		RegisterPlayer (MAIN_PLAYER_ID, GameCore.UserProfile.MainCharacterID, 0, 1, mMainPlayerSpawnPt);

		// register main actor
		GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.MAIN_ACTOR, MAIN_PLAYER_ID);
	}

	void RegisterEnemyGroup(int _Indx)
	{
		if (mEnemyGroup == null)
			return;

		if (mEnemyGroup.Length <= _Indx)
			return;

		if (mEnemyGroup [_Indx].IsTriggered == true)
			return;

		mEnemyGroup [_Indx].IsTriggered = true;

		mEnemyGroup [_Indx].ServicesList.ToObservable()
			.Subscribe(_ => RegisterPlayer(_.PlayerID, _.CharacterID, _.AiID, 2, _.SpawnPt));
	}

	void RegisterPlayer(uint _playerID, uint _characterID, int _aiID, int _team, Transform _spawn)
	{
		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.CREATE_PLAYER, _playerID, _playerID == MAIN_PLAYER_ID ? ContainerDefine.CONTAINER_PLAYER : ContainerDefine.CONTAINER_AI);
		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.SET_POSITION, _playerID, _spawn.position);
		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.SET_DIRECTION, _playerID, _spawn.rotation);


		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.SET_MODEL, _playerID, _characterID);

		GameCore.SendCommand (CommandGroup.GROUP_WEAPON, 
			WeaponInst.REGISTER_ACTOR, 
			_playerID,
			_characterID);
		
		GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.SET_TEAM, _playerID, _team);

		if (_aiID != 0)
			GameCore.SendCommand (CommandGroup.GROUP_AI, AiInst.REGISTER_AI, _playerID, _aiID);
	}
	#endregion

	#region Enemy
	bool IsAllEnemyDead()
	{
		return mEnemyGroup.Where (_ => _.IsTriggered == false).Count () == 0;
	}
	#endregion

	#region Camera
	void RegisterCamera()
	{
		GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.CAMERA_REGISTER, mCameraList.MainCamera);
		GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.MAIN_CAMERA, mCameraList.MainCamera.CameraID);
	}
	#endregion

	#region PreLoad
	void PreLoad()
	{
		// Mine
		PreLoadModel(GameCore.UserProfile.MainCharacterID);
		PreLoadRagDoll(GameCore.UserProfile.MainCharacterID);

		// Enemy
		mEnemyGroup.ToObservable ()
			.Select (_ => _.ServicesList)
			.Aggregate ((_pre, _cur) => _cur = _pre.Concat (_cur).ToArray ())
			.SelectMany (_ => _.ToObservable ())
			.Select (_ => _.CharacterID)
			.Distinct ()
			.Subscribe (_ => 
				{
					PreLoadModel(_);
					PreLoadRagDoll(_);
				}
			);
	}

	void PreLoadModel(uint _characterID)
	{
		GameObject roleGO = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.CHARACTER_MODEL, _characterID);
		if (roleGO != null)
			roleGO.SafeRecycle ();
	}

	void PreLoadRagDoll(uint _characterID)
	{
		if (mTombPt == null)
			return;

		GameObject ragdollGO = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.RAGDOLL_MODEL, _characterID);
		if (ragdollGO != null)
		{
			ragdollGO.transform.position = mTombPt.position;
			ragdollGO.SafeRecycle ();
		}
	}
	#endregion

	#region Register
	void RegisterList()
	{
		RegisterCamera ();
		RegisterMainPlayer ();
		RegisterEnemyGroup (0);
	}
	#endregion

	#if UNITY_EDITOR
	void ResetShader()
	{
		Renderer[] renders = GameObject.FindObjectsOfType<Renderer>();

		renders.ToObservable ()
			.SelectMany (_ => _.materials.ToObservable ())
			.Subscribe (_ => _.shader = Shader.Find (_.shader.name));
	}
	#endif
}
