using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class SystemManager
{
	[SerializeField]
	AudioSource mAudio;

	AudioClip mCurAudioClip = null;
	AudioClip mNextAudioClip = null;

	System.IDisposable mFadeInDisposable = null;
	System.IDisposable mFadeOutDisposable = null;

	bool mIsLoop = false;

	float mMaxVolume = 0.2f;

	void InitialAudio()
	{
		ReleaseAudio ();

		mFadeOutDisposable = UpdateObservable
			.Where (_ => mNextAudioClip != null)
			.Where (_ => 
			{
				mAudio.volume = Mathf.Clamp01 (mAudio.volume - (Time.deltaTime * 2f));
				return mAudio.volume == 0f;
			})
			.Subscribe (_ => 
			{
				mAudio.clip = mNextAudioClip;
				mAudio.loop = mIsLoop;
				mAudio.Play();
				mNextAudioClip = null;
			});

		mFadeInDisposable = UpdateObservable
			.Where (_ => mNextAudioClip == null && mAudio.volume != mMaxVolume)
			.Subscribe (_ => 
			{
				mAudio.volume = Mathf.Clamp (mAudio.volume + (Time.deltaTime * 2f), 0f, mMaxVolume);
			});
	}

	void ReleaseAudio()
	{
		mCurAudioClip = null;
		mNextAudioClip = null;

		if (mFadeOutDisposable != null) 
		{
			mFadeOutDisposable.Dispose ();
			mFadeOutDisposable = null;
		}

		if (mFadeInDisposable != null) 
		{
			mFadeInDisposable.Dispose ();
			mFadeInDisposable = null;
		}
	}

	void PlayBGM(string _bgm, bool _isLoop)
	{
		if (mAudio == null)
			return;

		AudioClip clip = (AudioClip)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_AUDIO, _bgm, false);
		if (clip == null)
			return;

		if (clip == mCurAudioClip || clip == mNextAudioClip)
			return;

		mNextAudioClip = clip;

		mIsLoop = _isLoop;
	}
}
