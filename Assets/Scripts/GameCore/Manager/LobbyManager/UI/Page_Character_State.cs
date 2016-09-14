using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public sealed partial class Page_Character
{
	[System.Serializable]
	public struct StateButton
	{
		public UnityEngine.UI.Button Button;
		public UnityEngine.UI.Text Context;
		public System.IDisposable Disposable;
	}

	[System.Serializable]
	public struct StateButtonList
	{
		public StateButton mNameBtn;
		public StateButton mWeaponBtn;
		public StateButton mAbility1Btn;
		public StateButton mAbility2Btn;
		public StateButton mAbility3Btn;
		public StateButton mSkillBtn;
	}

	[SerializeField]
	StateButtonList mStateButtonList;

	void InitialStateButton()
	{
		CharacterRepository.CharacterData data = (CharacterRepository.CharacterData)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY,
			RepositoryParam.GET_CHARACTER_DATA,
			GameCore.UserProfile.MainCharacterID);

		SetStateButton(ref mStateButtonList.mAbility1Btn, () => GameCore.SendCommand(CommandGroup.GROUP_LOBBY, LobbyInst.SHOW_LOBBY_DIALOG,
			"Ability", (int)data.ability1ID, "Dialog-Ability", (int)data.ability1ID));

		SetStateButton(ref mStateButtonList.mAbility2Btn, () => GameCore.SendCommand(CommandGroup.GROUP_LOBBY, LobbyInst.SHOW_LOBBY_DIALOG,
			"Ability", (int)data.ability2ID, "Dialog-Ability", (int)data.ability2ID));

		SetStateButton(ref mStateButtonList.mAbility3Btn, () => GameCore.SendCommand(CommandGroup.GROUP_LOBBY, LobbyInst.SHOW_LOBBY_DIALOG,
			"Ability", (int)data.ability3ID, "Dialog-Ability", (int)data.ability3ID));

		SetStateButton(ref mStateButtonList.mSkillBtn, () => GameCore.SendCommand(CommandGroup.GROUP_LOBBY, LobbyInst.SHOW_LOBBY_DIALOG,
			"Skill", (int)data.skillID, "Dialog-Skill", (int)data.skillID));


		LocalizationStateButton ();
	}

	void LocalizationStateButton()
	{
		CharacterRepository.CharacterData data = (CharacterRepository.CharacterData)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY,
			RepositoryParam.GET_CHARACTER_DATA,
			GameCore.UserProfile.MainCharacterID);

		mStateButtonList.mNameBtn.Context.text = (string)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY,
			RepositoryParam.GET_LOCALIZATION,
			"Name", (int)data.ID);

		mStateButtonList.mWeaponBtn.Context.text = (string)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY,
			RepositoryParam.GET_LOCALIZATION,
			"Weapon", (int)data.weaponID);

		mStateButtonList.mAbility1Btn.Context.text = (string)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY,
			RepositoryParam.GET_LOCALIZATION,
			"Ability", (int)data.ability1ID);

		mStateButtonList.mAbility2Btn.Context.text = (string)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY,
			RepositoryParam.GET_LOCALIZATION,
			"Ability", (int)data.ability2ID);

		mStateButtonList.mAbility3Btn.Context.text = (string)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY,
			RepositoryParam.GET_LOCALIZATION,
			"Ability", (int)data.ability3ID);

		mStateButtonList.mSkillBtn.Context.text = (string)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY,
			RepositoryParam.GET_LOCALIZATION,
			"Skill", (int)data.skillID);
	}

	public void SetStateButton(ref StateButton _btn, System.Action _callback)
	{
		if (_btn.Disposable != null)
			_btn.Disposable.Dispose ();

		_btn.Disposable = _btn.Button.OnClickAsObservable ()
			.Subscribe (_ => _callback());
	}
}
