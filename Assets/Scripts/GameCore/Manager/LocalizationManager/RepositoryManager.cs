using UnityEngine;
using System.Collections;

public sealed partial class RepositoryManager : CommandBehaviour, IParam, IRegister 
{
	// Use this for initialization
	void Start () 
	{
	}

	public void OnRegister ()
	{
		GameCore.RegisterCommand (CommandGroup.GROUP_REPOSITORY, this);	

		GameCore.RegisterParam (ParamGroup.GROUP_REPOSITORY, this);

		InitialRequestQueue ();

		InitialLocalizationData ();

		InitialCharacterData ();
	}

	public void OnUnRegister ()
	{
		ReleasCharacterData ();

		ReleaseLocalizationData ();

		ReleaseRequestQueue ();

		GameCore.UnRegisterCommand (CommandGroup.GROUP_REPOSITORY);

		GameCore.UnRegisterParam (ParamGroup.GROUP_REPOSITORY);
	}

	protected override void BatchCommand (uint _inst, params System.Object[] _params)
	{
		switch (_inst) 
		{
		case RepositoryInst.LOAD_LOCALIZATION:
			LoadLocalization ((LocalizationRepository)_params[0]);
			break;

		case RepositoryInst.LOAD_CHARACTER_DATA:
			LoadCharacterData ((CharacterRepository)_params [0]);
			break;
		}
	}

	public System.Object GetParameter (uint _inst, params System.Object[] _params)
	{
		System.Object output = default(System.Object);

		switch (_inst) 
		{
		case RepositoryParam.GET_LOCALIZATION:
			output = GetLocalization ((string)_params[0], (int)_params[1]);
			break;

		case RepositoryParam.GET_CHARACTER_DATA:
			output = GetCharacterData ((uint)_params[0]);
			break;
		}

		return output;
	}
}
