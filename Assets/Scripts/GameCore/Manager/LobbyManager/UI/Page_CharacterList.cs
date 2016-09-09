using UnityEngine;
using System.Collections;

public sealed partial class Page_CharacterList : LobbyBehaviour 
{
	public class InstSet
	{
		public const uint UPDATE_CHARACTER_LIST = 0;
	}

	// Use this for initialization
	void Start () 
	{
		InitialMask ();	
	}

	void OnDestroy()
	{
		ReleaseItemPool ();
	}

	public override bool HandleInput ()
	{
		return true;
	}

	public override void SendCommand(uint _inst, params System.Object[] _params)
	{
		switch(_inst)
		{
		case InstSet.UPDATE_CHARACTER_LIST:
			UpdateView ();
			break;
		}
	}
}
