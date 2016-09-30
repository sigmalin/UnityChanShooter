using UnityEngine;
using System.Collections;
using UniRx;

public partial class Status
{
	[SerializeField]
	UnityEngine.UI.RawImage mPortrait;

	void InitialPortrait(WeaponManager.WeaponActor _actor)
	{
		mPortrait.texture = (Texture)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_PORTRAIT, _actor.CharacterID, ProtraitDefine.PROTRAIT_KEY_NORAML, false);

		_actor.Life.Subscribe (_ => {

			if(_ < 0.25f)
			{
				mPortrait.texture = (Texture)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_PORTRAIT, _actor.CharacterID, ProtraitDefine.PROTRAIT_KEY_PAIN, false);
			}
			else if(_ < 0.5f)
			{
				mPortrait.texture = (Texture)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_PORTRAIT, _actor.CharacterID, ProtraitDefine.PROTRAIT_KEY_SERIOUS, false);
			}

			mPortrait.color = new Color(1f, _, _);
		});
	}
}
