using UnityEngine;
using System.Collections;

public class CommandGroup
{
	public const uint GROUP_SYSTEM   		= 0;
	public const uint GROUP_PLAYER   		= 1;
	public const uint GROUP_CAMERA   		= 2;
	public const uint GROUP_RESOURCE 		= 3;
	public const uint GROUP_WEAPON   		= 4;
	public const uint GROUP_CACHE    		= 5;
	public const uint GROUP_LOBBY    		= 6;
	public const uint GROUP_REPOSITORY      = 7;
}

public class SystemInst
{
	public const uint GAME_QUIT = 0;
}

public class PlayerInst
{
	public const uint CREATE_PLAYER = 0;

	public const uint SET_MODEL = 1;
	public const uint SET_CONTAINER = 2;
	public const uint SET_WEAPON = 3;
	public const uint REMOVE_PLAYER = 4;
	public const uint SET_ACTOR_CONTROLLER = 5;

	public const uint GAME_START = 7;
	public const uint GAME_SUSPEND = 8;
	public const uint GAME_END = 9;

	public const uint SET_POSITION = 11;
	public const uint SET_DIRECTION = 12;
	public const uint SET_LAYER = 13;

	public const uint PLAYER_IDLE = 20;
	public const uint PLAYER_MOVE = 21;
	public const uint PLAYER_JUMP = 22;
	public const uint PLAYER_ROTATE = 23;
	public const uint PLAYER_AIM = 24;
	public const uint PLAYER_LOOKAT = 25;
}

public class CameraInst
{
	public const uint CAMERA_REGISTER = 0;
	public const uint MAIN_CAMERA = 1;
	public const uint CAMERA_TARGET = 2;
	public const uint CAMERA_UNREGISTER = 3;

	public const uint CAMERA_MOVEMENT = 10;

	public const uint SET_CAMERA_MODE = 100;
}

public class ResourceInst
{
	public const uint RELEASE_ALL = 0;
	public const uint RELEASE_CHARACTER_MODEL = 1;
	public const uint RELEASE_CONTAINER = 2;
	public const uint RELEASE_WEAPON = 3;
	public const uint RELEASE_BULLET = 4;

	public const uint RECYCLE_CHARACTER_MODEL = 11;
	public const uint RECYCLE_CONTAINER = 12;
	public const uint RECYCLE_WEAPON_MODEL = 13;
	public const uint RECYCLE_BULLET = 14;
}

public class WeaponInst
{
	public const uint REGISTER_ACTOR = 2;
	public const uint REMOVE_ALL_ACTOR = 3;
	public const uint REMOVE_ACTOR = 4;
	public const uint MAIN_ACTOR = 5;

	public const uint ARM_FIRE = 10;
}

public class CacheInst
{
	public const uint VERSION_VERIFY = 0;
	public const uint RELEASE_ALL_CACHE = 1;

	public const uint READ_CACHE = 2;
	public const uint RELEASE_CACHE = 3;

	public const uint REPORT_LOAD_STATE = 4;
	public const uint REPORT_READ_STATE = 5;
	public const uint DOWN_LOAD_CACHE = 6;
}

public class LobbyInst
{
	public const uint UPDATE_MAIN_CHARACTER = 0;

	public const uint SHOW_LOBBY_DIALOG = 1;
	public const uint HIDE_LOBBY_DIALOG = 2;

	public const uint ENTER_PAGE_CHARACTER = 11;
	public const uint SWITCH_TO_PAGE_CHARACTER_LIST = 12;
	public const uint SWITCH_TO_PAGE_CHARACTER_STATE = 13;
	public const uint EXIT_PAGE_CHARACTER = 14;

	public const uint ENTER_PAGE_SINGLE = 15;
	public const uint SWITCH_TO_PAGE_SINGLE_LIST = 16;
	public const uint SWITCH_TO_PAGE_SINGLE_STAGE = 17;
	public const uint SELECT_CHAPTER = 19;
	public const uint EXIT_PAGE_SINGLE = 20;
}

public class RepositoryInst
{
	public const uint LOAD_LOCALIZATION = 1;
	public const uint LOAD_CHARACTER_DATA = 11;
	public const uint LOAD_CHAPTER_DATA = 21;
	public const uint LOAD_WEAPON_DATA = 31;
}

public interface ICommand 
{
	void ExecCommand (uint _inst, params System.Object[] _params);

	void BatchUpdate (float _delta);

	void BatchLateUpdate (float _delta);
}
