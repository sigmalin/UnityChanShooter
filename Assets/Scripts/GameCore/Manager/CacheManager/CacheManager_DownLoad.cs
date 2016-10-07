	using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;

public partial class CacheManager
{
	MutexObservableQueue<CacheRequest> mDownLoadRequest = null;

	CacheState mDownLoadState = null;

	void InitialDownLoadRequest()
	{
		ReleaseDownLoadRequest ();

		mDownLoadRequest = new MutexObservableQueue<CacheRequest> ();

		mDownLoadRequest.Initial (LateUpdateObservable, (_request, _callback) => DownLoadCache (_request.ToArray (), _callback));

		mDownLoadState = new CacheState ();

		mDownLoadState.Disposable = LateUpdateObservable
			.Where (_ => mDownLoadState != null && mDownLoadState.NeedReport == true)
			.Subscribe (_ => SendDownLoadEvent ());
	}

	void ReleaseDownLoadRequest()
	{
		if (mDownLoadRequest != null) 
		{
			mDownLoadRequest.Release ();

			mDownLoadRequest = null;
		}

		if (mDownLoadState != null) 
		{
			if (mDownLoadState.Disposable != null) 
			{
				mDownLoadState.Disposable.Dispose ();
				mDownLoadState.Disposable = null;
			}

			mDownLoadState = null;
		}
	}

	void AddDownLoad(string _key)
	{
		if (IsLocalCacheExist (_key) == false) 
		{
			mDownLoadRequest.Enqueue (new CacheRequest (_key));
		}

		ReportDownLoadState ();
	}

	void ReportDownLoadState()
	{
		if (mDownLoadState == null)
			return;

		mDownLoadState.NeedReport = true;
	}

	void SetDownLoadStateFailure()
	{
		if (mDownLoadState == null)
			return;

		mDownLoadState.NeedReport = true;
		mDownLoadState.HasFailure = true;		
	}

	void SendDownLoadEvent()
	{
		if (mDownLoadState == null)
			return;

		if (mDownLoadState.HasFailure == true) 
		{
			GameCore.SendFlowEvent (FlowEvent.DOWN_LOAD_CACHE_FAILURE);
		} 
		else 
		{
			if (mDownLoadRequest.IsIdle ())
				GameCore.SendFlowEvent (FlowEvent.DOWN_LOAD_CACHE_COMPLETED);
			else
				GameCore.SendFlowEvent (FlowEvent.DOWN_LOAD_CACHE_UNDONE);
		}

		mDownLoadState.NeedReport = false;
		mDownLoadState.HasFailure = false;
	}

	void DownLoadCache(CacheRequest[] _list, System.Action _callbackUnlock)
	{
		VersionData[] versions = _list
								.Select(_ => _.Path)
								.Distinct()
								.Select (_ => GetVersionData(_))
								.ToArray ();

		DownLoadAndWrite2Stream (versions, 
			() => {
				_callbackUnlock ();

				ReportDownLoadState();
			},
			ex => {
				Debug.LogError (ex);

				_callbackUnlock ();

				SetDownLoadStateFailure();
			});
	}
}
