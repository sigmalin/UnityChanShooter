using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniRx;

public class DownLoadTest : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		string[] list = new string[]{ "a", "b", "b", "c", "d", "d", "e" };



		list.Distinct()
			.Where(_ => !string.Equals(_,"c"))
			.Select (_ => 
				Observable.FromCoroutine<Unit> (
					(observer, cancellationToken) => Test(_, observer, cancellationToken)
				)
			)
			.Aggregate ((pre, cur) => pre.SelectMany (cur))
			.Subscribe (
				_ => Debug.Log("Completed"),
				_ex => Debug.LogError(_ex)
			);
	}

	IEnumerator Test(string url, IObserver<Unit> observer, CancellationToken cancellationToken)
	{
		Debug.Log ("Test1 = " + url);
		yield return new WaitForSeconds (0.5f);

		if (cancellationToken.IsCancellationRequested) yield break;
		Debug.Log ("Test2 = " + url);

		if (string.Equals (url, "d"))
			observer.OnError (new System.Exception("test"));
		else
			observer.OnNext (Unit.Default);

	}
}
