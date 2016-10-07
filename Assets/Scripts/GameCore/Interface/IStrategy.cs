using UnityEngine;
using System.Collections;

public interface IStrategy
{
	uint StrategyID { get; set; }

	void Exec (IAi _owner, System.Action _onCompleted = null);

	bool Observe (IAi _owner);
}
