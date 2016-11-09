using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class FollowEffect : MonoBehaviour 
{
	Transform mTarget;
	public Transform Target { set { mTarget = value; } }

	// Use this for initialization
	void Start () 
	{
		this.UpdateAsObservable ()
			.Subscribe (_ => UpdatePosition());	
	}
	
	void OnDestroy()
	{
		mTarget = null;
	}

	public void UpdatePosition()
	{
		if (mTarget == null)
			return;

		this.transform.position = mTarget.position;
	}
}
