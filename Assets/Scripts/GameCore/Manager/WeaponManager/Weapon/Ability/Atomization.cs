using UnityEngine;
using System.Collections;
using UniRx;

public class Atomization : IAbility 
{
	public uint AbilityID { get { return AbilitySerial.TELEPORT; } }

	public bool IsPassive { get { return false; } }

	public ReadOnlyReactiveProperty<float> Charge { get; private set; }

	public ReadOnlyReactiveProperty<bool> IsUsable { get; private set; }

	public ReactiveProperty<float> ChargeTime { get; set; }

	uint mOwnerID = 0u;

	const float ABILITY_CHARGE_TIME = 30f;

	public void Initial (uint _ownerID)
	{
		Release ();

		ChargeTime = new ReactiveProperty<float> (ABILITY_CHARGE_TIME);

		Charge = ChargeTime.Select (_ => _ / ABILITY_CHARGE_TIME).ToReadOnlyReactiveProperty ();

		IsUsable = ChargeTime.Select (_ => _ == ABILITY_CHARGE_TIME).ToReadOnlyReactiveProperty ();

		mOwnerID = _ownerID;
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

		ClearDisposable ();
	}

	public void Use ()
	{
		if (IsUsable.Value == false)
			return;
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
	}
}
