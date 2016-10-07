using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public sealed partial class Page_Single
{
	[System.Serializable]
	public struct ChapterContest
	{
		public UnityEngine.UI.Button mButton;
		public UnityEngine.UI.RawImage mImage;
		public UnityEngine.UI.Text mText;
	}


	[SerializeField]
	ChapterContest[] mContests;

	int mSelChapter;
	int CurrentChapter { get { return mSelChapter; } set { mSelChapter = value; } }

	int mSelStage;
	int CurrentStage { get { return mSelStage; } set { mSelStage = value; } }

	void InitialContest()
	{
		mContests.ToObservable()
			.Where (_ => IsInvalidContest(ref _) == false)
			.Select ((_contest, _index) => _contest.mButton.OnClickAsObservable().Subscribe(_ => SelectStage(_index)))
			.Subscribe(_ => {});
	}

	bool IsInvalidContest(ref ChapterContest _contest)
	{
		return _contest.mButton == null || _contest.mImage == null || _contest.mText == null;
	}

	void SelectStage(int _stageIndx)
	{
		mSelStage = _stageIndx;

		GameCore.SendCommand(CommandGroup.GROUP_LOBBY, LobbyInst.SWITCH_TO_PAGE_SINGLE_STAGE, mSelChapter, _stageIndx);
	}

	void UpdateContest(int _chapterID)
	{
		mSelChapter = _chapterID;

		ChapterRepository.ChapterData curChapter = GetChapterData (_chapterID);

		mContests.ToObservable ()
			.Where (_ => IsInvalidContest (ref _) == false)
			.Select((_contest, _stageIndex) => {
				_contest.mButton.gameObject.SetActive(_stageIndex < curChapter.Stages.Length);
				return new {contest = _contest, stage = _stageIndex + 1};
			})
			.Where (_ => _.stage <= curChapter.Stages.Length)
			.Select (_ => {				
				_.contest.mImage.texture = (Texture)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_STAGE_IMAGE, _chapterID, _.stage, false);
				return _;
			})
			.Where(_ => _.contest.mImage.texture == null)
			.Subscribe(_ =>
				{
					string[] loadList = new string[] {
						(string)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_STAGE_IMAGE_PATH, _chapterID)
					};

					GameCore.AdditionalLoad (
						loadList,
						() => 
						{
							if(_.contest.mImage != null)
								_.contest.mImage.texture = (Texture)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_STAGE_IMAGE, _chapterID, _.stage, false);
						},
						true
					);
				});

		SetLocalizationStage ();
	}

	void SetLocalizationContest()
	{
		ChapterRepository.ChapterData curChapter = GetChapterData (mSelChapter);
		if (curChapter.Stages == null)
			return;

		mContests.ToObservable ()
			.Where (_ => IsInvalidContest (ref _) == false)
			.Select ((_contest, _stageIndex) => {
				return new {contest = _contest, stage = _stageIndex + 1};		
			})
			.Where (_ => _.stage <= curChapter.Stages.Length)
			.Subscribe (_ => {	
				_.contest.mText.text = (string)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY, RepositoryParam.GET_LOCALIZATION,
					string.Format (LocalizationDefine.LOCALIZATION_GROUP_STAGE, mSelChapter, _.stage),
					LocalizationDefine.LOCALIZATION_KEY_STAGE_NAME);
			});
	}
}
