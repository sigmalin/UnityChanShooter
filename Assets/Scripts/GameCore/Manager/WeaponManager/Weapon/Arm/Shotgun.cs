using UnityEngine;
using System.Collections;

public class Shotgun : MonoBehaviour, IArm 
{
	[SerializeField]
	Transform[] mMuzzle;
	public Transform Muzzle { get { return mMuzzle == null ? null : mMuzzle [Random.Range (0, mMuzzle.Length)]; } }

	[SerializeField]
	GameObject mShootShock;

	[SerializeField]
	Transform mShootShockPt;

	[SerializeField]
	ParticleSystem mAmmo;

	[SerializeField]
	AudioSource mAudio;

	public void OnFire (uint _shooterID, uint _bulletID, uint _atk, int _layer)
	{
		GameObject bullet = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.BULLET, _bulletID);
		if (bullet == null)
			return;

		BeamRay shoot = bullet.GetComponent<BeamRay> ();
		if (shoot == null)
			return;


		Transform muzzle = Muzzle;

		bullet.transform.position = muzzle.position;

		Vector3 forward = muzzle.forward;

		Ray ray = new Ray (muzzle.position + forward * -0.5F, forward);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 100F, _layer) == true) 
		{
			shoot.BeamEnd = hit.point;

			Collider[] cols = Physics.OverlapSphere (hit.point, 2.5f, _layer);

			for (int Indx = 0; Indx < cols.Length; ++Indx) 
			{
				ITarget target = cols[Indx].gameObject.GetComponent<ITarget> ();
				if (target != null)
					GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.ADD_FIRE_DAMAGE, target.ActorID, _shooterID, _atk, hit.point);
			}
		} 
		else 
		{
			shoot.BeamEnd = muzzle.position + forward * 10F;
		}
	}

	public void OnPullTrigger(int _count)
	{
		if (mShootShock != null && mShootShockPt != null) 
		{
			mShootShock.transform.position = mShootShockPt.position;
			mShootShock.transform.rotation = mShootShockPt.rotation;
			mShootShock.SetActive (true);
		}

		if (mAmmo != null) 
		{
			mAmmo.Emit (_count);
		}

		if (mAudio != null) 
		{
			mAudio.Play ();
		}
	}

	public void PreLoad ()
	{
		if (mAmmo != null) 
		{
			mAmmo.Emit (1);
		}
	}
}
