using UnityEngine;
using System.Collections;

public class WeaponDataRepository : ScriptableObject
{
	public enum Behavior
	{
		None,
		Shootgun,
		Zombie,
	}

	[System.Serializable]
	public struct WeaponData
	{
		public uint WeaponID;
		public uint ModelID;
		public uint BulletID;

		public Behavior WeaponBehavior;

		public uint ShootRayCount;
		public uint AmmoCount;

		public uint HP;
		public uint ATK;

		public float ReloadTime;
		public float ShootFreq;

		public float Speed;
		public float Range;

		public float Impact;
		public float Stamina;
	}

	[SerializeField]
	WeaponData[] mWeaponDataList;

	public WeaponData[] WeaponDataList { get { return mWeaponDataList; } }
}