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

		GameCore.ChangeScene ("GamePlay", GetLoadList());
	}

	public override void Event (uint _eventID)
	{
	}

	string[] GetLoadList()
	{
		return new string[] { 
			(string)GameCore.GetParameter(ParamGroup.GROUP_CACHE, CacheParam.GET_SCENE_PATH, "GamePlay"),
			(string)GameCore.GetParameter(ParamGroup.GROUP_CACHE, CacheParam.GET_BULLET_PATH, (uint)1),
			(string)GameCore.GetParameter(ParamGroup.GROUP_CACHE, CacheParam.GET_CHARACTER_PATH, (uint)100),
			(string)GameCore.GetParameter(ParamGroup.GROUP_CACHE, CacheParam.GET_CHARACTER_PATH, (uint)101),
			(string)GameCore.GetParameter(ParamGroup.GROUP_CACHE, CacheParam.GET_CONTAINER_PATH, (uint)1),
			(string)GameCore.GetParameter(ParamGroup.GROUP_CACHE, CacheParam.GET_WEAPON_DATA_PATH),
			(string)GameCore.GetParameter(ParamGroup.GROUP_CACHE, CacheParam.GET_INSTANT_RESOURCE_INPUT_PATH),
			(string)GameCore.GetParameter(ParamGroup.GROUP_CACHE, CacheParam.GET_WEAPON_PATH, (uint)1)
							};
	}
}
