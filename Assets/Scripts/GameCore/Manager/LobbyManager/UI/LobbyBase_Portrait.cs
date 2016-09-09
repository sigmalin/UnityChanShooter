using UnityEngine;
using System.Collections;

public sealed partial class LobbyBase
{
	[SerializeField]
	UnityEngine.UI.RawImage mPortrait;

	void UpdatePortrait()
	{
		if (mPortrait == null)
			return;

		mPortrait.texture = (Texture)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_PORTRAIT, GameCore.UserProfile.MainCharacterID, 1, false);
	}
}
