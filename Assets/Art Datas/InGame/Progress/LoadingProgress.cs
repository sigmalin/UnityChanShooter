using UnityEngine;
using System.Collections;

public sealed class LoadingProgress : Progress, IInput, IUserInterface 
{
	public IInput Operator { get { return this; } }

	public class InstSet
	{
		public const uint SET_PROGRESS_PERCENT = 1;
	}

	[SerializeField]
	Transform mHideRoot = null;

	// Use this for initialization
	void Start () 
	{
		InitialProgress ();	
	}

	public bool HandleInput ()
	{
		return true;
	}

	public void Show(Transform _root)
	{
		this.transform.SetParent(_root);
		this.transform.localPosition = new Vector3 (0F, 0F, 0F);
		this.transform.localScale = new Vector3 (1F, 1F, 1F);

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
			Tab = string.Format ("{0}%", (int)((float)_params [0] * 100));
			break;
		}
	}
}
