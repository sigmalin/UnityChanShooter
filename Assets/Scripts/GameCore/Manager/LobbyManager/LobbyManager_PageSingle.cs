using UnityEngine;
using System.Collections;

public sealed partial class LobbyManager 
{
	[SerializeField]
	LobbyBehaviour mPageSingle;

	void DestroyPageSingle()
	{
		if (mPageSingle != null) 
		{
			GameObject.Destroy (mPageSingle.gameObject);
			mPageSingle = null;
		}
	}

	void ShowPageSingle()
	{
		NormalizationLobbyUI ();

		OpenInterface (mPageSingle);

		mPageSingle.LobbyOrder (Page_Single.OrderList.UPDATE_CHAPTER_LIST);
	}

	void HidePageSingle()
	{
		CloseInterface (mPageSingle);
	}
}
