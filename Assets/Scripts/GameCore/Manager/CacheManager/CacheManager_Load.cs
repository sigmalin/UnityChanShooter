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
		mRequest = new ObservableQueue<LoadRequest> ();

		mRequest.Initial (this.LateUpdateAsObservable (), _ => BatchLoadList (_.ToArray ()));
	}

	void ReleaselLoadRequest()
	{
		if (mRequest != null) 
		{
			mRequest.Release ();

			mRequest = null;
		}
	}

	void AddLoad(string _key)
	{
		if (IsCacheExist (_key) == true)
			return;
		
		mRequest.Enqueue (new LoadRequest (_key));
	}

	string GetCachePath(string _key)
	{
		return string.Format ("{0}//{1}{2}", Application.persistentDataPath, _key, ASSET_BUNDLE_EXTENSION);
	}

	void BatchLoadList(LoadRequest[] _list)
	{
		mIsLoading = true;

		_list.Select (_ => _.Path)
			.Distinct ()
			.Where (_ => IsCacheExist(_) == false)
			.Select (_ => Observable.FromCoroutine<Unit> ((observer, cancellationToken) => LoadAssetBundleFromCache (_, observer, cancellationToken)))
			.Aggregate ((pre, cur) => pre.SelectMany (cur))
			.Subscribe (
				_ => 
				{
					mIsLoading = false;

					if (mRequest.IsEmpty(true))
						GameCore.FlowEvent(FlowEvent.LOAD_CACHE_COMPLETED);
				},
				_ex =>
				{
					mIsLoading = false;

					GameCore.FlowEvent(FlowEvent.LOAD_CACHE_FAILURE);
				}
			);
	}

	IEnumerator LoadAssetBundleFromCache(string _path, IObserver<Unit> observer, CancellationToken cancellationToken)
	{
		AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync (GetCachePath (_path));
		yield return request;

		if (request.assetBundle != null) 
		{
			AddCache (_path, request.assetBundle);

			observer.OnNext (Unit.Default);
		}
		else
			observer.OnError (new System.Exception (string.Format ("File {0} Load Failure!!!", _path)));
	}
}
