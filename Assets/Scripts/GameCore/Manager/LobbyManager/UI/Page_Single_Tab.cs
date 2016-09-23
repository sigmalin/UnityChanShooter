using UnityEngine;
using System.Collections;

public sealed partial class Page_Single 
{
	[System.Serializable]
	public class PageTab
	{
		public GameObject List;
		public GameObject Stage;
	}

	[SerializeField]
	PageTab mPageTab;

	const int SINGLE_PAGE_TAB_LIST = 0;
	const int SINGLE_PAGE_TAB_STAGE = 1;

	void SwitchPageTab(int _tab)
	{
		if (mPageTab.List != null)  mPageTab.List.SetActive (_tab == SINGLE_PAGE_TAB_LIST);
		if (mPageTab.Stage != null) mPageTab.Stage.SetActive (_tab == SINGLE_PAGE_TAB_STAGE);

		switch(_tab)
		{
		case SINGLE_PAGE_TAB_LIST:	
			OnClickMaskWhenShowList ();
			break;

		case SINGLE_PAGE_TAB_STAGE:
			OnClickMaskWhenShowStage ();
			break;
		}
	}
}
