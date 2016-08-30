using UnityEngine;
using System.Collections;

public partial class GameCore 
{
	IFlow mFlow = null;

	static public void SetFlow(IFlow _flow)
	{
		if (Instance == null)
			return;

		if (Instance.mFlow != null)
			Instance.mFlow.Exit ();

		Instance.mFlow = _flow;

		if (Instance.mFlow != null)
			Instance.mFlow.Enter ();
	}

	static public void FlowEvent(uint _eventID)
	{
		if (Instance == null)
			return;

		if (Instance.mFlow != null)
			Instance.mFlow.Event (_eventID);
	}
}
