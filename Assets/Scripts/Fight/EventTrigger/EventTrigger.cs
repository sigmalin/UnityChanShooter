using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class EventTrigger : MonoBehaviour 
{
	public enum EventTriggerEnum
	{
		Event_Trigger_1,
		Event_Trigger_2,
		Event_Trigger_3,
		Event_Trigger_4,
		Event_Trigger_5,
		Event_Trigger_6,
		Event_Trigger_7,
		Event_Trigger_8,
		Event_Trigger_9,
	}

	[SerializeField]
	EventTriggerEnum mTriggerEvent;

	// Use this for initialization
	void Start () 
	{
		this.OnTriggerEnterAsObservable ()
			.Subscribe (_ => SendEvent ());	
	}

	void SendEvent()
	{
		switch (mTriggerEvent) 
		{
		case EventTriggerEnum.Event_Trigger_1:
			GameCore.SendFlowEvent (FlowEvent.GAME_TRIGGER_EVENT_1);
			break;

		case EventTriggerEnum.Event_Trigger_2:
			GameCore.SendFlowEvent (FlowEvent.GAME_TRIGGER_EVENT_2);
			break;

		case EventTriggerEnum.Event_Trigger_3:
			GameCore.SendFlowEvent (FlowEvent.GAME_TRIGGER_EVENT_3);
			break;

		case EventTriggerEnum.Event_Trigger_4:
			GameCore.SendFlowEvent (FlowEvent.GAME_TRIGGER_EVENT_4);
			break;

		case EventTriggerEnum.Event_Trigger_5:
			GameCore.SendFlowEvent (FlowEvent.GAME_TRIGGER_EVENT_5);
			break;

		case EventTriggerEnum.Event_Trigger_6:
			GameCore.SendFlowEvent (FlowEvent.GAME_TRIGGER_EVENT_6);
			break;

		case EventTriggerEnum.Event_Trigger_7:
			GameCore.SendFlowEvent (FlowEvent.GAME_TRIGGER_EVENT_7);
			break;

		case EventTriggerEnum.Event_Trigger_8:
			GameCore.SendFlowEvent (FlowEvent.GAME_TRIGGER_EVENT_8);
			break;

		case EventTriggerEnum.Event_Trigger_9:
			GameCore.SendFlowEvent (FlowEvent.GAME_TRIGGER_EVENT_9);
			break;
		}
	}
}
