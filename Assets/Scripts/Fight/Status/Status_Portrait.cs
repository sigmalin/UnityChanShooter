using UnityEngine;
using System.Collections;
using UniRx;

public partial class Status
{
	[SerializeField]
	UnityEngine.UI.RawImage mPortrait;

	[SerializeField]
	Animator mAnim;

	Blood mBlood;

	uint mMainCameraID;

	System.IDisposable mLifeDisposable;
	System.IDisposable mDamgeDisposable;

	void InitialPortrait(WeaponManager.WeaponActor _actor)
	{
		ReleasePortrait ();

		mPortrait.texture = (Texture)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_PORTRAIT, _actor.CharacterID, ProtraitDefine.PROTRAIT_KEY_NORAML, false);

		if (mBlood == null) 
		{
			mMainCameraID = (uint)GameCore.GetParameter (ParamGroup.GROUP_CAMERA, CameraParam.MAIN_CAMERA);
			mBlood = new Blood ();
			GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.SET_CAMERA_POST_EFFECT, mMainCameraID, mBlood);
		}

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

			if (mBlood != null) 
				mBlood.SetBloodWeight((1 - _) * (1 - _));
		});

		_actor.Life.Pairwise ()
			.Where (_ => _.Current < _.Previous)
			.Subscribe (_ => {
				mAnim.ResetTrigger(GameCore.AnimID_triggerShock);
				mAnim.SetTrigger(GameCore.AnimID_triggerShock);
			});
	}

	void ReleasePortrait()
	{		
		mPortrait.texture = null;

		if (mBlood != null) 
		{
			GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.REMOVE_CAMERA_POST_EFFECT, mMainCameraID, mBlood);
			mBlood = null;
		}
	}
}
