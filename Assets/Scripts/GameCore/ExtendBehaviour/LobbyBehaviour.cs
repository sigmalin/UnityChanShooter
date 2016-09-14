using UnityEngine;
using System.Collections;
using UniRx;

public class LobbyBehaviour : MonoBehaviour, IInput, IUserInterface 
{
	public Transform Owner;

	public IInput Operator { get { return this; } }

	public IUserInterface Ui { get { return this; } }

	[System.Serializable]
	public struct LobbyText
	{
		public string Group;
		public int ID;
		public UnityEngine.UI.Text Text;
	}

	[SerializeField]
	LobbyText[] mLobbyText;

	public virtual bool HandleInput ()
	{
		return true;
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

		this.transform.SetParent(Owner);
	}

	public virtual void Localization()
	{
		if (mLobbyText == null)
			return;

		mLobbyText.ToObservable()
			.Where(_ => _.Text != null)
			.Subscribe(_ => _.Text.text = (string)GameCore.GetParameter(ParamGroup.GROUP_REPOSITORY, RepositoryParam.GET_LOCALIZATION, _.Group, _.ID));
	}

	public virtual void Clear()
	{
	}

	public virtual void Operation(uint _inst, params System.Object[] _params)
	{
	}

	public virtual void LobbyOrder(uint _order)
	{
	}
}
