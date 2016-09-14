using UnityEngine;
using System.Collections;

public sealed partial class Page_Character : LobbyBehaviour 
{
	public class OrderList
	{
		public const uint UPDATE_CHARACTER_LIST = 0;

		public const uint UPDATE_CHARACTER_STATE = 1;
	}

	void OnDestroy()
	{
		ReleaseItemPool ();
	}

	public override void Show(Transform _root)
	{
		base.Show (_root);

		InitialAnim ();
	}

	public override void Localization()
	{
		base.Localization ();

		LocalizationStateButton ();
	}

	public override void Operation(uint _inst, params System.Object[] _params)
	{
		switch(_inst)
		{
		case LobbyInst.SWITCH_TO_PAGE_CHARACTER_LIST:
			LobbyOrder (OrderList.UPDATE_CHARACTER_LIST);
			break;

		case LobbyInst.SWITCH_TO_PAGE_CHARACTER_STATE:
			LobbyOrder (OrderList.UPDATE_CHARACTER_STATE);
			break;
		}
	}

	public override void LobbyOrder(uint _order)
	{
		switch(_order)
		{
		case OrderList.UPDATE_CHARACTER_LIST:
			UpdateList ();
			SwitchPageTab (CHARACTER_PAGE_TAB_LIST);
			break;

		case OrderList.UPDATE_CHARACTER_STATE:
			CreateAnalysisGraphic ();
			SwitchPageTab (CHARACTER_PAGE_TAB_STATE);
			InitialStateButton ();
			break;
		}
	}
}
