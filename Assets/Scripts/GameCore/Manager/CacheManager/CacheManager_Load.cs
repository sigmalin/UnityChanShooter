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

	ObservableQueue<LoadRequest> mRequest = null;

	bool mIsLoading = false;

	void InitialLoadRequest()
	{
		ReleaselLoadRequest ();

		mRequest = new ObservableQueue<LoadRequest> ();

		mRequest.Initial (LateUpdateObservable.Where(_ => mIsLoading == false), _ => BatchLoadList (_.ToArray ()));
	}

	void ReleaselLoadRequest()
	{
		if (mRequest != null) 
		{
			mRequest.Release ();

			mRequest = null;
		}

		mIsLoading = false;
	}

	void AddLoad(string _key)
	{
		if (IsCacheLoad (_key) == true)
			return;
		
		mRequest.Enqueue (new LoadRequest (_key));
	}

	string GetCachePath(string _key)
	{
		return string.Format ("{0}//{1}{2}", Application.persistentDataPath, _key, ASSET_BUNDLE_EXTENSION);
	}

	void ReportLoadState(bool _isSuccess)
	{
		if (_isSuccess) 
		{
			if (mRequest.IsEmpty (true))
				GameCore.SendFlowEvent (FlowEvent.LOAD_CACHE_COMPLETED);
			else
				GameCore.SendFlowEvent (FlowEvent.LOAD_CACHE_UNDONE);
		} 
		else 
		{
			GameCore.SendFlowEvent(FlowEvent.LOAD_CACHE_FAILURE);
		}
	}

	void BatchLoadList(LoadRequest[] _list)
	{
		mIsLoading = true;

		_list.Select (_ => _.Path)
			.Distinct ()
			.Where (_ => IsCacheLoad(_) == false)
			.Select (_ => Observable.FromCoroutine<Unit> ((observer, cancellationToken) => LoadAssetBundleFromCache (_, observer, cancellationToken)))
			.Aggregate ((pre, cur) => pre.SelectMany (cur))
			.Subscribe (
				_ => 
				{
					mIsLoading = false;

					ReportLoadState(true);
				},
				_ex =>
				{
					mIsLoading = false;

					ReportLoadState(false);
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
