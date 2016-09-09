using UnityEngine;
using System.Collections;

public sealed partial class LobbyManager
{
	[SerializeField]
	GameObject mLobbyBase;

	void ShowLobbyBase()
	{
		if (mLobbyBase != null && mLobbyBase.activeSelf == false)
			GameCore.PushInterface (mLobbyBase.GetComponent<IUserInterface> ());

		UpdateLobbyBasePortrait ();
	}

	void HideLobbyBase()
	{
		if (mLobbyBase != null && mLobbyBase.activeSelf == true)
			GameCore.PopPopInterface (mLobbyBase.GetComponent<IUserInterface> ());
	}

	void UpdateLobbyBasePortrait()
	{
		if (mLobbyBase == null)
			return;

		IUserInterface ui = mLobbyBase.GetComponent<IUserInterface> ();
		if (ui != null)
			ui.SendCommand (LobbyBase.InstSet.UPDATE_PORTRAIT);
	}

	void SetLobbyBaseStateNormal()
	{
		if (mLobbyBase == null)
			return;

		IUserInterface ui = mLobbyBase.GetComponent<IUserInterface> ();
		if (ui != null)
			ui.SendCommand (LobbyBase.InstSet.SET_STATE_NORMAL);
	}

	void SetLobbyBaseStateCharacter()
	{
		if (mLobbyBase == null)
			return;
		
		IUserInterface ui = mLobbyBase.GetComponent<IUserInterface> ();
		if (ui != null)
			ui.SendCommand (LobbyBase.InstSet.SET_STATE_CHARACTER);
	}
}
