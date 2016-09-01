using UnityEngine;
using System.Collections;
using System.Linq;
using UniRx;

public partial class GameCore
{
	string[] mLoadList = null;

	System.IDisposable mSceneDisposable = null;

	void SwitchScene(string _scene, string[] _loadList)
	{
		if (mSceneDisposable != null) 
		{
			mSceneDisposable.Dispose ();
			mSceneDisposable = null;
		}

		mSceneDisposable = Observable.FromCoroutine<AsyncOperation> ((observer, cancellationToken) => LoadScene ("Scene/Loading", observer, cancellationToken))
			.Do(_ => { 				
				ReleaseCache(mLoadList); 
				mLoadList = _loadList; 
				_.allowSceneActivation = true;
			})
			.SelectMany (_ => Observable.FromCoroutine<Unit> ((observer, cancellationToken) => MemoryManagement (observer, cancellationToken)))
			.Do(_ => LoadCache(mLoadList))
			.SelectMany (FlowObservable.Where(_ => _ == FlowEvent.LOAD_CACHE_COMPLETED).First())
			.SelectMany (_ => Observable.FromCoroutine<AsyncOperation> ((observer, cancellationToken) => LoadScene (_scene, observer, cancellationToken)))
			.Subscribe (
				_ => 
				{ 
					_.allowSceneActivation = true; 
					mSceneDisposable.Dispose(); 
					mSceneDisposable = null; 
				},
				_ex => Debug.LogError(_ex),
				() => Debug.Log("Completed")
			);

	}

	IEnumerator LoadScene(string _scene, IObserver<AsyncOperation> observer, CancellationToken cancellationToken)
	{
		Debug.Log ("LoadScene = " + _scene);
		
		AsyncOperation oper = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (_scene);
		if (oper == null) 
		{
			observer.OnError (new System.Exception (string.Format ("Loading Scene {0} Failured !!", _scene)));
		} 
		else 
		{
			oper.allowSceneActivation = false;

			while (oper.progress < 0.9F && cancellationToken.IsCancellationRequested == false)
				yield return null;

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

	void LoadCache(string[] _loadList)
	{
		if (_loadList != null) 
		{
			for (int Indx = 0; Indx < _loadList.Length; ++Indx) 
			{
				if (string.IsNullOrEmpty (_loadList [Indx]) == false) 
				{
					SendCommand (CommandGroup.GROUP_CACHE, CacheInst.LOAD_CACHE, _loadList [Indx]);
				}
			}
		}

		SendCommand (CommandGroup.GROUP_CACHE, CacheInst.REPORT_LOAD_STATE);
	}

	static public void ChangeScene(string _scene, string[] _loadList = null)
	{
		if (Instance == null)
			return;

		Instance.SwitchScene (_scene, _loadList);
	}
}
