using UnityEngine;
using System.Collections;

public sealed partial class AiManager 
{
	IAi GenerateAI(int _aiID)
	{
		IAi ai = null;

		switch(_aiID)
		{
		case AiDefine.AI_ID_ZOOMBIE:
			ai = new Ai_Zoombie ();
			break;
		}

		return ai;
	}
}
