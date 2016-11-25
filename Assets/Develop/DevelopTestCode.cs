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

	public Material mMat2;

	float mTime = 0;

	public class preList
	{
		public int[] IDs;

	}

	// Use this for initialization
	void Start ()
	{		
		mMat2.SetFloat ("_BloodWeight", 0.3f);
		mMat.SetVector ("_ShakeParam", new Vector4(10F,10F,0.8F,0.2F));
		mTime = 0f;
	}

	UnityEngine.Sprite GetSprite(char _c)
	{
		return Sprites.SpriteDB.Where (_ => _.Key == _c).Select(_ => _.UiSprite).FirstOrDefault ();
	}

	void OnRenderImage(RenderTexture _src, RenderTexture _dest)
	{
		RenderTexture temporary1 = _src;

		RenderTexture temporary2 = null;

		for (int Indx = 0; Indx < 2; ++Indx) 
		{
			if (temporary2 == null)
				temporary2 = RenderTexture.GetTemporary (Screen.width, Screen.height, 0, RenderTextureFormat.Default);

			if(Indx == 0)
				Blood (temporary1, temporary2);
			else
				Shake(temporary1, temporary2);

			RenderTexture swap = temporary1 == _src ? null : temporary1;
			temporary1 = temporary2;
			temporary2 = swap;
		}

		Graphics.Blit (temporary1, _dest);

		if(temporary1 != null && temporary1 != _src) RenderTexture.ReleaseTemporary (temporary1);
		if(temporary2 != null) RenderTexture.ReleaseTemporary (temporary2);
	}

	public void Shake (RenderTexture _src, RenderTexture _dest)
	{
		if (mTime < 1) 
		{
			mTime += Time.deltaTime;

			mMat.SetFloat("_ShakeTime", (1F - Mathf.Abs((mTime - 0.5F) * 2F)));

			Graphics.Blit (_src, _dest, mMat);
		}
		else
		{
			Graphics.Blit (_src, _dest);
		}
	}

	public void Blood (RenderTexture _src, RenderTexture _dest)
	{
		Graphics.Blit (_src, _dest, mMat2);
	}
}
