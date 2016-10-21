using UnityEngine;
using System.Collections;

public static class NavMeshAgentUtil
{
	public static bool IsArrived(this NavMeshAgent _agent)
	{
		if (Vector3.Distance (_agent.destination, _agent.transform.position) <= _agent.stoppingDistance) 
		{
			//if (_agent.hasPath == false || _agent.velocity.sqrMagnitude == 0f)
				return true;
		}

		return false;
	}
}
