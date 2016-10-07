using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public sealed class UiVectory : MonoBehaviour, IInput, IUserInterface 
{
	public IInput Operator { get { return this; } }

	Subject<Unit> mOnHandleInputSubject = new Subject<Unit>();
	protected IObservable<Unit> InputStream { get { return mOnHandleInputSubject.AsObservable(); } }

	[System.Serializable]
	public struct UiText
	{
		public string Group;
		public int ID;
		public UnityEngine.UI.Text Text;
	}

	[SerializeField]
	UiText[] mUiText;

	[SerializeField]
	UnityEngine.UI.Button mMask;

	public void Start () 
	{
		if (mMask != null) 
		{
			mMask.OnClickAsObservable()
				.Subscribe(_ => GameCore.SetNextFlow(null));
		}
	}

	public void Show(Transform _root)
	{
		this.transform.SetParent(_root);

		RectTransform rectTrans = this.GetComponent<RectTransform> ();

		if (rectTrans != null)
		{
			rectTrans.anchorMin = new Vector2 (0F,0F);
			rectTrans.anchorMax = new Vector2 (1F,1F);
			rectTrans.pivot = new Vector2 (0.5F, 0.5F);
			rectTrans.offsetMax = new Vector2 (0F,0F);
			rectTrans.offsetMin = new Vector2 (0F,0F);
		}

		this.gameObject.SetActive (true);
	}

	public void Hide()
	{
		this.gameObject.SetActive (false);

		this.transform.SetParent(null);
	}

	public void Localization()
	{
		if (mUiText == null)
			return;

		mUiText.ToObservable()
			.Where(_ => _.Text != null)
			.Subscribe(_ => _.Text.text = (string)GameCore.GetParameter(ParamGroup.GROUP_REPOSITORY, RepositoryParam.GET_LOCALIZATION, _.Group, _.ID));
	}

	public void Clear()
	{
	}

	public void Operation(uint _inst, params System.Object[] _params)
	{
	}

	// Update is called once per frame
	public bool HandleInput ()
	{
		mOnHandleInputSubject.OnNext (Unit.Default);

		return false;
	}
}
