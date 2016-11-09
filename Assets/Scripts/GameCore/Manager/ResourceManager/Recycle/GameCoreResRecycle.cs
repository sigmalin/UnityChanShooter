using UnityEngine;
using System.Collections;

public class GameCoreResRecycle : MonoBehaviour 
{
	public delegate void RecycleCallBack ();

	RecycleCallBack mRecycleCallBack = null;
	public RecycleCallBack RecycleMethod { set { mRecycleCallBack = value; } }

	bool mIsRecycled = false;
	public bool IsRecycled { set { mIsRecycled = value; } }

	public virtual void OnDestroy()
	{
		mRecycleCallBack = null;

		mIsRecycled = true;
	}

	public void Recycle()
	{
		if (mIsRecycled == true)
			return;

		if (mRecycleCallBack != null) 
		{
			mRecycleCallBack ();

			mIsRecycled = true;
		}
		else 
		{
			this.gameObject.SetActive (false);
		}
	}
}
