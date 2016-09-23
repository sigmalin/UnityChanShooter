using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class WeaponUiBehavior : MonoBehaviour, IInput, IUserInterface 
{
	public class InstSet
	{
		public const uint SET_ACTOR_ID = 1;
	}

	uint mPlayerID = 0u;
	protected uint PlayerID { get { return mPlayerID; } }

	public IInput Operator { get { return this; } }

	Subject<Unit> mOnHandleInputSubject = new Subject<Unit>();
	protected IObservable<Unit> InputStream { get { return mOnHandleInputSubject.AsObservable(); } }

	[System.Serializable]
	public struct WeaponUiText
	{
		public string Group;
		public int ID;
		public UnityEngine.UI.Text Text;
	}

	[SerializeField]
	WeaponUiText[] mWeaponUiText;

	[SerializeField]
	JoyStick mJoyStick;
	protected JoyStick Device { get { return mJoyStick; } }

	[System.Serializable]
	public struct WeaponUiButton
	{
		public GameObject ButtonGO;
		public ObservablePointerDownTrigger DownTrigger;
		public ObservablePointerUpTrigger UpTrigger;
	}

	public virtual void Start () 
	{
		if (mJoyStick != null) 
			mJoyStick.Initial (InputStream);
	}

	public virtual void Show(Transform _root)
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

	public virtual void Hide()
	{
		this.gameObject.SetActive (false);

		this.transform.SetParent(null);
	}

	public virtual void Localization()
	{
		if (mWeaponUiText == null)
			return;

		mWeaponUiText.ToObservable()
			.Where(_ => _.Text != null)
			.Subscribe(_ => _.Text.text = (string)GameCore.GetParameter(ParamGroup.GROUP_REPOSITORY, RepositoryParam.GET_LOCALIZATION, _.Group, _.ID));
	}

	public virtual void Clear()
	{
	}

	public virtual void Operation(uint _inst, params System.Object[] _params)
	{
		switch (_inst) 
		{
		case InstSet.SET_ACTOR_ID:
			mPlayerID = (uint)_params[0];
			break;
		}
	}

	// Update is called once per frame
	public virtual bool HandleInput ()
	{
		mOnHandleInputSubject.OnNext (Unit.Default);

		return false;
	}
}
