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

		InitialWeaponData ();

		InitialSpriteData ();

		InitialPostEffectData ();
	}

	public void OnUnRegister ()
	{
		ReleasePostEffectData ();

		ReleaseSpriteData ();

		ReleaseWeaponData ();

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

		case RepositoryInst.LOAD_CHAPTER_DATA:
			LoadChapterData((ChapterRepository)_params [0]);
			break;

		case RepositoryInst.LOAD_WEAPON_DATA:
			LoadWeaponData((WeaponDataRepository)_params [0]);
			break;

		case RepositoryInst.LOAD_SPRITE_DATA:
			LoadSpriteData((string)_params[0], (SpriteRepository)_params [1]);
			break;

		case RepositoryInst.RELEASE_SPRITE_DATA:
			ReleaseSpriteData((string)_params[0]);
			break;

		case RepositoryInst.LOAD_POST_EFFECT_DATA:
			LoadPostEffectData((PostEffectRepository)_params [0]);
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

		case RepositoryParam.GET_CHAPTER_DATA:
			output = GetChapterData ((int)_params[0]);
			break;

		case RepositoryParam.GET_ALL_CHAPTER_DATA:
			output = GetAllChapterData ();
			break;

		case RepositoryParam.GET_WEAPON_DATA:
			output = GetWeaponData ((uint)_params[0]);
			break;

		case RepositoryParam.GET_SPRITE_DATA:
			output = GetSpriteData ((string)_params[0], (char)_params[1]);
			break;

		case RepositoryParam.GET_POST_EFFECT_DATA:
			output = GetPostEffectData ((uint)_params[0]);
			break;
		}

		return output;
	}
}
