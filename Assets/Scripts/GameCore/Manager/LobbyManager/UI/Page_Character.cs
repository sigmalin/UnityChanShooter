using UnityEngine;
using System.Collections;

public sealed partial class Page_Character : LobbyBehaviour 
{
	public class InstSet
	{
		public const uint CREATE_ANALYSIS_GRAPHIC = 0;
	}

	// Use this for initialization
	void Start () 
	{
		InitialMask ();	
	}

	public override bool HandleInput ()
	{
		return true;
	}

	public override void SendCommand(uint _inst, params System.Object[] _params)
	{
		switch(_inst)
		{
		case InstSet.CREATE_ANALYSIS_GRAPHIC:
			CreateAnalysisGraphic ();
			break;
		}
	}
}
