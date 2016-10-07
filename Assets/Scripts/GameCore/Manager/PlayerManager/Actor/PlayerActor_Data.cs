using UnityEngine;
using System.Collections;

public partial class PlayerActor
{
	public class PlayerMotionData
	{
		public Vector3 Move;
		public float Speed;

		public Vector3 FocusOn;
		public uint LockActor;

		public Vector3 LookAt
		{
			get 
			{
				PlayerActor actor = (PlayerActor)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_DATA, LockActor);
				if (actor == null || actor.PlayerRole == null)
					return FocusOn;
				else
					return actor.PlayerRole.BodyPt.AimPt.position;
			}
		}
	}

	PlayerMotionData mMotionData = new PlayerMotionData ();
	public PlayerMotionData MotionData { get { return mMotionData; } }
}
