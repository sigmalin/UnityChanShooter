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

		uiLoadingProgress.SendCommand (UiInst.SET_PROGRESS_PERCENT, 0F);

		GameCore.PushInterface (uiLoadingProgress);
	}

	void HideLoadingProgress()
	{
		if (mLoadingProgress == null)
			return;

		GameCore.PopPopInterface (mLoadingProgress.GetComponent<IUserInterface>());
	}

	void UpdateLoadingProgress(float _percent)
	{
		if (mLoadingProgress == null)
			return;

		IUserInterface uiLoadingProgress = mLoadingProgress.GetComponent<IUserInterface> ();

		uiLoadingProgress.SendCommand (UiInst.SET_PROGRESS_PERCENT, _percent);
	}
}
