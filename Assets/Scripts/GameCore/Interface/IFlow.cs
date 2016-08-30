using UnityEngine;
using System.Collections;

public class FlowEvent
{
	public const uint VERSION_VERFITY_COMPLETED = 0;
	public const uint VERSION_VERFITY_FAILURE = 1;

	public const uint VERSION_INCOMPATIBLE = 2;

	public const uint LOAD_CACHE_COMPLETED = 3;
	public const uint LOAD_CACHE_FAILURE = 4;
}

public interface IFlow 
{
	void Enter();

	void Exit ();

	void Event (uint _eventID);
}
