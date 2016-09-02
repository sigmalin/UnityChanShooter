using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniRx;

public class DevelopTestCode : MonoBehaviour 
{
	Subject<float> mSubject = new Subject<float>();

	System.IDisposable mDisposable = null;

	int[] test = new int[]{1,2,3,4,5};

	Dictionary<int, int> mActorTable = null;

	// Use this for initialization
	void Start () 
	{
		mActorTable = test.Select (_ => new {key = _, value = 10 + _})
			.ToDictionary (_ => _.key, _ => _.value);

		IObservable<int> obser = mSubject.AsObservable ()
			.Select (_ => mActorTable.Keys)
			.SelectMany (_ => _.ToObservable ())
			.Select (_ => mActorTable [_])
			.Do(_ => Debug.Log("pre"))
			.Publish().RefCount();

		mDisposable = obser.Where (_ => 13 < _)
			.Subscribe (_ => Debug.Log("post1 = " + _));

		obser.Where (_ => _ < 13)
			.Subscribe (_ => Debug.Log("post2 = " + _));


		Observable.Timer (System.TimeSpan.FromSeconds (3F)).Subscribe (
			_ =>
			{
				Debug.Log("Dispose");
				mDisposable.Dispose();
			});
	}

	void Update()
	{
		if(mSubject != null)
			mSubject.OnNext (Time.deltaTime);
	}

	void ResetObservable()
	{
		Debug.Log ("Call ResetObservable");
		if (mDisposable != null) 
		{
			mDisposable.Dispose ();
			mDisposable = null;
		}
	}
}
