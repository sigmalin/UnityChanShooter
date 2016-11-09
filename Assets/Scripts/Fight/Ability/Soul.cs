using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Soul : MonoBehaviour 
{
	public uint SoulOwnerID { get; set; }

	// Use this for initialization
	void Start () 
	{
		this.OnTriggerEnterAsObservable ()
			.Select(_ => _.gameObject.GetComponent<ITarget>())
			.Where(_ => _ != null && _.ActorID == SoulOwnerID)
			.Subscribe (_ => EatSoul());	
	}

	void EatSoul()
	{
		GameCore.SendCommand (ParamGroup.GROUP_WEAPON, WeaponInst.HEAL, SoulOwnerID, 30u);

		this.gameObject.SafeRecycle ();
	}
}
