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

	// Use this for initialization
	void Start () 
	{
		Debug.Log (string.Format("res = {0}", Application.CanStreamedLevelBeLoaded("GamePlay")));
		//ResetObservable ();
		//Observable.Timer (System.TimeSpan.FromSeconds (15)).Subscribe (_ => ResetObservable() );
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

		mDisposable = Observable.Start (() => 1F)
			.SelectMany (
			mSubject.AsObservable ()
				.Scan ((_pre, _cur) => _pre + _cur)
				.Do(res => Debug.Log("Step1 res = " + res))
				.Where (res => 5F <= res)
				.Do(res => Debug.Log("step1 Completed"))
				.First()
			)
			.SelectMany(
				mSubject.AsObservable ()
				.Scan ((_pre, _cur) => _pre + _cur)
				.Do(res => Debug.Log("Step2 res = " + res))
				.Where (res => 5F <= res)
				.Do(res => Debug.Log("Step2 Completed"))
				.First()
			)
			.Subscribe (_ => { Debug.Log ("Call Subscribe"); } );
	}
}
