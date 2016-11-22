using UnityEngine;
using System.Collections;

public class Shake : IPostEffect 
{
	Material mPostEffect;

	float mTime;

	// Use this for initialization
	public void Enter () 
	{
		mPostEffect = (Material)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY, 
			RepositoryParam.GET_POST_EFFECT_DATA,
			PostEffectRepository.PostEffectType.SHOCK);

		mPostEffect.SetVector ("_ShakeParam", new Vector4(10F,10F,0.8F,0.2F));

		mTime = 0f;
	}

	public void Exit () 
	{
		mPostEffect = null;
	}

	// Update is called once per frame
	public void OnPostRender (RenderTexture _src, RenderTexture _dest)
	{
		if (mPostEffect == null) 
		{
			Graphics.Blit (_src, _dest);
		} 
		else 
		{
			if (mTime < 1) 
			{
				mTime += Time.deltaTime;

				mPostEffect.SetFloat("_ShakeTime", (1F - Mathf.Abs((mTime - 0.5F) * 2F)));

				Graphics.Blit (_src, _dest, mPostEffect);
			}
			else
			{
				Graphics.Blit (_src, _dest);
			}
		}
	}
}
