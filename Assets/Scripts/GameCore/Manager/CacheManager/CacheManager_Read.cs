using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;

public partial class CacheManager
{
	public struct LoadRequest
	{
		public string Path;

		public LoadRequest(string _Path)
		{
			Path = _Path;
		}
	}

	MutexObservableQueue<LoadRequest> mReadRequest = null;

	void InitialReadRequest()
	{
		ReleaselReadRequest ();

		mReadRequest = new MutexObservableQueue<LoadRequest> ();

		mReadRequest.Initial (LateUpdateObservable, (_request, _callback) => BatchLoadList (_request.ToArray (), _callback));
	}

	void ReleaselReadRequest()
	{
		if (mReadRequest != null) 
		{
			mReadRequest.Release ();

			mReadRequest = null;
		}
	}

	void AddReadLoad(string _key)
	{
		if (IsCacheLoad (_key) == true)
			return;
		
		mReadRequest.Enqueue (new LoadRequest (_key));
	}

	string GetCachePath(string _key)
	{
		return string.Format ("{0}//{1}{2}", Application.persistentDataPath, _key, ASSET_BUNDLE_EXTENSION);
	}

	void ReportReadState(bool _isSuccess)
	{
		if (_isSuccess) 
		{
			if (mReadRequest.IsIdle ())
				GameCore.SendFlowEvent (FlowEvent.LOAD_CACHE_COMPLETED);
			else
				GameCore.SendFlowEvent (FlowEvent.LOAD_CACHE_UNDONE);
		} 
		else 
		{
			GameCore.SendFlowEvent(FlowEvent.LOAD_CACHE_FAILURE);
		}
	}

	void BatchLoadList(LoadRequest[] _list, System.Action _callbackUnLock)
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

					ReportReadState(true);
				},
				_ex =>
				{
					_callbackUnLock();

					ReportReadState(false);
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
