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
	}
}
