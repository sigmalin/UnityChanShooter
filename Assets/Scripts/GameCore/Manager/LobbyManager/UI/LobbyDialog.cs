using UnityEngine;
using System.Collections;

public sealed partial class LobbyDialog : LobbyBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		InitialDialogMask ();
	}

	void OnDestroy()
	{
		ReleaseDialogStack ();
	}

	public override void Localization()
	{
		base.Localization ();

		UpdateDialog ();
	}

	public override void Operation(uint _inst, params System.Object[] _params)
	{
		switch(_inst)
		{
		case LobbyInst.SHOW_LOBBY_DIALOG:
			AddDialogData ((string)_params [0], (int)_params [1], (string)_params [2], (int)_params [3]);
			break;
		}
	}
}
