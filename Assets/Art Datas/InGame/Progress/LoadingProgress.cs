using UnityEngine;
using System.Collections;

public sealed class LoadingProgress : Progress, IInput, IUserInterface 
{
	public IInput Operator { get { return this; } }

	public class InstSet
	{
		public const uint SET_PROGRESS_PERCENT = 1;
		public const uint SET_PROGRESS_MESSAGE = 2;
	}

	[SerializeField]
	Transform mHideRoot = null;

	// Use this for initialization
	void Start () 
	{
	}

	public bool HandleInput ()
	{
		return true;
	}

	public void Show(Transform _root)
	{
		this.transform.SetParent(_root);

		RectTransform rectTrans = this.GetComponent<RectTransform> ();

		if (rectTrans != null)
		{
			rectTrans.anchorMin = new Vector2 (0F,0F);
			rectTrans.anchorMax = new Vector2 (1F,1F);
			rectTrans.pivot = new Vector2 (0.5F, 0.5F);
			rectTrans.offsetMax = new Vector2 (0F,0F);
			rectTrans.offsetMin = new Vector2 (0F,0F);
			rectTrans.localScale = new Vector3 (1F,1F,1F);
			rectTrans.localPosition = new Vector3 (0F,0F,0F);
		}

		if (this.gameObject.activeSelf == false)
			this.gameObject.SetActive (true);
	}

	public void Hide()
	{
		if (this.gameObject.activeSelf == true)
			this.gameObject.SetActive (false);

		this.transform.SetParent(mHideRoot);
	}

	public void Localization()
	{
	}

	public void Clear()
	{
	}

	public void Operation(uint _inst, params System.Object[] _params)
	{
		switch(_inst)
		{
		case InstSet.SET_PROGRESS_PERCENT:
			Percent = (float)_params [0];
			break;

		case InstSet.SET_PROGRESS_MESSAGE:
			Tab = (string)_params [0];
			break;
		}
	}
}
