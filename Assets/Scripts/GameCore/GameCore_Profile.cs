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

			HoldCharacterList = new uint[] {101u};
		}
	}

	Profile mProfile;
	public Profile UserProfile { get { return mProfile; } }

	void InitialProfile()
	{
		mProfile = new Profile ();
	}
}
