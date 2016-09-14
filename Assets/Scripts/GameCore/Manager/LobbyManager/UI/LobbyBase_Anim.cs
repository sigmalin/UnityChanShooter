using UnityEngine;
using System.Collections;

public sealed partial class LobbyBase
{
	const int LOBBY_STATE_NORMAL = 0;

	const int LOBBY_STATE_CHARACTER = 1;

	const int LOBBY_STATE_SINGLE = 2;

	[SerializeField]
	Animator mAnim = null;

	void SetLobbyState(int _state)
	{
		if (mAnim != null)
			mAnim.SetInteger (GameCore.AnimID_iLobbyState, _state);
	}
}
