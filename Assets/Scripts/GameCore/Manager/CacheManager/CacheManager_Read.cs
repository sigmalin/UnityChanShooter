using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;

public partial class CacheManager
{
	MutexObservableQueue<CacheRequest> mReadRequest = null;

	CacheState mReadState = null;

	void InitialReadRequest()
	{
		ReleaselReadRequest ();

		mReadRequest = new MutexObservableQueue<CacheRequest> ();

		mReadRequest.Initial (LateUpdateObservable, (_request, _callback) => BatchLoadList (_request.ToArray (), _callback));

		mReadState = new CacheState ();

		mReadState.Disposable = LateUpdateObservable
			.Where (_ => mReadState != null && mReadState.NeedReport == true)
			.Subscribe (_ => SendReadEvent ());
	}

	void ReleaselReadRequest()
	{
		if (mReadRequest != null) 
		{
			mReadRequest.Release ();

			mReadRequest = null;
		}

		if (mReadState != null) 
		{
			if (mReadState.Disposable != null) 
			{
				mReadState.Disposable.Dispose ();
				mReadState.Disposable = null;
			}

			mReadState = null;
		}
	}

	void AddReadLoad(string _key)
	{
		if (IsCacheLoad (_key) == false) 
		{
			mReadRequest.Enqueue (new CacheRequest (_key));
		}

		ReportReadState ();
	}

	string GetCachePath(string _key)
	{
		return string.Format ("{0}//{1}{2}", Application.persistentDataPath, _key, ASSET_BUNDLE_EXTENSION);
	}

	void ReportReadState()
	{
		if (mReadState == null)
			return;

		mReadState.NeedReport = true;
	}

	void SetReadStateFailure()
	{
		if (mReadState == null)
			return;

		mReadState.NeedReport = true;
		mReadState.HasFailure = true;		
	}

	void SendReadEvent()
	{
		if (mReadState == null)
			return;

		if (mReadState.HasFailure == true) 
		{
			GameCore.SendFlowEvent (FlowEvent.READ_CACHE_FAILURE);
		} 
		else 
		{
			if (mReadRequest.IsIdle ())
				GameCore.SendFlowEvent (FlowEvent.READ_CACHE_COMPLETED);
			else
				GameCore.SendFlowEvent (FlowEvent.READ_CACHE_UNDONE);
		}

		mReadState.NeedReport = false;
		mReadState.HasFailure = false;
	}

	void BatchLoadList(CacheRequest[] _list, System.Action _callbackUnLock)
	{
		_list.Select (_ => _.Path)
			.Distinct ()
			.Where (_ => IsCacheLoad(_) == false)
			.Select (_ => Observable.FromCoroutine<Unit> ((observer, cancellationToken) => LoadAssetBundleFromCache (_, observer, cancellationToken)))
			.Aggregate ((pre, cur) => pre.SelectMany (cur))
			.Subscribe (
				_ => 
				{
					_callbackUnLock();

					ReportReadState();
				},
				_ex =>
				{
					_callbackUnLock();

					SetReadStateFailure();
				}
			);
	}

	IEnumerator LoadAssetBundleFromCache(string _path, IObserver<Unit> observer, CancellationToken cancellationToken)
	{
		AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync (GetCachePath (_path));
		yield return request;

		if (cancellationToken.IsCancellationRequested) yield break;

		if (request.assetBundle != null) 
		{
			AddCache (_path, request.assetBundle);

			observer.OnNext (Unit.Default);

			observer.OnCompleted ();
		}
		else
			observer.OnError (new System.Exception (string.Format ("File {0} Load Failure!!!", _path)));
	}
}
