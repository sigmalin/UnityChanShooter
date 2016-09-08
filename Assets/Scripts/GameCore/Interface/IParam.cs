using UnityEngine;
using System.Collections;

public class ParamGroup
{
	public const uint GROUP_SYSTEM   = 0;
	public const uint GROUP_PLAYER   = 1;
	public const uint GROUP_CAMERA   = 2;
	public const uint GROUP_RESOURCE = 3;
	public const uint GROUP_WEAPON   = 4;
	public const uint GROUP_CACHE    = 5;
	public const uint GROUP_LOBBY    = 6;
}

public class SystemParam
{
}

public class PlayerParam
{
	public const uint MAIN_PLAYER = 0;

	public const uint PLAYER_DATA = 10;

	public const uint PLAYER_TRANSFORM = 11;

	public const uint MAIN_PLAYER_DATA = 20;
}

public class CameraParam
{
	public const uint MAIN_CAMERA = 0;

	public const uint CAMERA_DATA = 10;

	public const uint MAIN_CAMERA_DATA = 20;
}

public class ResourceParam
{
	public const uint CHARACTER_MODEL = 11;
	public const uint CONTAINER = 12;
	public const uint WEAPON_DATA = 13;
	public const uint WEAPON_MODEL = 14;
	public const uint BULLET = 15;

	public const uint INSTANT_RESOURCE_INPUT = 99;
}

public class WeaponParam
{
	public const uint MODEL_ID = 1;
}

public class CacheParam
{
	public const uint GET_CACHE = 1;

	public const uint GET_SCENE_PATH = 10;
	public const uint GET_CHARACTER_PATH = 11;
	public const uint GET_CONTAINER_PATH = 12;
	public const uint GET_WEAPON_DATA_PATH = 13;
	public const uint GET_WEAPON_PATH = 14;
	public const uint GET_BULLET_PATH = 15;

	public const uint GET_INSTANT_RESOURCE_INPUT_PATH = 99;

	public const uint GET_CHARACTER = 111;
	public const uint GET_CONTAINER = 112;
	public const uint GET_WEAPON_DATA = 113;
	public const uint GET_WEAPON = 114;
	public const uint GET_BULLET = 115;

	public const uint GET_INSTANT_RESOURCE_INPUT = 199;
}

public class LobbyParam
{
}

public interface IParam 
{
	System.Object GetParameter (uint _inst, params System.Object[] _params);
}
