using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniRx;
using UniRx.Triggers;

public class DevelopTestCode : MonoBehaviour 
{
	Subject<float> mSubject = new Subject<float>();

	public UnityEngine.EventSystems.EventSystem mEventSys;

	public UnityEngine.UI.GraphicRaycaster mRaycaster;

	public RectTransform mRectBoard;

	public RectTransform mRectNob;

	bool isPress = false;

	public GameObject go;

	public ReactiveProperty<bool> IsMousePress { get; private set; }

	public NavMeshAgent Agent;

	public Transform target;

	public Rigidbody rig;

	// Use this for initialization
	void Start () 
	{
		IsMousePress = new ReactiveProperty<bool> (false);

		IsMousePress.Buffer (2, 1)
			.Where (_ => _ [0] != _ [1])
			.Select(_ => _[1])
			.Subscribe (_ => Debug.Log(_));

		IsMousePress.Value = true;
		IsMousePress.Value = false;
		IsMousePress.Value = false;
		IsMousePress.Value = false;
		IsMousePress.Value = false;
		IsMousePress.Value = true;
	}


	System.IDisposable test = null;

	void SetTimer()
	{
		if (test != null) {
			test.Dispose ();
			test = null;
		}

		test = Observable.Defer<long>
		(
			() => 
			{
				Debug.Log("start = " + Time.time);
				return Observable.Timer(System.TimeSpan.FromSeconds(2f));
			}
		)
			.Do(_ => Debug.Log("do = " + Time.time))
			.SelectMany(_ => Observable.Timer(System.TimeSpan.FromSeconds(1f)))
			.Subscribe(_ => Debug.Log("Subscribe = " + Time.time))
			.AddTo(this.gameObject);
	}
}
