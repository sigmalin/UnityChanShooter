using UnityEngine;
using System.Collections;
using UniRx;

public interface IAbility
{
	uint AbilityID { get; }

	bool IsPassive { get; }

	ReadOnlyReactiveProperty<float> Charge { get; }

	ReadOnlyReactiveProperty<bool> IsUsable { get; }

	void Initial (uint _ownerID);

	void FrameMove ();

	void Release ();

	void Use ();
}
