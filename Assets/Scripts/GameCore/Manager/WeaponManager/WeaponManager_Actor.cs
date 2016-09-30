using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public sealed partial class WeaponManager
{
	public class WeaponActor
	{
		public uint ActorID { get; set; }

		public uint CharacterID { get; set; }

		public ReactiveProperty<uint> HP { get; set; }

		public ReactiveProperty<uint> AmmoCount { get; set; }

		public ReadOnlyReactiveProperty<float> Life { get; set; }

		public ReadOnlyReactiveProperty<float> Charge { get; set; }

		public ReadOnlyReactiveProperty<bool> IsDead { get; set; }

		public float ReloadTime { get; set; }

		public float ShootFreq { get; set; }

		public int Team { get; set; }

		public uint MurdererID { get; set; }

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

	void RegisterActor(uint _actorID, uint _characterID, uint _weaponID)
	{
		if (mActorTable == null)
			return;

		if (mActorTable.ContainsKey (_actorID) == true)
			return;

		WeaponActor actor = new WeaponActor ();

		actor.ActorID = _actorID;
		actor.CharacterID = _characterID;
		actor.RefWeaponData = GetWeaponData (_weaponID);

		if (actor.HP == null) actor.HP = new ReactiveProperty<uint> (actor.RefWeaponData.HP);
		actor.Life = actor.HP.Select (_ => (float)_ / (float)actor.RefWeaponData.HP).ToReadOnlyReactiveProperty ();
		actor.IsDead = actor.HP.Select (_ => _ <= 0u).ToReadOnlyReactiveProperty ();
		actor.IsDead.Where (_ => _ == true).Subscribe (_ => ActorDead (actor.ActorID, actor.MurdererID));

		if (actor.AmmoCount == null) actor.AmmoCount = new ReactiveProperty<uint> (actor.RefWeaponData.AmmoCount);
		actor.Charge = actor.AmmoCount.Select (_ => ((float)_ / (float)actor.RefWeaponData.AmmoCount)).ToReadOnlyReactiveProperty ();

		actor.ReloadTime = 0F;
		actor.ShootFreq = 0F;

		actor.MurdererID = 0u;

		mActorTable.Add (_actorID, actor);


		SetActorController (actor);
		SetWeaponModel (actor);

		PreloadResource (actor);
	}

	WeaponActor GetWeaponActor(uint _actorID)
	{
		if (mActorTable == null)
			return null;

		if (mActorTable.ContainsKey (_actorID) == false)
			return null;

		return mActorTable [_actorID];
	}

	void SetActorTeam(uint _actorID, int _team)
	{
		WeaponActor actor = GetWeaponActor (_actorID);
		if (actor == null)
			return;

		actor.Team = _team;

		SetActorLayer (actor);
	}

	void RemoveActor(uint _actorID)
	{
		if (mActorTable == null)
			return;

		if (mActorTable.ContainsKey (_actorID) == false)
			return;

		mActorTable [_actorID].HP.Dispose ();
		mActorTable [_actorID].HP = null;

		mActorTable [_actorID].Life.Dispose ();
		mActorTable [_actorID].Life = null;

		mActorTable [_actorID].AmmoCount.Dispose ();
		mActorTable [_actorID].AmmoCount = null;

		mActorTable [_actorID].Charge.Dispose ();
		mActorTable [_actorID].Charge = null;

		mActorTable [_actorID].IsDead.Dispose ();
		mActorTable [_actorID].IsDead = null;

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
