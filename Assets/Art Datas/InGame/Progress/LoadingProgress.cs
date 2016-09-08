using UnityEngine;
using System.Collections;

public sealed class LoadingProgress : Progress, IInput, IUserInterface 
{
	public IInput Operator { get { return this; } }

	public class InstSet
	{
		public const uint SET_PROGRESS_TEXT = 0;
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

		if (this.gameObject.activeSelf == false)
			this.gameObject.SetActive (true);
	}

	public void Hide()
	{
		if (this.gameObject.activeSelf == true)
			this.gameObject.SetActive (false);

		this.transform.SetParent(mHideRoot);
	}

	public void SendCommand(uint _inst, params System.Object[] _params)
	{
		switch(_inst)
		{
		case InstSet.SET_PROGRESS_TEXT:
			Tab = (string)_params[0];
			break;

		case InstSet.SET_PROGRESS_PERCENT:
			Percent = (float)_params [0];
			break;
		}
	}
}
