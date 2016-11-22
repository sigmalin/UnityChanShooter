using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public sealed partial class Flow_Lobby : FlowBehaviour 
{
	public override void Enter()
	{
		base.Enter ();

		GameCore.SendCommand (CommandGroup.GROUP_REPOSITORY, 
			RepositoryInst.LOAD_CHARACTER_DATA, 
			(CharacterRepository)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_CHARACTER_DATA, false));

		GameCore.SendCommand (CommandGroup.GROUP_REPOSITORY, 
			RepositoryInst.LOAD_LOCALIZATION, 
			(LocalizationRepository)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_LOCALIZATION, "Tc", false));

		GameCore.SendCommand (CommandGroup.GROUP_REPOSITORY, 
			RepositoryInst.LOAD_CHAPTER_DATA, 
			(ChapterRepository)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_CHAPTER_DATA, false));

		GameCore.SendCommand (CommandGroup.GROUP_REPOSITORY, 
			RepositoryInst.LOAD_WEAPON_DATA, 
			(WeaponDataRepository)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_WEAPON_DATA, false));

		GameCore.SendCommand (CommandGroup.GROUP_SYSTEM,
			SystemInst.PLAY_BGM,
			SystemManager.BGM_LOBBY, true);

		//GameCore.SendCommand (CommandGroup.GROUP_REPOSITORY, 
		//	RepositoryInst.lo, 
		//	(ChapterRepository)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_CHAPTER_DATA, false));
	}

	public override void Event (uint _eventID)
	{
	}
}
