using UnityEngine;
using System.Collections;
using System.Linq;
using UniRx;
using UniRx.Triggers;

public partial class GameCore
{
	void InitialBatch()
	{
		IObservable<float> batchUpdate = this.UpdateAsObservable ().Select (_ => Time.deltaTime).Publish ().RefCount ();

		batchUpdate.Subscribe (_ => HandleInput ());

		batchUpdate.Subscribe (_ => BatchUpdate (_));


		IObservable<float> batchLateUpdate = this.LateUpdateAsObservable ().Select (_ => Time.deltaTime).Publish ().RefCount ();

		batchLateUpdate.Subscribe (_ => BatchLateUpdate (_));

		batchLateUpdate.Subscribe (_ => LateUpdateFlow ());
	}

	void BatchUpdate(float _delta)
	{
		ICommand[] commands = GetAllCommand ();
		if (commands == null)
			return;

		for (int Indx = 0; Indx < commands.Length; ++Indx) 
		{
			if (commands [Indx] != null)
				commands [Indx].BatchUpdate (_delta);
		}
	}

	void BatchLateUpdate(float _delta)
	{
		ICommand[] commands = GetAllCommand ();
		if (commands == null)
			return;

		for (int Indx = 0; Indx < commands.Length; ++Indx) 
		{
			if (commands [Indx] != null)
				commands [Indx].BatchLateUpdate (_delta);
		}
	}
}
