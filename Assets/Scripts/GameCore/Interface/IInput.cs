using UnityEngine;
using System.Collections;

public interface IInput
{
	IUserInterface UserInterface { get; }

	bool HandleInput ();
}
