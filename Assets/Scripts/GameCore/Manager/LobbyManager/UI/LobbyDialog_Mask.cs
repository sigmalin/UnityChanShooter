using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public sealed partial class LobbyDialog 
{
	[SerializeField]
	UnityEngine.UI.Button mMask;

	void InitialDialogMask()
	{
		if (mMask != null) 
		{
			mMask.OnClickAsObservable()
				.Subscribe(_ => RemoveDialogData());
		}
	}
}
