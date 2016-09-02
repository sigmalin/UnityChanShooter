using UnityEngine;
using System.Collections;

public sealed class LoadingProgress : Progress, IInput, IUserInterface 
{
	public IInput Operator { get { return this; } }

	Transform mHideRoot = null;

	// Use this for initialization
	void Start () 
	{
		InitialProgress ();	

		mHideRoot = this.transform.parent;

		if (this.gameObject.activeSelf == true)
			this.gameObject.SetActive (false);
	}

	public bool HandleInput ()
	{
		return true;
	}

	public void Show(Transform _root)
	{
		this.transform.parent = _root;

		if (this.gameObject.activeSelf == false)
			this.gameObject.SetActive (true);
	}

	public void Hide()
	{
		this.transform.parent = mHideRoot;

		if (this.gameObject.activeSelf == true)
			this.gameObject.SetActive (false);
	}

	public void SendCommand(uint _inst, params System.Object[] _params)
	{
		switch(_inst)
		{
		case UiInst.SET_PROGRESS_TEXT:
			Tab = (string)_params[0];
			break;

		case UiInst.SET_PROGRESS_PERCENT:
			Percent = (float)_params [0];
			break;
		}
	}
}
