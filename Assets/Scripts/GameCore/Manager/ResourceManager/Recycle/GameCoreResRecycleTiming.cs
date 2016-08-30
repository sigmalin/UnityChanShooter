using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class GameCoreResRecycleTiming : GameCoreResRecycle 
{
	public double RecycleCycle = 1;

	public delegate void RecycleCallBack ();

	// Use this for initialization
	void Start () 
	{
		IObservable<long> recycleTiming = Observable.Timer (System.TimeSpan.FromSeconds (RecycleCycle));
		recycleTiming.Subscribe (_ => Recycle ());
	
		this.OnEnableAsObservable ()
			.SelectMany (_ => recycleTiming)
			.Subscribe (_ => Recycle ());
	}
}
