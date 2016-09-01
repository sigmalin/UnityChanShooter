﻿using UnityEngine;
using System.Collections;

public static class Util
{
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