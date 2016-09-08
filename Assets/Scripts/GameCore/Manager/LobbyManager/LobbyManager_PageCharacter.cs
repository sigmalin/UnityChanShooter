using UnityEngine;
using System.Collections;

public sealed partial class LobbyManager
{
	[SerializeField]
	GameObject mPageCharacter;

	void ShowPageCharacter()
	{
		if (mPageCharacter != null && mPageCharacter.activeSelf == false)
			GameCore.PushInterface (mPageCharacter.GetComponent<IUserInterface> ());

		UpdatePageCharacter ();
	}

	void HidePageCharacter()
	{
		if (mPageCharacter != null && mPageCharacter.activeSelf == true)
			GameCore.PopPopInterface (mPageCharacter.GetComponent<IUserInterface> ());
	}

	void UpdatePageCharacter()
	{
		IUserInterface ui = mPageCharacter.GetComponent<IUserInterface> ();
		if (ui == null)
			return;

		ui.SendCommand (Page_Character.InstSet.CREATE_ANALYSIS_GRAPHIC);
	}
}
