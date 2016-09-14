using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public sealed partial class LobbyManager
{
	Stack<LobbyBehaviour> mLobbyUIs;

	void InitialStack()
	{
		ReleaseStack ();

		mLobbyUIs = new Stack<LobbyBehaviour> ();
	}

	void ReleaseStack()
	{
		if (mLobbyUIs != null) 
		{
			while (mLobbyUIs.Count != 0) 
			{
				GameCore.PopInterface (mLobbyUIs.Pop().Ui);
			}

			mLobbyUIs = null;
		}
	}

	void OpenInterface(LobbyBehaviour _lobbyUi)
	{
		if (mLobbyUIs == null || _lobbyUi == null)
			return;

		LobbyBehaviour res = mLobbyUIs.FirstOrDefault (_ => _ == _lobbyUi);
		if (res != default(LobbyBehaviour))
			return;

		mLobbyUIs.Push (_lobbyUi);

		GameCore.PushInterface (_lobbyUi.Ui);
	}

	void CloseInterface(LobbyBehaviour _lobbyUi)
	{
		if (mLobbyUIs == null || _lobbyUi == null)
			return;

		if (mLobbyUIs.Count == 0)
			return;

		if (mLobbyUIs.Peek () == _lobbyUi) 
		{
			mLobbyUIs.Pop ();

			GameCore.PopInterface (_lobbyUi.Ui);
		}
	}

	void NormalizationLobbyUI()
	{
		if (mLobbyUIs == null)
			return;

		LobbyBehaviour[] lobbyUIs = mLobbyUIs.ToArray ();

		lobbyUIs.ToObservable ()
			.TakeWhile (_ => _ != NormalizationUI)
			.Subscribe (_ => CloseInterface (_));

		if (mLobbyUIs.Count == 0)
			ShowLobbyBase ();
	}

	void BatchOperation(uint _inst, params System.Object[] _params)
	{
		if (mLobbyUIs == null)
			return;
		
		if (mLobbyUIs.Count == 0)
			return;

		mLobbyUIs.Peek ().Operation (_inst, _params);
	}
}
