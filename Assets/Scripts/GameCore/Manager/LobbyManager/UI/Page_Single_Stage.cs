using UnityEngine;
using System.Collections;
using System.Linq;
using UniRx;
using UniRx.Triggers;

public sealed partial class Page_Single 
{
	[SerializeField]
	UnityEngine.UI.Text mStageTitle;

	[SerializeField]
	UnityEngine.UI.Text mStageIntroduction;

	[SerializeField]
	UnityEngine.UI.Button mBtnMissionStart;

	[SerializeField]
	UnityEngine.UI.RawImage mStageImage;

	System.IDisposable mMissionBtnDisposable;

	void InitialStage()
	{
		if (mMissionBtnDisposable != null)
			mMissionBtnDisposable.Dispose ();
		
		mMissionBtnDisposable = mBtnMissionStart.OnClickAsObservable ()
			.Subscribe (_ => GoStage());
	}

	void SettingStage(int _chapterID, int _stage)
	{
		CurrentChapter = _chapterID;

		CurrentStage = _stage;

		SetStageImage ();

		SetLocalizationStage ();
	}

	void SetStageImage()
	{
		mStageImage.texture = (Texture)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_STAGE_IMAGE, CurrentChapter, CurrentStage + 1, false);

		if (mStageImage.texture == null) 
		{
			string[] loadList = new string[] {
				(string)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_STAGE_IMAGE_PATH, CurrentChapter)
			};

			GameCore.AdditionalLoad (
				loadList,
				() => 
				{
					if (mStageImage != null)
						mStageImage.texture = (Texture)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_STAGE_IMAGE, CurrentChapter, CurrentStage + 1, false);
				},
				true
			);
		}
	}

	void SetLocalizationStage()
	{
		mStageTitle.text = (string)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY, RepositoryParam.GET_LOCALIZATION, 
			string.Format (LocalizationDefine.LOCALIZATION_GROUP_STAGE, CurrentChapter, CurrentStage + 1),
			LocalizationDefine.LOCALIZATION_KEY_STAGE_TITLE);

		mStageIntroduction.text = (string)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY, RepositoryParam.GET_LOCALIZATION, 
			string.Format (LocalizationDefine.LOCALIZATION_GROUP_STAGE, CurrentChapter, CurrentStage + 1),
			LocalizationDefine.LOCALIZATION_KEY_STAGE_INTRODUCTION);
	}

	void GoStage()
	{
		ChapterRepository.ChapterData chapter = GetChapterData(CurrentChapter);

		ChapterRepository.StageData stage = chapter.Stages[CurrentStage];

		//GameCore.ChangeScene ("GamePlay", GetLoadList(stage));
		GameCore.ChangeScene (stage.ScemeName, GetLoadList(stage));
	}

	string[] GetLoadList(ChapterRepository.StageData _stage)
	{
		string[] loadList = new string[] {
			(string)GameCore.GetParameter(ParamGroup.GROUP_CACHE, CacheParam.GET_SCENE_PATH, _stage.ScemeName/*"GamePlay"*/),
			(string)GameCore.GetParameter(ParamGroup.GROUP_CACHE, CacheParam.GET_INSTANT_RESOURCE_INPUT_PATH),
			(string)GameCore.GetParameter(ParamGroup.GROUP_CACHE, CacheParam.GET_INSTANT_RESOURCE_UI_PATH),
			(string)GameCore.GetParameter(ParamGroup.GROUP_CACHE, CacheParam.GET_CONTAINER_PATH),
			(string)GameCore.GetParameter(ParamGroup.GROUP_CACHE, CacheParam.GET_BULLET_PATH),
			(string)GameCore.GetParameter(ParamGroup.GROUP_CACHE, CacheParam.GET_ABILITY_PATH),
		};

		loadList = loadList.Concat (GetCharacterDownLoadList (GameCore.UserProfile.MainCharacterID)).ToArray ();

		if (_stage.characterIDs != null && _stage.characterIDs.Length != 0) 
		{
			string[] stageCharacters = _stage.characterIDs
				.Select (_ => GetCharacterDownLoadList (_))
				.Aggregate ((_pre, _cur) => _pre = _pre.Concat (_cur).ToArray ());

			loadList = loadList.Concat (stageCharacters).ToArray ();
		}

		return loadList;
	}

	string[] GetCharacterDownLoadList(uint _characterID)
	{
		CharacterRepository.CharacterData character = (CharacterRepository.CharacterData)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY, RepositoryParam.GET_CHARACTER_DATA, _characterID);

		WeaponDataRepository.WeaponData weapon = (WeaponDataRepository.WeaponData)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY, RepositoryParam.GET_WEAPON_DATA, character.weaponID);

		return new string[] { 
			(string)GameCore.GetParameter(ParamGroup.GROUP_CACHE, CacheParam.GET_CHARACTER_PATH, _characterID),
			(string)GameCore.GetParameter(ParamGroup.GROUP_CACHE, CacheParam.GET_WEAPON_PATH, weapon.ModelID)
		};
	}
}
