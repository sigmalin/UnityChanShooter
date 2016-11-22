using UnityEngine;
using System.Collections;

public partial class CacheManager
{
	[SerializeField]
	GameObject mLoadingProgress;

	void ShowLoadingProgress()
	{
		if (mLoadingProgress == null)
			return;

		IUserInterface uiLoadingProgress = mLoadingProgress.GetComponent<IUserInterface> ();

		uiLoadingProgress.Operation (LoadingProgress.InstSet.SET_PROGRESS_PERCENT, 0F);

		GameCore.PushInterface (uiLoadingProgress);
	}

	void HideLoadingProgress()
	{
		if (mLoadingProgress == null)
			return;

		GameCore.PopInterface (mLoadingProgress.GetComponent<IUserInterface>());
	}

	void UpdateLoadingProgress(float _percent)
	{
		if (mLoadingProgress == null)
			return;

		IUserInterface uiLoadingProgress = mLoadingProgress.GetComponent<IUserInterface> ();

		uiLoadingProgress.Operation (LoadingProgress.InstSet.SET_PROGRESS_PERCENT, _percent);
	}

	void UpdateLoadingMessage(string _msg)
	{
		if (mLoadingProgress == null)
			return;

		IUserInterface uiLoadingProgress = mLoadingProgress.GetComponent<IUserInterface> ();

		uiLoadingProgress.Operation (LoadingProgress.InstSet.SET_PROGRESS_MESSAGE, _msg);
	}
}
