using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Cloud : MonoBehaviour 
{
	[SerializeField]
	Transform mCenterTrans;

	// Use this for initialization
	void Start () 
	{
		this.LateUpdateAsObservable ()
			.Where (_ => mCenterTrans != null)
			.Subscribe (_ => this.transform.position = mCenterTrans.position);
	}
}
