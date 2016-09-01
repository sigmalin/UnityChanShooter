using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UniRx.Triggers;

public sealed class Flow_Logo : FlowBehaviour 
{
	public override void Enter()
	{
		base.Enter ();

		WaitClick2LoadVersionList ();
	}

	public override void Exit ()
	{
		GameCore.ChangeScene ("Scene/Lobby");
	}

	public override void Event (uint _eventID)
	{
		switch (_eventID) 
		{
		case FlowEvent.VERSION_VERFITY_COMPLETED:
			GameCore.SetFlow (null);
			break;
		}
	}

	void WaitClick2LoadVersionList()
	{
		IObservable<Unit> stream = this.UpdateAsObservable ();

		stream.Where(_ => Input.GetMouseButtonDown(0))
			.TakeUntil(stream.Where(_ => Input.GetMouseButtonUp(0)))
			.Subscribe 
			( 
				_ => {},
				_ex => Debug.Log(_ex),
				() => GameCore.SendCommand(CommandGroup.GROUP_CACHE, CacheInst.VERSION_VERIFY)//LoadVersionList()
			);	
	}
}
