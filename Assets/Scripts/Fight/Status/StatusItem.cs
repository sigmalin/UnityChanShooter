using UnityEngine;
using System.Collections;
using UniRx;

public class StatusItem : MonoBehaviour, IItem 
{
	[SerializeField]
	UnityEngine.UI.Text mTitle;

	[SerializeField]
	UnityEngine.UI.Text mValue;

	System.IDisposable mDisposable;

	public void Initial (params System.Object[] _params)
	{
		mTitle.text = (string)_params[0];
	}

	public void Release ()
	{
		if (mDisposable != null)
			mDisposable.Dispose ();

		mDisposable = null;
	}

	public void SetReactiveProperty<T>(ReadOnlyReactiveProperty<T> _reactiveProperty)
	{
		Release ();

		mDisposable = _reactiveProperty.Subscribe (_ => mValue.text = string.Format("{0:0.0%}", _));
	}
}
