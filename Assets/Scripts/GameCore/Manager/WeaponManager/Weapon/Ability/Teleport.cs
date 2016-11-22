using UnityEngine;
using System.Collections;
using UniRx;

public class Teleport : IAbility 
{
	public uint AbilityID { get { return AbilitySerial.TELEPORT; } }

	public bool IsPassive { get { return false; } }

	public ReadOnlyReactiveProperty<float> Charge { get; private set; }

	public ReadOnlyReactiveProperty<bool> IsUsable { get; private set; }

	public ReactiveProperty<float> ChargeTime { get; set; }

	uint mOwnerID = 0u;

	IMode mTeleportCameraMode;

	System.IDisposable mTeleportDisposable = null;

	Shake mShake = null;

	const float ABILITY_CHARGE_TIME = 30f;

	public void Initial (uint _ownerID)
	{
		Release ();

		ChargeTime = new ReactiveProperty<float> (ABILITY_CHARGE_TIME);

		Charge = ChargeTime.Select (_ => _ / ABILITY_CHARGE_TIME).ToReadOnlyReactiveProperty ();

		IsUsable = ChargeTime.Select (_ => _ == ABILITY_CHARGE_TIME).ToReadOnlyReactiveProperty ();

		mOwnerID = _ownerID;

		mTeleportCameraMode = new Mode_BlackOut ();

		mShake = new Shake ();
	}

	public void FrameMove ()
	{
		if (ChargeTime != null && ChargeTime.Value < ABILITY_CHARGE_TIME) 
		{
			ChargeTime.Value = Mathf.Clamp (ChargeTime.Value + Time.deltaTime, 0f, ABILITY_CHARGE_TIME);
		}
	}

	public void Release ()
	{
		if (ChargeTime != null) 
		{
			ChargeTime.Dispose ();
			ChargeTime = null;
		}

		if (Charge != null) 
		{
			Charge.Dispose ();
			Charge = null;
		}

		mTeleportCameraMode = null;

		ClearDisposable ();
	}

	public void Use ()
	{
		if (IsUsable.Value == false)
			return;

		uint[] enemyIDs = (uint[])GameCore.GetParameter (ParamGroup.GROUP_WEAPON, WeaponParam.GET_HOSTILITY_LIST, mOwnerID);
		if (enemyIDs == null || enemyIDs.Length == 0)
			return;

		uint targetID = enemyIDs [Random.Range (0, enemyIDs.Length)];

		Transform ownerTrans = (Transform)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_TRANSFORM, mOwnerID);
		if (ownerTrans == null)
			return;

		Transform targetTrans = (Transform)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_TRANSFORM, targetID);
		if (targetTrans == null)
			return;

		ChargeTime.Value = 0F;
		
		Vector3 ownerPos = ownerTrans.position;
		Vector3 targetPos = targetTrans.position;

		ClearDisposable ();

		mTeleportDisposable = Observable.Defer<long>
			(
				() => 
				{
					CreateEffect(ownerPos);
					CreateEffect(targetPos);
					LockActor(mOwnerID, targetID, true);
					return Observable.Timer(System.TimeSpan.FromSeconds(2f));
				}
			)
			.Do
			( 
				_ => 
				{
					uint mainCameraID = (uint)GameCore.GetParameter(ParamGroup.GROUP_CAMERA, CameraParam.MAIN_CAMERA);
					//GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.SET_CAMERA_MODE, mainCameraID, mTeleportCameraMode);
					GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.SET_CAMERA_POST_EFFECT, mainCameraID, mShake);

					GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.SET_POSITION, mOwnerID, targetPos);
					GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.SET_POSITION, targetID, ownerPos);
				}
			)
			.SelectMany(_ => Observable.Timer(System.TimeSpan.FromSeconds(1f)))
			.Subscribe(_ => 
				{
					uint mainCameraID = (uint)GameCore.GetParameter(ParamGroup.GROUP_CAMERA, CameraParam.MAIN_CAMERA);
					//GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.REMOVE_CAMERA_MODE, mainCameraID, mTeleportCameraMode);
					GameCore.SendCommand (CommandGroup.GROUP_CAMERA, CameraInst.REMOVE_CAMERA_POST_EFFECT, mainCameraID, mShake);

					LockActor(mOwnerID, targetID, false);
				})
			.AddTo(ownerTrans.gameObject);
	}

	void LockActor(uint ownerID, uint targetID, bool _isLock)
	{
		// Lock Ai
		GameCore.SendCommand (CommandGroup.GROUP_AI, AiInst.FREEZE_AI, ownerID, _isLock);
		GameCore.SendCommand (CommandGroup.GROUP_AI, AiInst.FREEZE_AI, targetID, _isLock);

		// Lock Actor
		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_STUN, ownerID, _isLock);
		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_STUN, targetID, _isLock);

		// set Invicible
		GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.SET_INVINCIBLE, ownerID, _isLock);
		GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.SET_INVINCIBLE, targetID, _isLock);
	}

	void CreateEffect(Vector3 _pos)
	{
		GameObject ability = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.ABILITY, AbilityID);
		if (ability == null)
			return;

		ability.transform.position = _pos;
	}

	void ClearDisposable()
	{
		if (mTeleportDisposable != null) 
		{
			mTeleportDisposable.Dispose ();
			mTeleportDisposable = null;
		}
	}
}
