using UnityEngine;
using System.Collections;

public static class Util
{
	public static T GetOrAddComponent<T>(this GameObject _go) where T : MonoBehaviour
	{
		T behaviour = _go.GetComponent<T> ();
		if (behaviour == null)
			behaviour = _go.AddComponent<T> ();

		return behaviour;
	}

	public static void SafeRecycle(this GameObject _go)
	{
		if (_go == null)
			return;

		GameCoreResRecycle recycle = _go.GetComponent<GameCoreResRecycle> ();
		if (recycle != null) 
			recycle.Recycle ();
		else 
			GameObject.Destroy (_go);
	}

	public static bool IsPointAhead(this Transform _center, Vector3 _pt)
	{
		Vector3 dir = (_pt - _center.position).normalized;
		return _center.forward.IsAhead (dir);
	}

	public static bool IsPointRightSide(this Transform _center, Vector3 _pt)
	{
		Vector3 dir = (_pt - _center.position).normalized;
		return _center.forward.IsRightSide (dir);
	}

	public static bool IsAhead(this Vector3 _forward, Vector3 _dir)
	{
		return 0F < Vector3.Dot(_forward, _dir);
	}

	public static bool IsRightSide(this Vector3 _forward, Vector3 _dir)
	{
		return 0F < Vector3.Cross(_forward, _dir).y;
	}

	public static bool IsGround(this Collider _col)
	{
		float distToGround = _col.bounds.extents.y;
		return Physics.Raycast (_col.transform.position + Vector3.up * distToGround * 0.5F, -Vector3.up, distToGround + distToGround * 0.25F);
	}
}
