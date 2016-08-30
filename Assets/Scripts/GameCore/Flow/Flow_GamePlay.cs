using UnityEngine;
using System.Collections;
using UniRx;

public sealed partial class Flow_GamePlay : FlowBehaviour, IInput 
{
	#region Spawn
	[SerializeField]
	Transform[] mSpawnPtList;
	#endregion

	#region Actor
	public enum ArmType
	{
		Shotgun = 1,
	}

	[System.Serializable]
	public class ActorData
	{
		public uint ID;
		public ArmType Arm;
	}

	[System.Serializable]
	public class ActorList
	{
		public uint MainPlayerID = 1;
		public ActorData[] ActorDataList;
	}

	[SerializeField]
	ActorList mActorList;
	#endregion

	#region Camera
	[System.Serializable]
	public class CameraList
	{
		public GameCamera MainCamera;
	}

	[SerializeField]
	CameraList mCameraList;
	#endregion

	public override void Enter()
	{
		base.Enter ();

		LoadCache ();
	}

	public override void Exit ()
	{
		GameCore.PopInputUntil (this);

		base.Exit ();

		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
	}

	public override void Event (uint _eventID)
	{
		switch(_eventID)
		{
		case FlowEvent.LOAD_CACHE_COMPLETED:
			RunFlowStep ();
			break;

		case FlowEvent.LOAD_CACHE_FAILURE:
			Debug.Log ("LOAD_CACHE_FAILURE");
			break;
		}
	}

	void RunFlowStep()
	{
		InitialInput ();

		Observable.NextFrame (FrameCountType.Update)
			.Do (_ => LoadGameData ())
			.SelectMany(Observable.NextFrame (FrameCountType.Update))
			.Do (_ => RegisterActor ())
			.SelectMany(Observable.NextFrame (FrameCountType.Update))
			.Do (_ => RegisterCamera ())
			.SelectMany(Observable.NextFrame (FrameCountType.Update))
			.Subscribe (_ => 
				{
					GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.GAME_START);
				});
	}

	#region Spawn
	Transform GetRandonSpawn()
	{
		return mSpawnPtList [Random.Range (0, mSpawnPtList.Length)];
	}
	#endregion

	#region Actor
	void RegisterActor()
	{
		for (int Indx = 0; Indx < mActorList.ActorDataList.Length; ++Indx) 
		{
			GameObject container = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.CONTAINER, (uint)1);
			Transform spawn = GetRandonSpawn ();
			container.transform.position = spawn.position;
			container.transform.rotation = spawn.rotation;

			GameObject character = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.CHARACTER_MODEL, (uint)100);
			Actor actor = character.GetComponent<Actor> ();
			if (actor == null)
				continue;
			
			GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.CREATE_PLAYER, mActorList.ActorDataList[Indx].ID, actor);
			GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.SET_CONTAINER, mActorList.ActorDataList[Indx].ID, container);

			GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.REGISTER_ACTOR, mActorList.ActorDataList[Indx].ID, GetWeaponID (mActorList.ActorDataList[Indx].Arm));

			uint modelID = (uint)GameCore.GetParameter (ParamGroup.GROUP_WEAPON, WeaponParam.MODEL_ID, GetWeaponID (mActorList.ActorDataList[Indx].Arm));
			GameObject weapon = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.WEAPON_MODEL, modelID);
			if (weapon != null)
				GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.SET_WEAPON, mActorList.ActorDataList[Indx].ID, weapon);
		}

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.MAIN_PLAYER, mActorList.MainPlayerID);
	}

	uint GetWeaponID(ArmType _type)
	{
		uint weaponID = 0;

		switch(_type)
		{
		case ArmType.Shotgun:
			weaponID = 1;
			break;
		}

		return weaponID;
	}
	#endregion

	#region Camera
	void RegisterCamera()
	{
		uint mainCameraID = 1;

		GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.CAMERA_REGISTER, mainCameraID, mCameraList.MainCamera);
		GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.MAIN_CAMERA, mainCameraID);

		GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.CAMERA_TARGET, mainCameraID, mActorList.MainPlayerID);

		GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.SET_CAMERA_MODE_FOLLOW, mainCameraID);
	}
	#endregion

	#region GameData
	void LoadGameData()
	{
		GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.LOAD_DATA, GameCore.GetParameter (CommandGroup.GROUP_RESOURCE, ResourceParam.WEAPON_DATA));
	}
	#endregion

	#region Cache
	void LoadCache()
	{
		GameCore.SendCommand (CommandGroup.GROUP_CACHE, CacheInst.LOAD_CACHE, (string)GameCore.GetParameter(ParamGroup.GROUP_RESOURCE, ResourceParam.GET_BULLET_PATH, (uint)1));
		GameCore.SendCommand (CommandGroup.GROUP_CACHE, CacheInst.LOAD_CACHE, (string)GameCore.GetParameter(ParamGroup.GROUP_RESOURCE, ResourceParam.GET_CHARACTER_PATH, (uint)100));
		GameCore.SendCommand (CommandGroup.GROUP_CACHE, CacheInst.LOAD_CACHE, (string)GameCore.GetParameter(ParamGroup.GROUP_RESOURCE, ResourceParam.GET_CHARACTER_PATH, (uint)101));
		GameCore.SendCommand (CommandGroup.GROUP_CACHE, CacheInst.LOAD_CACHE, (string)GameCore.GetParameter(ParamGroup.GROUP_RESOURCE, ResourceParam.GET_CONTAINER_PATH, (uint)1));
		GameCore.SendCommand (CommandGroup.GROUP_CACHE, CacheInst.LOAD_CACHE, (string)GameCore.GetParameter(ParamGroup.GROUP_RESOURCE, ResourceParam.GET_WEAPON_DATA_PATH));
		GameCore.SendCommand (CommandGroup.GROUP_CACHE, CacheInst.LOAD_CACHE, (string)GameCore.GetParameter(ParamGroup.GROUP_RESOURCE, ResourceParam.GET_INSTANT_RESOURCE_INPUT_PATH));
		GameCore.SendCommand (CommandGroup.GROUP_CACHE, CacheInst.LOAD_CACHE, (string)GameCore.GetParameter(ParamGroup.GROUP_RESOURCE, ResourceParam.GET_WEAPON_PATH, (uint)1));
	}
	#endregion
}
