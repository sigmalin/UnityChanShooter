using UnityEngine;
using System.Collections;

public interface IMotion
{
	void EnterMotion(PlayerActor _owner);

	void LeaveMotion(PlayerActor _owner);

	void UpdateMotion(PlayerActor _owner);

	void AnimMoveMotion(PlayerActor _owner);

	void AnimIKMotion(PlayerActor _owner);
}
