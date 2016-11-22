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

		public ReactiveProperty<float> Stamina { get; set; }

		public ReadOnlyReactiveProperty<float> Life { get; set; }

		public ReadOnlyReactiveProperty<float> Charge { get; set; }

		public ReadOnlyReactiveProperty<bool> IsUsable { get; set; }

		public ReadOnlyReactiveProperty<bool> IsDead { get; set; }

		public float ReloadTime { get; set; }

		public int Team { get; set; }

		public ReactiveProperty<uint> Murderer { get; set; }

		public Subject<uint> Victims { get; set; }

		public ReactiveProperty<int> Flag { get; set; }

		public Vector3 HitPt { get; set; }

		public IAbility[] Abilities { get; set; }

		public WeaponDataRepository.WeaponData RefWeaponData { get; set; }
		public uint BulletID { get { return RefWeaponData.BulletID; } }
		public uint ShootATK { get { return RefWeaponData.ATK; } }
		public uint MaxHP { get { return RefWeaponData.HP; } }
		public uint MaxAmmoCount { get { return RefWeaponData.AmmoCount; } }
		public uint MaxShootRayCount { get { return RefWeaponData.ShootRayCount; } }
		public float MaxReloadTime { get { return RefWeaponData.ReloadTime; } }
		public float MaxShootFreq { get { return RefWeaponData.ShootFreq; } }
		public float MoveSpeed { get { return RefWeaponData.Speed; } }
		public float AttackRange { get { return RefWeaponData.Range; } }
		public float Impact { get { return RefWeaponData.Impact; } }
		public float MaxStamina { get { return RefWeaponData.Stamina; } }
	}

	Dictionary<uint, WeaponActor> mActorTable = null;

	void InitialWeaponActor()
	{
		mActorTable = new Dictionary<uint, WeaponActor> ();
	}

	void RegisterActor(uint _actorID, uint _characterID)
	{
		if (mActorTable == null)
			return;

		if (mActorTable.ContainsKey (_actorID) == true)
			return;

		WeaponActor actor = new WeaponActor ();

		CharacterRepository.CharacterData characterData = GetCharacterData (_characterID);

		actor.ActorID = _actorID;
		actor.CharacterID = characterData.ID;
		actor.RefWeaponData = GetWeaponData (characterData.weaponID);

		if (actor.HP == null) actor.HP = new ReactiveProperty<uint> (actor.MaxHP);
		actor.Life = actor.HP.Select (_ => (float)_ / (float)actor.MaxHP).ToReadOnlyReactiveProperty ();
		actor.IsDead = actor.HP.Select (_ => _ <= 0u).ToReadOnlyReactiveProperty ();
		actor.IsDead.Where (_ => _ == true).Subscribe (_ => ActorDead (actor.ActorID, actor.Murderer.Value));

		if (actor.AmmoCount == null) actor.AmmoCount = new ReactiveProperty<uint> (actor.RefWeaponData.AmmoCount);
		actor.Charge = actor.AmmoCount.Select (_ => ((float)_ / (float)actor.RefWeaponData.AmmoCount)).ToReadOnlyReactiveProperty ();
		actor.IsUsable = actor.AmmoCount.Select(_ => _ != 0u).ToReadOnlyReactiveProperty ();

		actor.ReloadTime = 0F;

		actor.Stamina = new ReactiveProperty<float> (actor.MaxStamina);
		actor.Stamina
			.Where (_ => _ <= 0f)
			.Subscribe (_ => 
				{
					actor.Stamina.Value = actor.MaxStamina;
					GameCore.SendCommand(CommandGroup.GROUP_PLAYER, PlayerInst.PLAYER_DAMAGE, actor.ActorID);
				}
			);

		actor.Murderer = new ReactiveProperty<uint> (0u);
		actor.Victims = new Subject<uint> ();

		actor.Flag = new ReactiveProperty<int> (Flags.NONE);

		mActorTable.Add (_actorID, actor);

		actor.Abilities = new IAbility[] 
		{
			GetAbility (_actorID, characterData.ability1ID),
			GetAbility (_actorID, characterData.ability2ID),
			GetAbility (_actorID, characterData.ability3ID),
		};

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

	void SetActorFlag(uint _actorID, int _flag, bool _enable)
	{
		WeaponActor actor = GetWeaponActor (_actorID);
		if (actor == null)
			return;

		if (_enable == true)
			actor.Flag.Value |= _flag;
		else
			actor.Flag.Value &= ~_flag;
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

		mActorTable [_actorID].IsUsable.Dispose ();
		mActorTable [_actorID].IsUsable = null;

		mActorTable [_actorID].IsDead.Dispose ();
		mActorTable [_actorID].IsDead = null;

		mActorTable [_actorID].Victims.Dispose ();
		mActorTable [_actorID].Victims = null;

		ReleaseAbility (mActorTable [_actorID]);

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
