using UnityEngine;
using System.Collections;

public class Shock : IPostEffect 
{
	Material mPostEffect;

	float mTime;

	// Use this for initialization
	public void Enter () 
	{
		mPostEffect = (Material)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY, 
			RepositoryParam.GET_POST_EFFECT_DATA,
			PostEffectRepository.PostEffectType.SHOCK);

		mPostEffect.SetVector("_ShockCenter", new Vector4(0.5f, 0.5f, 1f, 0f));

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

				mPostEffect.SetFloat("_ShockTime", mTime);

				Graphics.Blit (_src, _dest, mPostEffect);
			}
			else
			{
				Graphics.Blit (_src, _dest);
			}
		}
	}
}
