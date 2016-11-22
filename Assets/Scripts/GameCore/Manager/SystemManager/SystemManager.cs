using UnityEngine;
using System.Collections;

public sealed partial class SystemManager : CommandBehaviour, IRegister
{
	public void OnRegister ()
	{
		GameCore.RegisterCommand (CommandGroup.GROUP_SYSTEM, this);

		InitialRequestQueue ();

		InitialAudio ();

		InitialFPS ();
	}

	public void OnUnRegister ()
	{
		ReleaseAudio ();

		ReleaseRequestQueue ();

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

		case SystemInst.PLAY_BGM:
			PlayBGM ((string)_params[0], (bool)_params[1]);
			break;

		case SystemInst.SHOW_FPS:
			IsShowFPS.Value = ((bool)_params[0]);
			break;
		}
	}
}
