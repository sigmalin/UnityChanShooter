using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;

public partial class CacheManager
{
	MutexObservableQueue<LoadRequest> mDownLoadRequest = null;

	void InitialDownLoadRequest()
	{
		ReleaseDownLoadRequest ();

		mDownLoadRequest = new MutexObservableQueue<LoadRequest> ();

		mDownLoadRequest.Initial (LateUpdateObservable, (_request, _callback) => DownLoadCache (_request.ToArray (), _callback));
	}

	void ReleaseDownLoadRequest()
	{
		if (mDownLoadRequest != null) 
		{
			mDownLoadRequest.Release ();

			mDownLoadRequest = null;
		}
	}

	void AddDownLoad(string _key)
	{
		if (IsLocalCacheExist (_key) == false) 
		{
			mDownLoadRequest.Enqueue (new LoadRequest (_key));
		}
	}

	void ReportDownLoadState(bool _isSuccess)
	{
		if (_isSuccess) 
		{
			if (mDownLoadRequest.IsIdle ())
				GameCore.SendFlowEvent (FlowEvent.DOWN_LOAD_CACHE_COMPLETED);
			else
				GameCore.SendFlowEvent (FlowEvent.DOWN_LOAD_CACHE_UNDONE);
		} 
		else 
		{
			GameCore.SendFlowEvent(FlowEvent.DOWN_LOAD_CACHE_FAILURE);
		}
	}

	void DownLoadCache(LoadRequest[] _list, System.Action _callbackUnlock)
	{
		VersionRepository.VersionInfo[] versions = _list
													.Select(_ => _.Path)
													.Distinct()
													.Select (_ => new  VersionRepository.VersionInfo (_, GetLastestVersion (_)))
													.ToArray ();

		DownLoadAndWrite2Stream (versions, 
			() => {
				_callbackUnlock ();
				ReportDownLoadState (true);
			},
			ex => {
				_callbackUnlock ();
				ReportDownLoadState (false);
			});
	}
}
