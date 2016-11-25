using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public partial class GameCamera 
{
	List<IPostEffect> mPostEffectList;

	void InitialPostEffect()
	{
		ReleasePostEffect ();

		mPostEffectList = new List<IPostEffect> ();
	}

	void ReleasePostEffect()
	{
		ClearPostEffect ();

		mPostEffectList = null;
	}

	void AddPostEffect(IPostEffect _postEffect)
	{
		if (mPostEffectList == null || _postEffect == null)
			return;

		if (mPostEffectList.Contains (_postEffect) == true)
			return;

		mPostEffectList.Add (_postEffect);

		_postEffect.Enter ();
	}

	void RemovePostEffect(IPostEffect _postEffect)
	{
		if (mPostEffectList == null || _postEffect == null)
			return;

		mPostEffectList.Remove (_postEffect);

		_postEffect.Exit ();
	}

	void OnRenderImage(RenderTexture _src, RenderTexture _dest)
	{
		if (mPostEffectList != null && mPostEffectList.Count != 0) 
		{
			RenderTexture temporary1 = _src;

			RenderTexture temporary2 = null;

			for (int Indx = 0; Indx < mPostEffectList.Count; ++Indx) 
			{
				if (mPostEffectList [Indx] == null)
					continue;

				if (temporary2 == null)
					temporary2 = RenderTexture.GetTemporary (Screen.width, Screen.height, 0, RenderTextureFormat.Default);

				mPostEffectList [Indx].OnPostRender (temporary1, temporary2);

				RenderTexture swap = temporary1 == _src ? null : temporary1;
				temporary1 = temporary2;
				temporary2 = swap;
			}

			Graphics.Blit (temporary1, _dest);

			if(temporary1 != null && temporary1 != _src) RenderTexture.ReleaseTemporary (temporary1);
			if(temporary2 != null) RenderTexture.ReleaseTemporary (temporary2);
		} 
		else
		{
			Graphics.Blit (_src, _dest);
		}

	}

	void ClearPostEffect()
	{
		if (mPostEffectList != null) 
		{
			mPostEffectList.Clear ();
		}
	}
}
