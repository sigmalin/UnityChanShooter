using UnityEngine;
using System.Collections;

public interface IMotion
{
	float Weight { get; }

	void UpdateWeight(float _varWeight);

	void EnterMotion(PlayerActor _owner);

	void LeaveMotion(PlayerActor _owner);

	void UpdateMotion(PlayerActor _owner);

	void AnimMoveMotion(PlayerActor _owner);

	void AnimIKMotion(PlayerActor _owner);

	void AnimEventMotion(PlayerActor _owner, string _event);
}
