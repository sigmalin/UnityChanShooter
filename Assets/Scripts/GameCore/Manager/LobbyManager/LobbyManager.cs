using UnityEngine;
using System.Collections;

public sealed partial class LobbyManager : CommandBehaviour, IParam, IRegister
{
	// Use this for initialization
	void Start () 
	{
	
	}

	public void OnRegister ()
	{
		InitialRequestQueue ();

		ShowLobbyBase ();

		GameCore.RegisterCommand (CommandGroup.GROUP_LOBBY, this);	

		GameCore.RegisterParam (ParamGroup.GROUP_LOBBY, this);
	}

	public void OnUnRegister ()
	{
		HideLobbyBase ();

		HideLobbyBase ();

		ReleaseRequestQueue ();

		GameCore.UnRegisterCommand (CommandGroup.GROUP_LOBBY);

		GameCore.UnRegisterParam (ParamGroup.GROUP_LOBBY);
	}

	protected override void BatchCommand (uint _inst, params System.Object[] _params)
	{
		switch(_inst)
		{
		case LobbyInst.ENTER_PAGE_CHARACTER:
			SetLobbyBaseStateCharacter ();
			ShowPageCharacter ();
			break;

		case LobbyInst.EXIT_PAGE_CHARACTER:
			SetLobbyBaseStateNormal ();
			HidePageCharacter ();
			break;
		}
	}

	public System.Object GetParameter (uint _inst, params System.Object[] _params)
	{
		System.Object output = default(System.Object);

		switch (_inst) 
		{
		}

		return output;
	}
}
