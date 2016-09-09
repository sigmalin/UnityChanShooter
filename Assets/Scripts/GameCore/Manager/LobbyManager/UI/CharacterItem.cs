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
				GameCore.SendCommand(CommandGroup.GROUP_LOBBY, LobbyInst.ENTER_PAGE_CHARACTER);
			});
	}

	public void Initial (params System.Object[] _params)
	{
		mCharacterID = (uint)_params[0];

		mPortrait.texture = (Texture)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_PORTRAIT, mCharacterID, 1, false);
	}
}
