using UnityEngine;
using System.Collections;

public class Claw : MonoBehaviour, IArm 
{
	public Transform Muzzle { get { return null; } }

	[SerializeField]
	AudioSource mAudio;

	public void OnFire (uint _shooterID, uint _bulletID, uint _atk, int _layer)
	{
		Transform shooterPos = (Transform)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_TRANSFORM, _shooterID);
		if (shooterPos == null)
			return;

		GameObject bullet = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.BULLET, _bulletID);
		if (bullet == null)
			return;
		
		bullet.transform.position = shooterPos.position;
		bullet.transform.rotation = shooterPos.rotation;


		Vector3 attackPos = shooterPos.position + (shooterPos.forward * 1f) + (shooterPos.up * 0.5f);

		Collider[] cols = Physics.OverlapSphere (attackPos, 1f, _layer);

		for (int Indx = 0; Indx < cols.Length; ++Indx) 
		{
			ITarget target = cols[Indx].gameObject.GetComponent<ITarget> ();
			if (target != null)
				GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.ADD_FIRE_DAMAGE, target.ActorID, _shooterID, _atk, attackPos);
		}
	}

	public void OnPullTrigger(int _count)
	{
		if (mAudio != null) 
		{
			mAudio.Play ();
		}
	}

	public void PreLoad ()
	{
	}
}
