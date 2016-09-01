using UnityEngine;
using System.Collections;

public partial class GameCore : MonoBehaviour 
{
	static GameCore mInstance = null;
	static public GameCore Instance { get { return mInstance; } }

	// Use this for initialization
	void Awake () 
	{
		if (mInstance != null) 
		{
			Destroy (this);
		} 
		else 
		{
			mInstance = this;

			Initialize ();

			DontDestroyOnLoad (this.gameObject);
		}	
	}

	void OnDestroy ()
	{
		if (mInstance == this) 
		{
			mInstance = null;

			Release ();
		}
	}

	void Initialize()
	{
		InitialAnimID ();

		InitialLayer ();

		InitialInput ();

		InitialCommand ();

		InitialParam ();

		InitialBatch ();

		InitialFlow ();
	}

	void Release()
	{
		ReleaseFlow ();
	}
}
