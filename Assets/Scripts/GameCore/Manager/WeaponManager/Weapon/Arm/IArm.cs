using UnityEngine;
using System.Collections;

public interface IArm 
{
	Transform Muzzle { get; }

	void OnFire (uint _shooterID, uint _bulletID, uint _atk, int _layer);

	void OnPullTrigger ();
}
