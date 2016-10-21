using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class StatusItem : MonoBehaviour, IItem 
{
	[SerializeField]
	UnityEngine.UI.Text mTitle;

	[SerializeField]
	UnityEngine.UI.Text mValue;

	System.IDisposable mReactivePropertyDisposable;

	float mStartValue;

	float mEndValue;

	float mWeight;

	void Start()
	{
		mStartValue = 0F;

		mEndValue = 0F;

		mWeight = 1F;

		this.UpdateAsObservable ()
			.Where (_ => mWeight < 1F)
			.Subscribe (_ => {
				mWeight = Mathf.Clamp01(mWeight + Time.deltaTime);
				mValue.text = string.Format("{0:0.0%}", Mathf.Lerp(mStartValue, mEndValue, mWeight));
			});
	}

	public void Initial (params System.Object[] _params)
	{
		mTitle.text = (string)_params[0];
	}

	public void Release ()
	{
		if (mReactivePropertyDisposable != null) 
		{
			mReactivePropertyDisposable.Dispose ();

			mReactivePropertyDisposable = null;
		}
	}

	public void SetReactiveProperty(ReadOnlyReactiveProperty<float> _reactiveProperty)
	{
		Release ();

		mReactivePropertyDisposable = _reactiveProperty.Subscribe (_ => {
			mStartValue = mEndValue;
			mEndValue = _;
			mWeight = 0F;
		});

		mStartValue = _reactiveProperty.Value;
		mEndValue = _reactiveProperty.Value;
		mWeight = 1F;

		mValue.text = string.Format("{0:0.0%}", _reactiveProperty.Value);
	}
}
