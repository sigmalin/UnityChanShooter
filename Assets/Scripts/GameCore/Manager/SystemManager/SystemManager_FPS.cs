using UnityEngine;
using System.Collections;
using System.Linq;
using UniRx;
using UniRx.Triggers;

public partial class SystemManager
{
	public IReadOnlyReactiveProperty<float> FPSCounter { get; private set; }

	[SerializeField]
	GameObject mFpsCounter;

	public ReactiveProperty<bool> IsShowFPS { get; private set; }

	// Use this for initialization
	void InitialFPS()
	{
		FPSCounter = UpdateObservable
			.Buffer (10, 1)
			.Select (_ => 1.0f / _.Average ())
			.ToReadOnlyReactiveProperty ();

		IsShowFPS = new ReactiveProperty<bool> (false);

		IObservable<bool> streamFPS = IsShowFPS.Buffer (2, 1)
			.Where (_ => _ [0] != _ [1])
			.Select(_ => _[1])
			.Publish ().RefCount ();

		streamFPS.Where (_ => _ == true)
			.Subscribe (_ => 
				{
					IUserInterface uiFPSCounter = mFpsCounter.GetComponent<IUserInterface> ();

					uiFPSCounter.Operation (FpsCounter.InstSet.UPDATE_FPS, FPSCounter);

					GameCore.PushInterface (uiFPSCounter);
				});

		streamFPS.Where (_ => _ == false)
			.Subscribe (_ => 
				{
					GameCore.PopInterface (mFpsCounter.GetComponent<IUserInterface>());	
				});
	}
}
