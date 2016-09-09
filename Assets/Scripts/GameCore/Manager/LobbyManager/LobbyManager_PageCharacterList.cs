using UnityEngine;
using System.Collections;

public sealed partial class LobbyManager
{
	[SerializeField]
	GameObject mPageCharacterList;

	void ShowPageCharacterList()
	{
		if (mPageCharacterList != null && mPageCharacterList.activeSelf == false)
			GameCore.PushInterface (mPageCharacterList.GetComponent<IUserInterface> ());

		UpdatePageCharacterList ();
	}

	void HidePageCharacterList()
	{
		if (mPageCharacterList != null && mPageCharacterList.activeSelf == true)
			GameCore.PopPopInterface (mPageCharacterList.GetComponent<IUserInterface> ());
	}

	void UpdatePageCharacterList()
	{
		IUserInterface ui = mPageCharacterList.GetComponent<IUserInterface> ();
		if (ui == null)
			return;

		ui.SendCommand (Page_CharacterList.InstSet.UPDATE_CHARACTER_LIST);
	}
}
