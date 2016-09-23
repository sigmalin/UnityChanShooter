using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public sealed partial class WeaponManager
{
	public class WeaponActor
	{
		public uint ActorID { get; set; }

		public uint HP { get; set; }

		public uint AmmoCount { get; set; }

		public float ReloadTime { get; set; }

		public float ShootFreq { get; set; }

		public WeaponDataRepository.WeaponData RefWeaponData { get; set; }
		public uint BulletID { get { return RefWeaponData.BulletID; } }
		public uint ShootATK { get { return RefWeaponData.ATK; } }
		public uint MaxAmmoCount { get { return RefWeaponData.AmmoCount; } }
		public uint MaxShootRayCount { get { return RefWeaponData.ShootRayCount; } }
		public float MaxReloadTime { get { return RefWeaponData.ReloadTime; } }
		public float MaxShootFreq { get { return RefWeaponData.ShootFreq; } }
	}

	Dictionary<uint, WeaponActor> mActorTable = null;

	void InitialWeaponActor()
	{
		mActorTable = new Dictionary<uint, WeaponActor> ();
	}

	void RegisterActor(uint _actorID, uint _weaponID)
	{
		if (mActorTable == null)
			return;

		if (mActorTable.ContainsKey (_actorID) == true)
			return;

		WeaponActor actor = new WeaponActor ();

		actor.ActorID = _actorID;
		actor.RefWeaponData = GetWeaponData (_weaponID);

		actor.HP = actor.RefWeaponData.HP;
		actor.AmmoCount = actor.RefWeaponData.AmmoCount;

		actor.ReloadTime = 0F;
		actor.ShootFreq = 0F;

		mActorTable.Add (_actorID, actor);


		SetActorController (ref actor);
		SetWeaponModel (ref actor);
	}

	WeaponActor GetWeaponActor(uint _actorID)
	{
		if (mActorTable == null)
			return null;

		if (mActorTable.ContainsKey (_actorID) == false)
			return null;

		return mActorTable [_actorID];
	}

	void RemoveActor(uint _actorID)
	{
		if (mActorTable == null)
			return;

		if (mActorTable.ContainsKey (_actorID) == false)
			return;

		mActorTable.Remove (_actorID);
	}

	void RemoveAllActor()
	{
		if (mActorTable == null)
			return;
		
		uint[] actorIDs = GetAllActorID ();

		for (int Indx = 0; Indx < actorIDs.Length; ++Indx)
			RemoveActor (actorIDs[Indx]);
	}

	uint[] GetAllActorID()
	{
		return mActorTable.Keys.ToArray ();
	}
}
