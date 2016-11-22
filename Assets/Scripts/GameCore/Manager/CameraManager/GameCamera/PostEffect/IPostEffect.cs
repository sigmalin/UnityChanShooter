using UnityEngine;
using System.Collections;

public interface IPostEffect
{
	void Enter();

	void Exit();

	void OnPostRender (RenderTexture _src, RenderTexture _dest);
}
