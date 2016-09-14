using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public sealed partial class LobbyBase
{
	[System.Serializable]
	public class MenuButtons
	{
		public UnityEngine.UI.Button Character;
		public UnityEngine.UI.Button Single;
		public UnityEngine.UI.Button Multi;
		public UnityEngine.UI.Button SpecialThanks;
		public UnityEngine.UI.Button Config;
	}

	[SerializeField]
	MenuButtons mMenuBtns;

	void InitialMenuBtns()
	{
		if (mMenuBtns == null)
			return;

		if (mMenuBtns.Character != null) 
		{
			mMenuBtns.Character.OnClickAsObservable ()
				.Subscribe (_ => GameCore.SendCommand(CommandGroup.GROUP_LOBBY, LobbyInst.ENTER_PAGE_CHARACTER));
		}

		if (mMenuBtns.Single != null) 
		{
			mMenuBtns.Single.OnClickAsObservable ()
				.Subscribe (_ => GameCore.SendCommand(CommandGroup.GROUP_LOBBY, LobbyInst.ENTER_PAGE_SINGLE));
		}
	}
}
