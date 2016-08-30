using UnityEngine;
using System.Collections;

public sealed class SystemManager : CommandBehaviour, IRegister
{
	// Use this for initialization
	void Start ()
	{
		InitialRequestQueue ();
	}

	public void OnRegister ()
	{
		GameCore.RegisterCommand (CommandGroup.GROUP_SYSTEM, this);
	}

	public void OnUnRegister ()
	{
		GameCore.UnRegisterCommand (CommandGroup.GROUP_SYSTEM);
	}

	//public void ExecCommand(uint _inst, params System.Object[] _params)
	protected override void BatchCommand (uint _inst, params System.Object[] _params)
	{
		switch(_inst)
		{
		case SystemInst.GAME_QUIT:
			Application.Quit ();
			break;
		}
	}
}
