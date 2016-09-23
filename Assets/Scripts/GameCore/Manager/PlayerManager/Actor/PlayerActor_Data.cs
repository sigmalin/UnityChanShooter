using UnityEngine;
using System.Collections;

public partial class PlayerActor
{
	public class PlayerMotionData
	{
		public Vector3 Move;
		public float Speed;
		public Vector3 LookAt;
	}

	PlayerMotionData mMotionData = new PlayerMotionData ();
	public PlayerMotionData MotionData { get { return mMotionData; } }
}
