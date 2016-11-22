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

	public SpriteRepository Sprites;

	public SpriteList SpriteNum;

	public delegate bool IsActive();

	public ParticleSystem psys;

	public class simMouse
	{
		public int ID;

		public IsActive Func;
		 
	}

	public Transform ori;
	public Transform tar;
	float angel = 0;

	public Material mMat;

	float mTime = 0;

	// Use this for initialization
	void Start ()
	{
		/*
		ReactiveProperty<int> counter = new ReactiveProperty<int> (0);

		counter.Where (_ => _ == 0).Subscribe (_ => SpriteNum.SetSpriteList (string.Empty, Color.white, null));

		counter.Where (_ => _ != 0).Subscribe (_ => SpriteNum.SetSpriteList (_.ToString(), Color.white, GetSprite));

		int count = 1;

		this.UpdateAsObservable ()
			.Where (_ => Input.GetMouseButtonUp (0))
			.Subscribe (_ => counter.Value += count);

		Observable.Interval (System.TimeSpan.FromSeconds (1f))
			.Where (_ => counter != null && counter.Value != 0)
			.Select (_ => counter.Value)
			.Buffer (2, 1)
			.Where (_ => _ [0] == _ [1])
			.Subscribe (_ => 
				{					
					count += 3;
					counter.Value = 0;
					Debug.Log("count = " + count);
				});
				*/

		int[] xxx = {1,2,3,4,5,6 };

		xxx.ToObservable ()
			.Select(_ => _) 
			.Subscribe (_ => Debug.Log (_));

		mMat.SetVector ("_ShakeParam", new Vector4(10F,10F,0.8F,0.2F));
		mTime = 0f;
	}

	UnityEngine.Sprite GetSprite(char _c)
	{
		return Sprites.SpriteDB.Where (_ => _.Key == _c).Select(_ => _.UiSprite).FirstOrDefault ();
	}

	void OnRenderImage(RenderTexture _src, RenderTexture _dest)
	{
		if (mTime < 1) 
		{
			mTime += Time.deltaTime;

			mMat.SetFloat("_ShakeTime", (1F - Mathf.Abs((mTime - 0.5F) * 2F)));
		}

		Graphics.Blit( _src, _dest, mMat);  
	}

}
