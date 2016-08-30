using UnityEngine;
using System.Collections;

public class DisplayBehaviour : MonoBehaviour
{
	[SerializeField]
	Renderer[] mRenderers;

	protected void SetDisplay(bool _isDisplay)
	{
		if (mRenderers == null)
			return;

		for (int Indx = 0; Indx < mRenderers.Length; ++Indx)
			if (mRenderers [Indx]) mRenderers [Indx].enabled = true;
	}

	#if UNITY_EDITOR
	public void CollectRenderers()
	{
		mRenderers = this.gameObject.GetComponentsInChildren<Renderer> ();
	}
	#endif
}
