using UnityEngine;
using System.Collections;

public sealed partial class Page_Character 
{
	[System.Serializable]
	public class PageTab
	{
		public GameObject List;
		public GameObject State;
	}

	[SerializeField]
	PageTab mPageTab;

	const int CHARACTER_PAGE_TAB_LIST = 0;
	const int CHARACTER_PAGE_TAB_STATE = 1;

	void SwitchPageTab(int _tab)
	{
		if (mPageTab.List != null)  mPageTab.List.SetActive (_tab == CHARACTER_PAGE_TAB_LIST);
		if (mPageTab.State != null) mPageTab.State.SetActive (_tab == CHARACTER_PAGE_TAB_STATE);

		switch(_tab)
		{
		case CHARACTER_PAGE_TAB_LIST:	
			OnClickMaskWhenShowList ();
			break;

		case CHARACTER_PAGE_TAB_STATE:
			OnClickMaskWhenShowState ();
			break;
		}
	}
}
