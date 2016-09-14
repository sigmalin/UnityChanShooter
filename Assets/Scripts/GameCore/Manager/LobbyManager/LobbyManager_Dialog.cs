using UnityEngine;
using System.Collections;

public sealed partial class LobbyManager
{
	[SerializeField]
	LobbyBehaviour mLobbyDialog;

	void DestroyDialog()
	{
		if (mLobbyDialog != null) 
		{
			GameObject.Destroy (mLobbyDialog.gameObject);
			mLobbyDialog = null;
		}
	}

	void ShowDialog()
	{
		OpenInterface (mLobbyDialog);
	}

	void HideDialog()
	{
		CloseInterface (mLobbyDialog);
	}
}
