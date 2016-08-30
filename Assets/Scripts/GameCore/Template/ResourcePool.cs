using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourcePool<T>
{
	protected Stack<T> mStack = null;

	public delegate T ResourceProduce();
	public delegate void ResourceRelease(T _res);
	public delegate void ResourceActive(T _res, bool _isActive);

	ResourceProduce mCallbackResourceProduce = null;
	ResourceRelease mCallbackResourceRelease = null;
	ResourceActive mCallbackResourceActive = null;

	public bool IsUseabled { get; private set; }

	public ResourcePool(ResourceProduce _callbackResourceProduce, ResourceRelease _callbackResourceRelease, ResourceActive _callbackResourceActive, int _capacity = 16)
	{
		mCallbackResourceProduce = _callbackResourceProduce;
		mCallbackResourceRelease = _callbackResourceRelease;
		mCallbackResourceActive = _callbackResourceActive;

		mStack = new Stack<T> (_capacity);

		IsUseabled = true;
	}

	public T Produce()
	{
		T res = default(T);

		if (mStack.Count != 0) 
		{
			res = mStack.Pop ();
			if (mCallbackResourceActive != null && res != null)
				mCallbackResourceActive (res, true);
		} 
		else if (mCallbackResourceProduce != null) 
		{
			res = mCallbackResourceProduce ();
		}

		return res;
	}

	public void Recycle(T _res)
	{
		if (mCallbackResourceActive != null && _res != null)
			mCallbackResourceActive (_res, false);

		mStack.Push (_res);
	}

	public void ReleaseAllResource()
	{
		while (mStack.Count != 0) 
		{
			T res = mStack.Pop ();

			if (mCallbackResourceRelease != null && res != null)
				mCallbackResourceRelease (res);
		}
	}

	public void Destroy()
	{
		ReleaseAllResource ();

		mCallbackResourceProduce = null;
		mCallbackResourceRelease = null;
		mCallbackResourceActive = null;

		mStack = null;

		IsUseabled = false;
	}
}
