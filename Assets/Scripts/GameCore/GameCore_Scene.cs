﻿using UnityEngine;
using System.Collections;
using System.Linq;
using UniRx;

public partial class GameCore
{
	string[] mLoadList = null;

	System.IDisposable mSceneDisposable = null;

	bool mIsAllowSwitchScene = true;

	void SwitchScene(string _scene, string[] _loadList, bool _record)
	{
		if (mIsAllowSwitchScene == false)
			return;

		mIsAllowSwitchScene = false;

		if (mSceneDisposable != null) 
		{
			mSceneDisposable.Dispose ();
			mSceneDisposable = null;
		}

		mSceneDisposable = Observable.FromCoroutine<AsyncOperation> ((observer, cancellationToken) => LoadScene ("Scene/Loading", true, observer, cancellationToken))
			.Do(_ => { 	
				//_.allowSceneActivation = true;

				ReleaseCache(mLoadList); 
				mLoadList = _loadList; 
			})
			.SelectMany (_ => Observable.FromCoroutine<Unit> ((observer, cancellationToken) => MemoryManagement (observer, cancellationToken)))
			.Do(_ => DownLoadCache(mLoadList))
			.SelectMany (FlowObservable.Where(_ => _ == FlowEvent.DOWN_LOAD_CACHE_COMPLETED).First())
			.Do(_ => ReadCache(mLoadList))
			.SelectMany (FlowObservable.Where(_ => _ == FlowEvent.READ_CACHE_COMPLETED).First())
			.SelectMany (_ => Observable.FromCoroutine<AsyncOperation> ((observer, cancellationToken) => LoadScene (_scene, false, observer, cancellationToken)))
			.Subscribe (
				_ => 
				{ 
					_.allowSceneActivation = true; 
					mSceneDisposable.Dispose(); 
					mSceneDisposable = null; 

					if (_record == false)
						mLoadList = null;

					mIsAllowSwitchScene = true;
				},
				_ex => Debug.LogError(_ex),
				() => Debug.Log("Completed")
			);

	}

	IEnumerator LoadScene(string _scene, bool _isAllowSceneActivation, IObserver<AsyncOperation> observer, CancellationToken cancellationToken)
	{
		Debug.Log ("LoadScene = " + _scene);

		AsyncOperation oper = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (_scene);
		if (oper == null) 
		{
			observer.OnError (new System.Exception (string.Format ("Loading Scene {0} Failured !!", _scene)));
		} 
		else 
		{
			oper.allowSceneActivation = _isAllowSceneActivation;

			if (_isAllowSceneActivation) 
			{
				while (oper.isDone && cancellationToken.IsCancellationRequested == false)
					yield return null;
			} 
			else 
			{
				while (oper.progress < 0.9F && cancellationToken.IsCancellationRequested == false)
					yield return null;
			}

			//while (oper.progress < 0.9F && cancellationToken.IsCancellationRequested == false)
			//	yield return null;
			
			if (cancellationToken.IsCancellationRequested) yield break;

			observer.OnNext (oper);
			observer.OnCompleted ();
		}
	}

	IEnumerator MemoryManagement(IObserver<Unit> observer, CancellationToken cancellationToken)
	{
		System.GC.Collect ();

		AsyncOperation operRes = Resources.UnloadUnusedAssets ();
		while(operRes.isDone == false && cancellationToken.IsCancellationRequested == false)
			yield return null;

		if (cancellationToken.IsCancellationRequested) yield break;

		observer.OnNext (Unit.Default);
	}

	void ReleaseCache(string[] _releaseList)
	{
		if (_releaseList == null)
			return;

		for (int Indx = 0; Indx < _releaseList.Length; ++Indx) 
		{
			if (string.IsNullOrEmpty (_releaseList [Indx]) == false)
				SendCommand (CommandGroup.GROUP_CACHE, CacheInst.RELEASE_CACHE, _releaseList [Indx]);
		}
	}

	void DownLoadCache(string[] _loadList)
	{
		if (_loadList != null) 
		{
			for (int Indx = 0; Indx < _loadList.Length; ++Indx) 
			{
				if (string.IsNullOrEmpty (_loadList [Indx]) == false) 
				{
					SendCommand (CommandGroup.GROUP_CACHE, CacheInst.DOWN_LOAD_CACHE, _loadList [Indx]);
				}
			}
		} 
		else 
		{
			SendCommand (CommandGroup.GROUP_CACHE, CacheInst.REPORT_LOAD_STATE);
		}
	}

	void ReadCache(string[] _loadList)
	{
		if (_loadList != null) 
		{
			for (int Indx = 0; Indx < _loadList.Length; ++Indx) 
			{
				if (string.IsNullOrEmpty (_loadList [Indx]) == false) 
				{
					SendCommand (CommandGroup.GROUP_CACHE, CacheInst.READ_CACHE, _loadList [Indx]);
				}
			}
		}
		else 
		{
			SendCommand (CommandGroup.GROUP_CACHE, CacheInst.REPORT_READ_STATE);
		}
	}

	void AddCache(string[] _loadList, System.Action _callbackCompleted, bool _record)
	{
		if (_loadList == null)
			return;

		if (_record == true)
		{
			if(mLoadList == null)
				mLoadList = _loadList;
			else
				mLoadList = mLoadList.Concat(_loadList).Distinct().ToArray();
		}

		Observable.Start(() => DownLoadCache(_loadList))
			.SelectMany (FlowObservable.Where(_ => _ == FlowEvent.DOWN_LOAD_CACHE_COMPLETED).First())
			.Do(_ => ReadCache(_loadList))
			.SelectMany (FlowObservable.Where(_ => _ == FlowEvent.READ_CACHE_COMPLETED).First())
			.Subscribe(_ =>
				{
					if (_callbackCompleted != null)
						_callbackCompleted();
				}).AddTo(this);
	}

	static public void AdditionalLoad(string[] _loadList, System.Action _callbackCompleted = null, bool _record = true)
	{
		if (Instance == null)
			return;

		Instance.AddCache (_loadList, _callbackCompleted, _record);
	}

	static public void ChangeScene(string _scene, string[] _loadList = null, bool _record = true)
	{
		if (Instance == null)
			return;

		Instance.SwitchScene (_scene, _loadList, _record);
	}
}
