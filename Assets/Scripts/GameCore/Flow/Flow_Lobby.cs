using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public sealed class Flow_Lobby : FlowBehaviour 
{
	public override void Enter()
	{
		base.Enter ();

		IObservable<Unit> stream = this.UpdateAsObservable ();

		stream.Where(_ => Input.GetMouseButtonDown(0))
			.Zip(stream.Where(_ => Input.GetMouseButtonUp(0)), (down, up) => Unit.Default)
			.Subscribe ( _ => GameCore.SetFlow(null) );	
	}

	public override void Exit ()
	{
		base.Exit ();

		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(2);
	}

	public override void Event (uint _eventID)
	{
	}
}
