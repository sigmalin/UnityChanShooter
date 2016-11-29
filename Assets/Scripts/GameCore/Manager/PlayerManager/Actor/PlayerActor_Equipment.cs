using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public partial class PlayerActor
{
	System.IDisposable mAnimatorMoveDisposable;
	System.IDisposable mAnimatorIKDisposable;
	System.IDisposable mAnimatorEventDisposable;

	Role mRole;
	public Role PlayerRole { get { return mRole; } }

	WeaponLauncher mLauncher;
	public WeaponLauncher Launcher { get { return mLauncher; } }

	Stack<ActorController> mControllerStack;
	public ActorController Controller { get { return mControllerStack == null ? null : mControllerStack.Peek(); } }

	uint mModelID;
	public uint ModelID { get { return mModelID; } }

	void ReleaseAll()
	{
		ClearActorController ();

		ClearWeaponModel ();

		ClearRoleModel ();

		this.gameObject.SafeRecycle ();
	}

	void PushActorController(ActorController _actorCtrl)
	{
		if (mControllerStack == null)
			mControllerStack = new Stack<ActorController> ();
		
		if (_actorCtrl != null && mControllerStack.Contains(_actorCtrl) == false) 
		{
			mControllerStack.Push (_actorCtrl);
			_actorCtrl.Initial (this);
		}
	}

	void PopActorController(ActorController _actorCtrl)
	{
		if (mControllerStack == null)
			return;

		Stack<ActorController> tempStack = new Stack<ActorController> ();

		while (mControllerStack.Count != 0) 
		{
			ActorController ctrl = mControllerStack.Pop ();
			if (_actorCtrl != ctrl)
				tempStack.Push (ctrl);
		}

		while (tempStack.Count != 0) 
		{
			mControllerStack.Push (tempStack.Pop());
		}
	}

	void ClearActorController()
	{
		if (mControllerStack != null) 
		{
			mControllerStack.Clear ();

			mControllerStack = null;
		}
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
			.Select (_ => Controller)
			.Where (_ => _ != null)
			.Subscribe (_ => _.OnAnimMove ());

		mAnimatorIKDisposable = mRole.OnAnimatorIKAsObservable
			.Select (_ => Controller)
			.Where (_ => _ != null)
			.Subscribe (_ => _.OnAnimIK ());

		mAnimatorEventDisposable = mRole.OnAnimatorEventAsObservable
			.Where (_ => Controller != null)
			.Subscribe (_ => Controller.OnAnimEvent (_));

		mRole.transform.parent = this.transform;
		mRole.transform.localPosition = Vector3.zero;
		mRole.transform.localRotation = Quaternion.identity;
		mRole.transform.localScale = Vector3.one;

		mActorData.Anim = roleGO.GetComponent<Animator> ();

		InitStateMachineBehaviour (mActorData.Anim);

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

		if (mAnimatorEventDisposable != null) 
		{
			mAnimatorEventDisposable.Dispose ();
			mAnimatorIKDisposable = null;
		}

		if (mRole != null) 
		{
			mRole.gameObject.SafeRecycle ();
			mRole = null;
		}

		ReleaseStateMachineBehaviour ();

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

			if (mLauncher.RightArm != null)
				mLauncher.RightArm.PreLoad ();
		}

		if (mLauncher.LeftHand != null) 
		{
			mLauncher.LeftHand.transform.parent = mRole.BodyPt.LeftHand;
			mLauncher.LeftHand.transform.localPosition = Vector3.zero;
			mLauncher.LeftHand.transform.localRotation = Quaternion.identity;

			if (mLauncher.LeftArm != null)
				mLauncher.LeftArm.PreLoad ();
		}
	}
}
