using UnityEngine;
using System.Collections;

public sealed partial class LobbyManager : CommandBehaviour, IParam, IRegister
{
	public override void OnDestroy ()
	{
		base.OnDestroy ();

		DestroyLobbyBase ();
		DestroyPageCharacter ();
	} 

	public void OnRegister ()
	{
		InitialRequestQueue ();

		InitialStack ();

		ShowLobbyBase ();

		GameCore.RegisterCommand (CommandGroup.GROUP_LOBBY, this);	

		GameCore.RegisterParam (ParamGroup.GROUP_LOBBY, this);
	}

	public void OnUnRegister ()
	{
		HideLobbyBase ();

		ReleaseStack ();

		ReleaseRequestQueue ();

		GameCore.UnRegisterCommand (CommandGroup.GROUP_LOBBY);

		GameCore.UnRegisterParam (ParamGroup.GROUP_LOBBY);
	}

	protected override void BatchCommand (uint _inst, params System.Object[] _params)
	{
		switch(_inst)
		{
		case LobbyInst.UPDATE_MAIN_CHARACTER:
			UpdateLobbyBasePortrait ();
			break;

		case LobbyInst.SHOW_LOBBY_DIALOG:
			ShowDialog ();
			BatchOperation (_inst, _params);
			break;

		case LobbyInst.HIDE_LOBBY_DIALOG:
			HideDialog ();
			break;


		case LobbyInst.ENTER_PAGE_CHARACTER:
			ShowPageCharacter ();
			SetLobbyBaseStateCharacter ();
			break;

		case LobbyInst.EXIT_PAGE_CHARACTER:
			HidePageCharacter ();
			SetLobbyBaseStateNormal ();
			break;

		case LobbyInst.ENTER_PAGE_SINGLE:
			ShowPageSingle ();
			SetLobbyBaseStateCharacter ();
			break;

		case LobbyInst.EXIT_PAGE_SINGLE:
			HidePageSingle ();
			SetLobbyBaseStateNormal ();
			break;

		default:
			BatchOperation (_inst, _params);
			break;
		}
	}

	public System.Object GetParameter (uint _inst, params System.Object[] _params)
	{
		System.Object output = default(System.Object);

		//switch (_inst) 
		//{
		//}

		return output;
	}
}
