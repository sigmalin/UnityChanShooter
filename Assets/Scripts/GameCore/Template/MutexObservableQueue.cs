using UnityEngine;
using System.Collections;
using UniRx;

public class MutexObservableQueue<T> : RequestQueue<T> where T : struct
{
	System.IDisposable mRequestDisposable = null;

	bool mIsLock = false;

	public void Initial(IObservable<float> _observable, System.Action<RequestQueue<T>, System.Action> _OnCompleted)
	{
		Release ();

		mRequestDisposable = _observable
			.Where(_ => mIsLock == false)
			.Select(_ => Package())
			.Where(_ => _.IsEmpty() == false)
			.Subscribe (_ => {
				mIsLock = true;
				_OnCompleted(_, () => mIsLock = false);
			});
	}

	public void Release()
	{
		mIsLock = false;

		Clear ();

		if (mRequestDisposable != null) 
		{
			mRequestDisposable.Dispose ();

			mRequestDisposable = null;
		}
	}

	public bool IsIdle()
	{
		return ((mIsLock == false) && (TotalCount() == 0));
	}
}
