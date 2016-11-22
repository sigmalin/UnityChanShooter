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
		if (mPostEffectList != null) 
		{
			mPostEffectList.Clear ();
			mPostEffectList = null;
		}
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
			RenderTexture temporary = RenderTexture.GetTemporary (Screen.width, Screen.height, 0, RenderTextureFormat.Default);

			RenderTexture target = temporary;

			for (int Indx = 0; Indx < mPostEffectList.Count; ++Indx) 
			{
				if (mPostEffectList [Indx] == null)
					continue;

				mPostEffectList [Indx].OnPostRender (_src, target);

				RenderTexture swap = _src;
				_src = target;
				target = swap;
			}

			Graphics.Blit (_src, _dest);

			RenderTexture.ReleaseTemporary (temporary);
		} 
		else
		{
			Graphics.Blit (_src, _dest);
		}

	}
}
