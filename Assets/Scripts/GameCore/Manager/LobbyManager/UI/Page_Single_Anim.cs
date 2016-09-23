using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public sealed partial class Page_Single
{
	[SerializeField]
	Animator mAnim;

	// Use this for initialization
	void InitialAnim () 
	{
		if (mAnim == null)
			return;

		mAnim.GetBehaviour<ObservableStateMachineTrigger> ()
			.OnStateEnterAsObservable ()
			.Where (_ => _.StateInfo.IsName ("Exit"))
			.Subscribe (_ => GameCore.SendCommand (CommandGroup.GROUP_LOBBY, LobbyInst.EXIT_PAGE_SINGLE));
	}

	void PlayPageSceneOut()
	{
		if (mAnim == null)
			return;

		mAnim.SetTrigger (GameCore.AnimID_triggerExit);
	}
}
