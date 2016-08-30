using UnityEngine;
using System.Collections;
using UniRx;

public class ObservableQueue<T> : RequestQueue<T> where T : struct
{
	System.IDisposable mRequestDisposable = null;

	public void Initial(IObservable<Unit> _observable, System.Action<RequestQueue<T>> _OnCompleted)
	{
		Release ();

		mRequestDisposable = _observable
			.Select(_ => Package())
			.Where(_ => _.IsEmpty() == false)
			.Subscribe (_ => _OnCompleted(_));
	}

	public void Release()
	{
		Clear ();

		if (mRequestDisposable != null) 
		{
			mRequestDisposable.Dispose ();

			mRequestDisposable = null;
		}
	}
}
