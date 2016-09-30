using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class GameCore
{
	[SerializeField]
	Transform mCanvas = null;

	[SerializeField]
	UnityEngine.EventSystems.EventSystem mEventSys;

	[SerializeField]
	UnityEngine.UI.GraphicRaycaster mRaycaster;

	List<UnityEngine.EventSystems.RaycastResult> GetRaycastResult(Vector2 _position)
	{
		UnityEngine.EventSystems.PointerEventData eventData = new UnityEngine.EventSystems.PointerEventData (mEventSys);
		eventData.pressPosition = _position;
		eventData.position = _position;

		List<UnityEngine.EventSystems.RaycastResult> res = new List<UnityEngine.EventSystems.RaycastResult> ();
		mRaycaster.Raycast (eventData, res);

		return res;
	}

	static public List<UnityEngine.EventSystems.RaycastResult> InterfaceRaycast(Vector2 _position)
	{
		if (Instance == null)
			return null;

		return Instance.GetRaycastResult (_position);
	}

	static public bool IsTouchInterface(Vector2 _position)
	{
		if (Instance == null)
			return false;

		return Instance.GetRaycastResult (_position).Count != 0;
	}

	static public bool IsTouchInterface(Vector2 _position, string _name)
	{
		if (Instance == null)
			return false;

		List<UnityEngine.EventSystems.RaycastResult> list = Instance.GetRaycastResult (_position);

		return list.Where (_ => string.Equals (_name, _.gameObject.name)).Count () != 0;
	}
}
