using UnityEngine;
using System.Collections;
using UniRx;

public sealed partial class LobbyBase : LobbyBehaviour
{
	public class OrderList
	{
		public const uint UPDATE_PORTRAIT = 0;

		public const uint SET_STATE_NORMAL = 1;

		public const uint SET_STATE_CHARACTER = 2;

		public const uint SET_STATE_SINGLE = 3;
	}

	// Use this for initialization
	void Start () 
	{
		InitialMenuBtns ();
	}

	public override void LobbyOrder(uint _order)
	{
		switch(_order)
		{
		case OrderList.UPDATE_PORTRAIT:
			UpdatePortrait ();
			break;

		case OrderList.SET_STATE_NORMAL:
			SetLobbyState (LOBBY_STATE_NORMAL);
			break;

		case OrderList.SET_STATE_CHARACTER:
			SetLobbyState (LOBBY_STATE_CHARACTER);
			break;

		case OrderList.SET_STATE_SINGLE:
			SetLobbyState (LOBBY_STATE_SINGLE);
			break;
		}
	}
}
