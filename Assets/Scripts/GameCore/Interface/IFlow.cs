﻿using UnityEngine;
using System.Collections;

public class FlowEvent
{
	public const uint VERSION_VERFITY_COMPLETED = 0;
	public const uint VERSION_VERFITY_FAILURE = 1;

	public const uint VERSION_INCOMPATIBLE = 2;

	public const uint READ_CACHE_COMPLETED = 3;
	public const uint READ_CACHE_FAILURE = 4;
	public const uint READ_CACHE_UNDONE = 5;

	public const uint DOWN_LOAD_CACHE_COMPLETED = 6;
	public const uint DOWN_LOAD_CACHE_FAILURE = 7;
	public const uint DOWN_LOAD_CACHE_UNDONE = 8;

	public const uint CONNECT_FAILURED = 9;

	public const uint MAIN_ACTOR_DEAD = 21;
	public const uint ALL_ENEMY_DEAD = 22;
}

public interface IFlow 
{
	void Enter();

	void Exit ();

	void Event (uint _eventID);
}
