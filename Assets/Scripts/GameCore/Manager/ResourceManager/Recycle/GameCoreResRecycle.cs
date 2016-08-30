using UnityEngine;
using System.Collections;

public class GameCoreResRecycle : MonoBehaviour 
{
	public delegate void RecycleCallBack ();

	RecycleCallBack mRecycleCallBack = null;
	public RecycleCallBack RecycleMethod { set { mRecycleCallBack = value; } }

	public virtual void OnDestroy()
	{
		mRecycleCallBack = null;
	}

	public void Recycle()
	{
		if (mRecycleCallBack != null)
			mRecycleCallBack ();
		else
			this.gameObject.SetActive (false);
	}
}
