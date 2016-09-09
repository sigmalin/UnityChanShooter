﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
		GameCore.ChangeScene ("Scene/Lobby", GetLoadList());
	}

	public override void Event (uint _eventID)
	{
		switch (_eventID) 
		{
		case FlowEvent.VERSION_VERFITY_COMPLETED:
			GameCore.SetNextFlow (null);
			break;
		}
	}

	void WaitClick2LoadVersionList()
	{
		IObservable<Unit> stream = this.UpdateAsObservable ();

		stream.Where(_ => Input.GetMouseButtonDown(0))
			.First()
			.SelectMany(stream.Where(_ => Input.GetMouseButtonUp(0)))
			.First()
			//.RepeatUntilDestroy(this.gameObject)
			.Subscribe 
			( 
				_ => {},
				_ex => Debug.Log(_ex),
				() => GameCore.SendCommand(CommandGroup.GROUP_CACHE, CacheInst.VERSION_VERIFY)//LoadVersionList()
			);	
	}

	string[] GetLoadList()
	{
		string[] listPortrait = GameCore.UserProfile.HoldCharacterList
			.Select (_ => (string)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_PORTRAIT_PATH, _))
			.ToArray ();

		return listPortrait;
	}
}