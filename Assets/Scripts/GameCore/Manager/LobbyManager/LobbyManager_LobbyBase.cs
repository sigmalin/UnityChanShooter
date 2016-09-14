using UnityEngine;
using System.Collections;

public sealed partial class LobbyManager
{
	[SerializeField]
	LobbyBehaviour mLobbyBase;
	public LobbyBehaviour NormalizationUI { get { return mLobbyBase; } }

	void DestroyLobbyBase()
	{
		if (mLobbyBase != null) 
		{
			GameObject.Destroy (mLobbyBase.gameObject);
			mLobbyBase = null;
		}
	}

	void ShowLobbyBase()
	{
		OpenInterface(mLobbyBase);

		UpdateLobbyBasePortrait ();
	}

	void HideLobbyBase()
	{
		CloseInterface (mLobbyBase);
	}

	void UpdateLobbyBasePortrait()
	{
		if (mLobbyBase == null)
			return;

		mLobbyBase.LobbyOrder (LobbyBase.OrderList.UPDATE_PORTRAIT);
	}

	void SetLobbyBaseStateNormal()
	{
		if (mLobbyBase == null)
			return;

		mLobbyBase.LobbyOrder (LobbyBase.OrderList.SET_STATE_NORMAL);
	}

	void SetLobbyBaseStateCharacter()
	{
		if (mLobbyBase == null)
			return;
		
		mLobbyBase.LobbyOrder (LobbyBase.OrderList.SET_STATE_CHARACTER);
	}

	void SetLobbyBaseStateSingle()
	{
		if (mLobbyBase == null)
			return;

		mLobbyBase.LobbyOrder (LobbyBase.OrderList.SET_STATE_SINGLE);
	}
}
