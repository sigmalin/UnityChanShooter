using UnityEngine;
using System.Collections;

public class Lut_BuleHue : IPostEffect 
{
	Material mPostEffect;

	// Use this for initialization
	public void Enter () 
	{
		mPostEffect = (Material)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY, 
			RepositoryParam.GET_POST_EFFECT_DATA,
			PostEffectRepository.PostEffectType.BLUE_HUE);
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
			Graphics.Blit (_src, _dest, mPostEffect);
		}
	}
}
