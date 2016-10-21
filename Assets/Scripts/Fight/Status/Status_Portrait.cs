using UnityEngine;
using System.Collections;
using UniRx;

public partial class Status
{
	[SerializeField]
	UnityEngine.UI.RawImage mPortrait;

	[SerializeField]
	Animator mAnim;

	void InitialPortrait(WeaponManager.WeaponActor _actor)
	{
		mPortrait.texture = (Texture)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_PORTRAIT, _actor.CharacterID, ProtraitDefine.PROTRAIT_KEY_NORAML, false);

		_actor.Life.Subscribe (_ => {

			if(_ < 0.25f)
			{
				mPortrait.texture = (Texture)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_PORTRAIT, _actor.CharacterID, ProtraitDefine.PROTRAIT_KEY_PAIN, false);

				GameCore.SendCommand(CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_FACE, _actor.ActorID, ProtraitDefine.PROTRAIT_KEY_PAIN);
			}
			else if(_ < 0.5f)
			{
				mPortrait.texture = (Texture)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_PORTRAIT, _actor.CharacterID, ProtraitDefine.PROTRAIT_KEY_SERIOUS, false);

				GameCore.SendCommand(CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_FACE, _actor.ActorID, ProtraitDefine.PROTRAIT_KEY_SERIOUS);
			}
			else
			{
				mPortrait.texture = (Texture)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_PORTRAIT, _actor.CharacterID, ProtraitDefine.PROTRAIT_KEY_NORAML, false);

				GameCore.SendCommand(CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_FACE, _actor.ActorID, ProtraitDefine.PROTRAIT_KEY_NORAML);
			}

			mAnim.SetFloat(GameCore.AnimID_fLife, _); 
		});

		_actor.Life.Pairwise ()
			.Where (_ => _.Current < _.Previous)
			.Subscribe (_ => {
				mAnim.ResetTrigger(GameCore.AnimID_triggerShock);
				mAnim.SetTrigger(GameCore.AnimID_triggerShock);
			});
	}
}
