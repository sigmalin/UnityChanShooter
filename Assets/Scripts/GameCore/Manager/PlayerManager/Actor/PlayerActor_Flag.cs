using UnityEngine;
using System.Collections;

public partial class PlayerActor
{
	public class Flags
	{
		public const int NONE = 0x00;
		public const int STUN = 0x01;
		public const int FORM_CHANGE = 0x02;
	}
		
	public int Flag { private set; get; }

	void InitialFlag()
	{
		Flag = Flags.NONE;
	}

	void SetFlag(int _flag, bool _enable)
	{
		if (_enable == true)
			Flag |= _flag;
		else
			Flag &= ~_flag;
	}

	public bool HasFlag(int _flag)
	{
		return (Flag & _flag) != Flags.NONE;
	}

	void SetStun(bool _enable)
	{
		mMotionData.LockActor = 0u;
		SetFlag (Flags.STUN, _enable);
	}

	void SetFormChange(bool _enable)
	{
		SetFlag (Flags.FORM_CHANGE, _enable);
	}
}
