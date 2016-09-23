using UnityEngine;
using System.Collections;
using System.Linq;

public sealed partial class Page_Single : LobbyBehaviour 
{
	public class OrderList
	{
		public const uint UPDATE_CHAPTER_LIST = 0;
	}

	// Use this for initialization
	void Start () 
	{
		InitialContest ();
	}

	void OnDestroy()
	{
		ReleaseItemPool ();
	}

	public override void Show(Transform _root)
	{
		base.Show (_root);

		InitialAnim ();

		InitialStage ();
	}

	public override void Localization()
	{
		base.Localization ();

		SetLocalizationContest ();

		SetLocalizationList ();

		SetLocalizationStage ();
	}

	public override void Operation(uint _inst, params System.Object[] _params)
	{
		switch (_inst) 
		{
		case LobbyInst.SWITCH_TO_PAGE_SINGLE_LIST:
			SwitchPageTab (SINGLE_PAGE_TAB_LIST);
			break;

		case LobbyInst.SWITCH_TO_PAGE_SINGLE_STAGE:
			SettingStage ((int)_params [0], (int)_params [1]);
			SwitchPageTab (SINGLE_PAGE_TAB_STAGE);
			break;

		case LobbyInst.SELECT_CHAPTER:
			UpdateContest ((int)_params[0]);
			break;
		}
	}

	public override void LobbyOrder(uint _order)
	{
		switch(_order)
		{
		case OrderList.UPDATE_CHAPTER_LIST:
			UpdateList ();
			UpdateContest (1);
			SwitchPageTab (SINGLE_PAGE_TAB_LIST);
			break;
		}
	}

	ChapterRepository.ChapterData GetChapterData(int _chapterID)
	{
		ChapterRepository.ChapterData[] chapters = (ChapterRepository.ChapterData[])GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY, RepositoryParam.GET_ALL_CHAPTER_DATA);

		return chapters.Where (_ => _.ChapterID == _chapterID).FirstOrDefault ();
	}
}
