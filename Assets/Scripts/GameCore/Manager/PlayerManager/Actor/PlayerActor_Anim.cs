using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;

public partial class PlayerActor
{
	public const string PLAYER_DAMAGE = "Damage";

	List<System.IDisposable> mStateMachineDisposableList = null;

	void InitStateMachineBehaviour(Animator _anim)
	{
		ReleaseStateMachineBehaviour ();

		if (_anim == null)
			return;

		if (mStateMachineDisposableList == null)
			mStateMachineDisposableList = new List<System.IDisposable> ();

		ObservableStateMachineTrigger[] stateMachineObservable = _anim.GetBehaviours<ObservableStateMachineTrigger> ();

		for (int Indx = 0; Indx < stateMachineObservable.Length; ++Indx) 
		{
			// stun
			mStateMachineDisposableList.Add (
				stateMachineObservable [Indx].OnStateEnterAsObservable ()
					.Where (_ => _.StateInfo.IsName (PLAYER_DAMAGE))
					.Subscribe (_ => 
					{
						// Lock Ai
						GameCore.SendCommand (CommandGroup.GROUP_AI, AiInst.FREEZE_AI, ActorID, true);
						// Lock Actor
						GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_STUN, ActorID, true);
						// set Invicible
						GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.SET_INVINCIBLE, ActorID, true);
					}
				)
			);

			mStateMachineDisposableList.Add (
				stateMachineObservable [Indx].OnStateExitAsObservable ()
				.Where (_ => _.StateInfo.IsName (PLAYER_DAMAGE))
				.Subscribe (_ => 
					{
						// Lock Ai
						GameCore.SendCommand (CommandGroup.GROUP_AI, AiInst.FREEZE_AI, ActorID, false);
						// Lock Actor
						GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_STUN, ActorID, false);
						// set Invicible
						GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.SET_INVINCIBLE, ActorID, false);
					}
				)
			);
		}
	}

	void ReleaseStateMachineBehaviour()
	{
		if (mStateMachineDisposableList == null)
			return;

		mStateMachineDisposableList.ToObservable ()
			.Where (_ => _ != null)
			.Subscribe (_ => _.Dispose ());

		mStateMachineDisposableList.Clear ();
	}
}
