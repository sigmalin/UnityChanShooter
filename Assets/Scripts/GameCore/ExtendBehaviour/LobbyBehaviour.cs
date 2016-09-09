using UnityEngine;
using System.Collections;

public class LobbyBehaviour : MonoBehaviour, IInput, IUserInterface 
{
	public Transform Owner;

	public IInput Operator { get { return this; } }

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

	public virtual void SendCommand(uint _inst, params System.Object[] _params)
	{
	}
}
