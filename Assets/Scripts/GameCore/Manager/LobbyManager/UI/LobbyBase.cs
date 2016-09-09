using UnityEngine;
using System.Collections;
using UniRx;

public sealed partial class LobbyBase : LobbyBehaviour
{
	public class InstSet
	{
		public const uint UPDATE_PORTRAIT = 0;

		public const uint SET_STATE_NORMAL = 1;

		public const uint SET_STATE_CHARACTER = 2;
	}

	// Use this for initialization
	void Start () 
	{
		InitialMenuBtns ();
	}

	public override bool HandleInput ()
	{
		return true;
	}

	public override void SendCommand(uint _inst, params System.Object[] _params)
	{
		switch(_inst)
		{
		case InstSet.UPDATE_PORTRAIT:
			UpdatePortrait ();
			break;

		case InstSet.SET_STATE_NORMAL:
			SetLobbyState (LOBBY_STATE_NORMAL);
			break;

		case InstSet.SET_STATE_CHARACTER:
			SetLobbyState (LOBBY_STATE_CHARACTER);
			break;
		}
	}
}
