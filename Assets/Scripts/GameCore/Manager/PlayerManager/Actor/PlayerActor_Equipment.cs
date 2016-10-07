using UnityEngine;
using System.Collections;
using UniRx;

public partial class PlayerActor
{
	System.IDisposable mAnimatorMoveDisposable;
	System.IDisposable mAnimatorIKDisposable;

	Role mRole;
	public Role PlayerRole { get { return mRole; } }

	WeaponLauncher mLauncher;
	public WeaponLauncher Launcher { get { return mLauncher; } }

	ActorController mActorController = null;
	public ActorController Controller { get { return mActorController; } }

	uint mModelID;
	public uint ModelID { get { return mModelID; } }

	void ReleaseAll()
	{
		ClearWeaponModel ();

		ClearRoleModel ();

		this.gameObject.SafeRecycle ();
	}

	void SetActorController(ActorController _actorCtrl)
	{
		mActorController = _actorCtrl;

		if (mActorController != null)
			mActorController.Initial (this);
	}

	void SetRoleModel(uint _modelID)
	{
		ClearRoleModel ();

		mModelID = _modelID;

		GameObject roleGO = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.CHARACTER_MODEL, mModelID);

		if (roleGO == null)
			return;

		mRole = roleGO.GetComponent<Role> ();

		if (mRole == null)
			return;

		mAnimatorMoveDisposable = mRole.OnAnimatorMoveAsObservable
			.Select (_ => mActorController)
			.Where (_ => _ != null)
			.Subscribe (_ => _.OnAnimMove ());

		mAnimatorIKDisposable = mRole.OnAnimatorIKAsObservable
			.Select (_ => mActorController)
			.Where (_ => _ != null)
			.Subscribe (_ => _.OnAnimIK ());

		mRole.transform.parent = this.transform;
		mRole.transform.localPosition = Vector3.zero;
		mRole.transform.localRotation = Quaternion.identity;
		mRole.transform.localScale = Vector3.one;

		mActorData.Anim = roleGO.GetComponent<Animator> ();

		Equipment ();
	}

	void ClearRoleModel()
	{
		if (mAnimatorMoveDisposable != null) 
		{
			mAnimatorMoveDisposable.Dispose ();
			mAnimatorMoveDisposable = null;
		}

		if (mAnimatorIKDisposable != null) 
		{
			mAnimatorIKDisposable.Dispose ();
			mAnimatorIKDisposable = null;
		}

		if (mRole != null) 
		{
			mRole.gameObject.SafeRecycle ();
			mRole = null;
		}

		mActorData.Anim = null;

		mModelID = 0;
	}

	void SetWeaponModel(uint _weaponID)
	{
		ClearWeaponModel ();

		GameObject launcherGO = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.WEAPON_MODEL, _weaponID);

		if (launcherGO == null)
			return;

		mLauncher = launcherGO.GetComponent<WeaponLauncher>();

		Equipment ();
	}

	void ClearWeaponModel()
	{
		if (mLauncher == null)
			return;

		if (mLauncher.RightHand != null)
			mLauncher.RightHand.transform.parent = mLauncher.transform;

		if (mLauncher.LeftHand != null)
			mLauncher.LeftHand.transform.parent = mLauncher.transform;

		mLauncher.gameObject.SafeRecycle ();
		mLauncher = null;
	}

	void Equipment()
	{
		if (mRole == null || mLauncher == null)
			return;

		if (mLauncher.RightHand != null) 
		{
			mLauncher.RightHand.transform.parent = mRole.BodyPt.RightHand;
			mLauncher.RightHand.transform.localPosition = Vector3.zero;
			mLauncher.RightHand.transform.localRotation = Quaternion.identity;
		}

		if (mLauncher.LeftHand != null) 
		{
			mLauncher.LeftHand.transform.parent = mRole.BodyPt.LeftHand;
			mLauncher.LeftHand.transform.localPosition = Vector3.zero;
			mLauncher.LeftHand.transform.localRotation = Quaternion.identity;
		}
	}
}
