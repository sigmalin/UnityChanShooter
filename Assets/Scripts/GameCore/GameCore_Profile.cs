using UnityEngine;
using System.Collections;

public partial class GameCore 
{
	public class Profile
	{
		public uint MainCharacterID;

		public uint[] HoldCharacterList;

		public Profile()
		{
			MainCharacterID = 101u;

			HoldCharacterList = new uint[] {101u, /*102u, 103u*/};
		}
	}

	Profile mProfile;
	public static Profile UserProfile { get { return mInstance == null ? null : mInstance.mProfile; } }

	void InitialProfile()
	{
		mProfile = new Profile ();
	}
}
