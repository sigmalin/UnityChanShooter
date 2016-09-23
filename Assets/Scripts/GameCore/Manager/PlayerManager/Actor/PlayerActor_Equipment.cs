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

	// Update is called once per frame
	public void FrameMove () 
	{
		if (mActorController == null)
			return;

		mActorController.OnUpdate ();
	}

	void ReleaseAll()
	{
		ClearWeaponModel ();

		ClearRoleModel ();

		RecycleResource (this.gameObject);
	}

	void SetActorController(ActorController _actorCtrl)
	{
		mActorController = _actorCtrl;

		if (mActorController != null)
			mActorController.Initial (this);
	}

	void SetRoleModel(GameObject _roleGO)
	{
		ClearRoleModel ();

		if (_roleGO == null)
			return;

		mRole = _roleGO.GetComponent<Role> ();

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

		mActorData.Anim = _roleGO.GetComponent<Animator> ();

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
			RecycleResource (mRole.gameObject);
			mRole = null;
		}

		mActorData.Anim = null;
	}

	void SetWeaponModel(GameObject _launcherGO)
	{
		ClearWeaponModel ();

		if (_launcherGO == null)
			return;

		mLauncher = _launcherGO.GetComponent<WeaponLauncher>();

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

		RecycleResource (mLauncher.gameObject);

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
