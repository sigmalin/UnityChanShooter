using UnityEngine;
using System.Collections;
using UniRx;

public sealed partial class Page_Character 
{
	[SerializeField]
	UnityEngine.UI.Button mMask;

	void InitialMask()
	{
		if (mMask != null) 
		{
			mMask.OnClickAsObservable()
				.Subscribe(_ => GameCore.SendCommand(CommandGroup.GROUP_LOBBY, LobbyInst.EXIT_PAGE_CHARACTER));
		}
	}
}
