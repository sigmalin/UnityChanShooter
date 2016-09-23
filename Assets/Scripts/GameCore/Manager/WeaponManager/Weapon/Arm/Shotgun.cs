using UnityEngine;
using System.Collections;

public class Shotgun : MonoBehaviour, IArm 
{
	[SerializeField]
	Transform[] mMuzzle;
	public Transform Muzzle { get { return mMuzzle == null ? null : mMuzzle [Random.Range (0, mMuzzle.Length)]; } }

	[SerializeField]
	GameObject mShootShock;

	public void OnFire (uint _shooterID, uint _bulletID, uint _atk)
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

		int layer = GameCore.GetRaycastLayer (GameCore.LAYER_DEFAULT) |
			GameCore.GetRaycastLayer (GameCore.LAYER_ENEMY);

		if (Physics.Raycast (ray, out hit, 100F, layer) == true) 
		{
			shoot.BeamEnd = hit.point;
		} 
		else 
		{
			shoot.BeamEnd = muzzle.position + forward * 10F;
		}
	}

	public void OnPullTrigger()
	{
		if (mShootShock != null)
			mShootShock.SetActive (true);
	}
}
