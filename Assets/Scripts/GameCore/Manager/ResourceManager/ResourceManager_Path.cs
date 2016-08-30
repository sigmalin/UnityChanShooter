using UnityEngine;
using System.Collections;

public partial class ResourceManager
{
	#region Bullet
	string GetBulletPath(uint _key)
	{
		return "Bullet/bullet";
	}

	string GetBulletAsset(uint _key)
	{
		return string.Format ("Bullet/{0}.prefab", _key.ToString ());
	}
	#endregion

	#region Character
	string GetCharacterPath(uint _key)
	{
		return string.Format ("Character/{0}/{0}", _key.ToString ());
	}

	string GetCharacterAsset(uint _key)
	{
		return string.Format ("{0}.prefab", GetCharacterPath(_key));
	}
	#endregion

	#region Container
	string GetContainerPath(uint _key)
	{
		return "Container/container";
	}

	string GetContainerAsset(uint _key)
	{
		return string.Format ("Container/{0}.prefab", _key.ToString ());
	}
	#endregion

	#region Data
	string GetDataPath(string _key)
	{
		return string.Format ("Data/{0}/{0}", _key);
	}

	string GetDataAsset(string _key)
	{
		return string.Format ("{0}.asset", GetDataPath(_key));
	}
	#endregion

	#region InstantRes
	string GetInstantResInputPath()
	{
		return "InstantRes/Input/input";
	}

	string GetInstantResInputAsset(string _key)
	{
		return string.Format ("InstantRes/Input/{0}.prefab", _key.ToString ());
	}
	#endregion

	#region Weapon
	string GetWeaponPath(uint _key)
	{
		return string.Format ("Weapon/{0}/{0}", _key.ToString ());
	}

	string GetWeaponAsset(uint _key)
	{
		return string.Format ("{0}.prefab", GetWeaponPath(_key));
	}
	#endregion
}
