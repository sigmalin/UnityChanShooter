using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class FpsCounter : MonoBehaviour, IInput, IUserInterface
{
	[SerializeField]
	UnityEngine.UI.Text mValue;

	[SerializeField]
	Transform mHideRoot = null;

	public IInput Operator { get { return this; } }

	public class InstSet
	{
		public const uint UPDATE_FPS = 1;
	}

	System.IDisposable mReactivePropertyDisposable;

	public bool HandleInput ()
	{
		return true;
	}

	public void Show(Transform _root)
	{
		this.transform.SetParent(_root);
		this.transform.localPosition = new Vector3 (0F, 0F, 0F);
		this.transform.localScale = new Vector3 (1F, 1F, 1F);

		if (this.gameObject.activeSelf == false)
			this.gameObject.SetActive (true);
	}

	public void Hide()
	{
		if (this.gameObject.activeSelf == true)
			this.gameObject.SetActive (false);

		this.transform.SetParent(mHideRoot);

		ClearDisposable ();
	}

	public void Localization()
	{
	}

	public void Clear()
	{
	}

	public void Operation(uint _inst, params System.Object[] _params)
	{
		switch(_inst)
		{
		case InstSet.UPDATE_FPS:
			
			ClearDisposable ();

			IReadOnlyReactiveProperty<float> counter = (IReadOnlyReactiveProperty<float>)_params [0];
			mReactivePropertyDisposable = counter.Subscribe (_ => mValue.text = string.Format ("{0}", _));
			break;
		}
	}

	void ClearDisposable()
	{
		if (mReactivePropertyDisposable != null) 
		{
			mReactivePropertyDisposable.Dispose ();
			mReactivePropertyDisposable = null;
		}
	}
}
