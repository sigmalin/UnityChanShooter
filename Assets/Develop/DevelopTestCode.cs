using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniRx;

public class DevelopTestCode : MonoBehaviour 
{
	Subject<float> mSubject = new Subject<float>();

	public UnityEngine.EventSystems.EventSystem mEventSys;

	public UnityEngine.UI.GraphicRaycaster mRaycaster;

	public RectTransform mRectBoard;

	public RectTransform mRectNob;

	bool isPress = false;

	public ReactiveProperty<bool> IsMousePress { get; private set; }

	// Use this for initialization
	void Start () 
	{
		mSubject.AsObservable ()
			.Buffer(System.TimeSpan.FromSeconds(0.5))
			.Subscribe(_ => Debug.Log(Time.time));
	}

	void Update()
	{
		mSubject.OnNext (Time.deltaTime);
	}
}
