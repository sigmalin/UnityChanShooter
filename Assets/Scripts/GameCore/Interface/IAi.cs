using UnityEngine;
using System.Collections;

public interface IAi
{
	uint ActorID { get; }

	void Initial (uint _actorID);

	void Release ();

	void Think ();
}
