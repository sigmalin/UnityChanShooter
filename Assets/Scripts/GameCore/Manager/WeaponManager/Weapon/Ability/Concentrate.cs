using UnityEngine;
using System.Collections;
using UniRx;

public class Concentrate : IAbility 
{
	public uint AbilityID { get { return AbilitySerial.CONCENTRATE; } }

	public bool IsPassive { get { return false; } }

	public ReadOnlyReactiveProperty<float> Charge { get; private set; }

	public ReadOnlyReactiveProperty<bool> IsUsable { get; private set; }

	public ReactiveProperty<float> ChargeTime { get; set; }

	uint mOwnerID = 0u;

	bool mIsUsed = false;

	GameObject mEffect = null;

	const float ABILITY_CONCENTRATE_TIME = 10f;

	const float ABILITY_RECOVER_FREQ = 0.5f;

	public void Initial (uint _ownerID)
	{
		Release ();

		ChargeTime = new ReactiveProperty<float> (ABILITY_CONCENTRATE_TIME);

		Charge = ChargeTime.Select (_ => _ / ABILITY_CONCENTRATE_TIME).ToReadOnlyReactiveProperty ();

		IsUsable = ChargeTime.Select(_ => 0f < _).ToReadOnlyReactiveProperty ();

		ChargeTime
			.Where (_ => _ <= 0f)
			.Subscribe (_ => FormChange (false));

		mOwnerID = _ownerID;

		mIsUsed = false;

		mEffect = null;
	}

	public void FrameMove ()
	{
		if (ChargeTime != null) 
		{
			if (mIsUsed == true) 
			{
				ChargeTime.Value = Mathf.Clamp (ChargeTime.Value - Time.deltaTime, 0f, ABILITY_CONCENTRATE_TIME);
			} 
			else if (ChargeTime.Value < ABILITY_CONCENTRATE_TIME) 
			{
				ChargeTime.Value = Mathf.Clamp (ChargeTime.Value + (Time.deltaTime * ABILITY_RECOVER_FREQ), 0f, ABILITY_CONCENTRATE_TIME);
			}
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

		if (mEffect != null) 
		{
			mEffect.SafeRecycle ();
			mEffect = null;
		}
	}

	public void Use ()
	{
		FormChange (!mIsUsed);
	}

	void FormChange(bool _isEnabled)
	{
		mIsUsed = _isEnabled;
		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_FORM_CHANGE, mOwnerID, mIsUsed);

		if (mIsUsed == true && mEffect == null) 
		{
			Transform transOwner = (Transform)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_TRANSFORM, mOwnerID);
			if (transOwner == null)
				return;

			mEffect = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_RESOURCE, ResourceParam.ABILITY, AbilityID);
			if (mEffect == null)
				return;

			FollowEffect follow = mEffect.GetOrAddComponent<FollowEffect> ();
			follow.Target = transOwner;
		} 
		else if (mIsUsed == false && mEffect != null) 
		{
			mEffect.SafeRecycle ();
			mEffect = null;
		}
	}
}
