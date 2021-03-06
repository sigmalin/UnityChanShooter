﻿using UnityEngine;
using System.Collections;

public class ParamGroup
{
	public const uint GROUP_SYSTEM   		= 0;
	public const uint GROUP_PLAYER   		= 1;
	public const uint GROUP_CAMERA   		= 2;
	public const uint GROUP_RESOURCE 		= 3;
	public const uint GROUP_WEAPON   		= 4;
	public const uint GROUP_CACHE    		= 5;
	public const uint GROUP_LOBBY    		= 6;
	public const uint GROUP_REPOSITORY      = 7;
	public const uint GROUP_AI              = 8;
}

public class SystemParam
{
}

public class PlayerParam
{
	public const uint PLAYER_DATA = 10;

	public const uint PLAYER_TRANSFORM = 11;

	public const uint PLAYER_LOCK_TARGET = 12;

	public const uint PLAYER_DISTANCE_BETWEEN_LOCK_ACTOR = 20;
}

public class CameraParam
{
	public const uint MAIN_CAMERA = 0;

	public const uint CAMERA_OBJECT = 10;
}

public class ResourceParam
{
	public const uint CHARACTER_MODEL = 11;
	public const uint CONTAINER = 12;
	public const uint WEAPON_DATA = 13;
	public const uint WEAPON_MODEL = 14;
	public const uint BULLET = 15;
	public const uint RAGDOLL_MODEL = 16;
	public const uint ABILITY = 17;
	public const uint EFFECT = 18;

	public const uint INSTANT_RESOURCE_UI = 98;
	public const uint INSTANT_RESOURCE_INPUT = 99;
}

public class WeaponParam
{
	public const uint WEAPON_ACTOR_DATA = 1;
	public const uint WEAPON_ACTOR_SPEED = 2;
	public const uint WEAPON_ACTOR_RANGE = 3;
	public const uint WEAPON_ACTOR_FREQ = 4;

	public const uint GET_LASTEST_MURDERER = 5;

	public const uint GET_MAIN_ACTOR_ID = 6;

	public const uint GET_ALLY_LIST = 11;
	public const uint GET_HOSTILITY_LIST = 12;
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
	public const uint GET_PORTRAIT_PATH = 16;
	public const uint GET_LOCALIZATION_PATH = 17;
	public const uint GET_CHARACTER_DATA_PATH = 18;
	public const uint GET_CHAPTER_DATA_PATH = 19;
	public const uint GET_CHAPTER_IMAGE_PATH = 20;
	public const uint GET_STAGE_IMAGE_PATH = 21;
	public const uint GET_ABILITY_PATH = 22;
	public const uint GET_AUDIO_PATH = 23;
	public const uint GET_SPRITE_PATH = 24;
	public const uint GET_POST_EFFECT_PATH = 25;
	public const uint GET_EFFECT_PATH = 26;

	public const uint GET_INSTANT_RESOURCE_UI_PATH = 98;
	public const uint GET_INSTANT_RESOURCE_INPUT_PATH = 99;

	public const uint GET_CHARACTER = 111;
	public const uint GET_CONTAINER = 112;
	public const uint GET_WEAPON_DATA = 113;
	public const uint GET_WEAPON = 114;
	public const uint GET_BULLET = 115;
	public const uint GET_PORTRAIT = 116;
	public const uint GET_LOCALIZATION = 117;
	public const uint GET_CHARACTER_DATA = 118;
	public const uint GET_CHAPTER_DATA = 119;
	public const uint GET_CHAPTER_IMAGE= 120;
	public const uint GET_STAGE_IMAGE= 121;
	public const uint GET_ABILITY = 122;
	public const uint GET_AUDIO = 123;
	public const uint GET_SPRITE = 124;
	public const uint GET_POST_EFFECT = 125;
	public const uint GET_EFFECT = 126;

	public const uint GET_INSTANT_RESOURCE_UI = 198;
	public const uint GET_INSTANT_RESOURCE_INPUT = 199;
}

public class LobbyParam
{
}

public class RepositoryParam
{
	public const uint GET_LOCALIZATION = 1;

	public const uint GET_CHARACTER_DATA = 11;

	public const uint GET_CHAPTER_DATA = 21;
	public const uint GET_ALL_CHAPTER_DATA = 22;

	public const uint GET_WEAPON_DATA = 31;

	public const uint GET_SPRITE_DATA = 41;

	public const uint GET_POST_EFFECT_DATA = 51;
}

public class AiParam
{
	public const uint GET_STRATEGY_FREEZE = 0;
	public const uint GET_STRATEGY_IDLE = 1;
	public const uint GET_STRATEGY_TRACKING = 2;
	public const uint GET_STRATEGY_FACE_LOOK_TARGET = 3;
	public const uint GET_STRATEGY_HURT_PLAYER = 4;
	public const uint GET_STRATEGY_AIM = 5;
}

public interface IParam 
{
	System.Object GetParameter (uint _inst, params System.Object[] _params);
}
