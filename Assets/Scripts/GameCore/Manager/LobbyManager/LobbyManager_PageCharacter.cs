using UnityEngine;
using System.Collections;

public sealed partial class LobbyManager
{
	[SerializeField]
	LobbyBehaviour mPageCharacter;

	void DestroyPageCharacter()
	{
		if (mPageCharacter != null) 
		{
			GameObject.Destroy (mPageCharacter.gameObject);
			mPageCharacter = null;
		}
	}

	void ShowPageCharacter()
	{
		NormalizationLobbyUI ();

		OpenInterface (mPageCharacter);

		mPageCharacter.LobbyOrder (Page_Character.OrderList.UPDATE_CHARACTER_LIST);
	}

	void HidePageCharacter()
	{
		CloseInterface (mPageCharacter);
	}
}
