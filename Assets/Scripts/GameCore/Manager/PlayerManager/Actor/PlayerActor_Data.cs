using UnityEngine;
using System.Collections;

public partial class PlayerActor
{
	[System.Serializable]
	public class ActorData
	{
		public Animator Anim;
		public Collider Col;
		public Rigidbody Rigid;
		public NavMeshAgent Agent;
	}

	[SerializeField]
	ActorData mActorData;
	public ActorData Actordata { get { return mActorData; } }

	public class PlayerMotionData
	{
		public Vector3 Move;
		public float Speed;

		public Vector3 CameraFocusOn;
		public uint LockActor;
		public uint SubLockActor;
	}

	PlayerMotionData mMotionData = new PlayerMotionData ();
	public PlayerMotionData MotionData { get { return mMotionData; } }

	void InitialNavMeshAgent()
	{
		if (mActorData.Agent != null) 
		{
			if (mActorData.Agent.enabled == false) 
			{
				mActorData.Agent.enabled = true;
				mActorData.Agent.updatePosition = false;
				mActorData.Agent.updateRotation = false;
			}
		}
	}

	void StopNavMeshAgent()
	{
		if (mActorData.Agent != null) 
		{
			mActorData.Agent.Stop ();
			mActorData.Agent.speed = 0f;

			mActorData.Agent.ResetPath ();

			mActorData.Agent.enabled = false;
		}
	}

	void ResetMotionData()
	{
		mMotionData.Move = Vector3.zero;
		mMotionData.Speed = 0f;
		mMotionData.CameraFocusOn = Vector3.zero;
		mMotionData.LockActor = 0u;
	}
}
