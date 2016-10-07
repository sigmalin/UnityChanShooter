﻿using UnityEngine;
using System.Collections;
using UniRx;

public partial class Flow_GamePlay
{
	GameObject mResultUI;

	void ClearResultUI()
	{
		if (mResultUI != null) 
		{
			IUserInterface ui = mResultUI.GetComponent<IUserInterface> ();
			if (ui != null) GameCore.PopInterface (ui);

			mResultUI = null;
		}
	}

	void SetVictoryUI()
	{
		ClearResultUI ();

		mResultUI = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.INSTANT_RESOURCE_UI, "Victory");
		if (mResultUI != null) 
		{
			IUserInterface ui = mResultUI.GetComponent<IUserInterface> ();
			if (ui != null) GameCore.PushInterface (ui);
		}
	}
}
