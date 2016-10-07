using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class CharacterItem : MonoBehaviour, IItem 
{
	[SerializeField]
	UnityEngine.UI.RawImage mPortrait;

	[SerializeField]
	UnityEngine.UI.Button mButton;

	uint mCharacterID;

	// Use this for initialization
	void Start () 
	{
		mButton.OnClickAsObservable ()
			.Subscribe (_ => {
				GameCore.UserProfile.MainCharacterID = mCharacterID;
				GameCore.SendCommand(CommandGroup.GROUP_LOBBY, LobbyInst.UPDATE_MAIN_CHARACTER);
				GameCore.SendCommand(CommandGroup.GROUP_LOBBY, LobbyInst.SWITCH_TO_PAGE_CHARACTER_STATE);
			});
	}

	public void Initial (params System.Object[] _params)
	{
		mCharacterID = (uint)_params[0];

		mPortrait.texture = (Texture)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_PORTRAIT, mCharacterID, ProtraitDefine.PROTRAIT_KEY_NORAML, false);

		if (mButton != null)
			mButton.interactable = true;
	}

	public void Release ()
	{
	}

	public void SetReactiveProperty(ReadOnlyReactiveProperty<float> _reactiveProperty)
	{
	}
}
