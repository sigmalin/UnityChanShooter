using UnityEngine;
using System.Collections;
using UniRx;

public sealed partial class Page_Single 
{
	[SerializeField]
	UnityEngine.UI.Button mMask;

	System.IDisposable mMaskOnClickDisposable;

	void ClearDisposable()
	{
		if (mMaskOnClickDisposable != null) 
		{
			mMaskOnClickDisposable.Dispose ();

			mMaskOnClickDisposable = null;
		}
	}

	void OnClickMaskWhenShowList()
	{
		ClearDisposable ();

		if (mMask != null) 
		{
			mMaskOnClickDisposable = mMask.OnClickAsObservable()
				.Subscribe(_ => PlayPageSceneOut());
		}
	}

	void OnClickMaskWhenShowStage()
	{
		ClearDisposable ();

		if (mMask != null) 
		{
			mMaskOnClickDisposable = mMask.OnClickAsObservable()
				.Subscribe(_ => GameCore.SendCommand(CommandGroup.GROUP_LOBBY, LobbyInst.SWITCH_TO_PAGE_SINGLE_LIST));
		}
	}
}
