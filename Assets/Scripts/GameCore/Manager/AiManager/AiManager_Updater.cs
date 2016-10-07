using System.Collections.Generic;
using System.Linq;
using UniRx;

public sealed partial class AiManager
{
	System.IDisposable mAiDisposable = null;

	void InitialAiUpdate()
	{
		ReleaseAiUpdate ();

		mAiDisposable = UpdateObservable.Select (_ => GetAllPlayerID ())
			.SelectMany (_ => _.ToObservable ())
			.Select(_ => GetAiData(_))
			.Where(_ => _ != null)
			.Subscribe (_ => _.Think ());
	}

	void ReleaseAiUpdate()
	{
		if (mAiDisposable != null) 
		{
			mAiDisposable.Dispose ();

			mAiDisposable = null;
		}
	}
}
