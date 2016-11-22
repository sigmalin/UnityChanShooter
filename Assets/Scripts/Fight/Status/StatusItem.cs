using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class StatusItem : MonoBehaviour, IItem 
{
	[SerializeField]
	UnityEngine.UI.Text mTitle;

	[SerializeField]
	public SpriteList mSpriteNum;

	System.IDisposable mReactivePropertyDisposable_f;

	System.IDisposable mReactivePropertyDisposable_b;

	float mStartValue;

	float mEndValue;

	float mWeight;

	Color mColor;

	void Start()
	{
		mStartValue = 0F;

		mEndValue = 0F;

		mWeight = 1F;

		mColor = Color.white;

		this.UpdateAsObservable ()
			.Where (_ => mWeight < 1F)
			.Subscribe (_ => {
				mWeight = Mathf.Clamp01(mWeight + Time.deltaTime);
				//mValue.text = string.Format("{0:0.0%}", Mathf.Lerp(mStartValue, mEndValue, mWeight));
				mSpriteNum.SetSpriteList (string.Format ("{0:0%}", Mathf.Lerp(mStartValue, mEndValue, mWeight)), mColor, GetSprite);
			});
	}

	public void Initial (params System.Object[] _params)
	{
		mTitle.text = (string)_params[0];
	}

	public void Release ()
	{
		if (mReactivePropertyDisposable_f != null) 
		{
			mReactivePropertyDisposable_f.Dispose ();

			mReactivePropertyDisposable_f = null;
		}

		if (mReactivePropertyDisposable_b != null) 
		{
			mReactivePropertyDisposable_b.Dispose ();

			mReactivePropertyDisposable_b = null;
		}
	}

	public void SetReactiveProperty(ReadOnlyReactiveProperty<float> _reactiveProperty)
	{
		Release ();

		mReactivePropertyDisposable_f = _reactiveProperty.Subscribe (_ => {
			mStartValue = mEndValue;
			mEndValue = _;
			mWeight = 0F;
		});

		mStartValue = _reactiveProperty.Value;
		mEndValue = _reactiveProperty.Value;
		mWeight = 1F;

		//mValue.text = string.Format("{0:0.0%}", _reactiveProperty.Value);
		mSpriteNum.SetSpriteList (string.Format ("{0:0%}", _reactiveProperty.Value), mColor, GetSprite);
	}

	public void SetReactiveProperty (ReadOnlyReactiveProperty<bool> _reactiveProperty)
	{
		mReactivePropertyDisposable_b = _reactiveProperty.Subscribe(_ => mColor = _ ? Color.white : Color.red);
	}

	UnityEngine.Sprite GetSprite(char _key)
	{
		return (UnityEngine.Sprite)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY, RepositoryParam.GET_SPRITE_DATA, RepositoryManager.SPRITE_DIGITS, _key);
	}

}
