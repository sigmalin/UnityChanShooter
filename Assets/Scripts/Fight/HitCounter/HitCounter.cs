using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;


public class HitCounter : MonoBehaviour 
{
	[SerializeField]
	SpriteList mSpriteCounter;

	[SerializeField]
	UnityEngine.UI.Image mHitMark;

	ReactiveProperty<int> mHitsCount;

	System.IDisposable mVictimsDisposable;

	System.IDisposable mResetDisposable;

	void OnDestroy()
	{
		Release ();
	}

	public void Initial(uint _actorID)
	{
		Release ();

		mHitsCount = new ReactiveProperty<int> (0);

		mHitsCount.Where (_ => _ != 0 && mSpriteCounter != null)
			.Subscribe (_ => mSpriteCounter.SetSpriteList(_.ToString(), Color.white, GetSprite));

		mHitsCount.Subscribe (_ => Display (_ != 0));

		WeaponManager.WeaponActor mineActor = (WeaponManager.WeaponActor)GameCore.GetParameter (ParamGroup.GROUP_WEAPON, WeaponParam.WEAPON_ACTOR_DATA, _actorID);
		if (mineActor != null) 
		{
			IObservable<uint> hitObservable = mineActor.Victims.AsObservable ().Where (_ => _ != 0);

			mVictimsDisposable = hitObservable
				.Buffer(hitObservable.ThrottleFrame(1))
				.Subscribe(_ => mHitsCount.Value += _.Count);

			mResetDisposable = hitObservable.Throttle(System.TimeSpan.FromSeconds (1f))
				.Subscribe (_ => mHitsCount.Value = 0);
		}

		if (mHitMark != null)
			mHitMark.sprite = GetSprite ('h');
	}

	void Release()
	{
		if (mHitsCount != null) 
		{
			mHitsCount.Dispose ();
			mHitsCount = null;
		}

		if (mVictimsDisposable != null) 
		{
			mVictimsDisposable.Dispose ();
			mVictimsDisposable = null;
		}

		if (mResetDisposable != null) 
		{
			mResetDisposable.Dispose ();
			mResetDisposable = null;
		}
	}

	UnityEngine.Sprite GetSprite(char _key)
	{
		return (UnityEngine.Sprite)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY, RepositoryParam.GET_SPRITE_DATA, RepositoryManager.SPRITE_HITS, _key);
	}

	void Display(bool _isDisplay)
	{
		if (mSpriteCounter != null && _isDisplay == false)
			mSpriteCounter.SetSpriteList (string.Empty, Color.white, null);

		if (mHitMark != null && mHitMark.gameObject.activeSelf != _isDisplay)
			mHitMark.gameObject.SetActive (_isDisplay);
	}
}
