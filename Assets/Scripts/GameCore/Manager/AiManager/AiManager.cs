using UnityEngine;
using System.Collections;

public sealed partial class AiManager : CommandBehaviour, IParam, IRegister 
{
	// Use this for initialization
	void Start () 
	{
		InitialAiTable ();	
	}
	
	public void OnRegister ()
	{
		GameCore.RegisterCommand (CommandGroup.GROUP_AI, this);	

		GameCore.RegisterParam (ParamGroup.GROUP_AI, this);

		InitialRequestQueue ();

		InitialAiUpdate ();

		InitialStrategyTable ();
	}

	public void OnUnRegister ()
	{
		ReleaseStrategyTable ();

		ReleaseAiUpdate ();

		ReleaseRequestQueue ();

		ClearAiData ();

		GameCore.UnRegisterCommand (CommandGroup.GROUP_AI);

		GameCore.UnRegisterParam (ParamGroup.GROUP_AI);
	}

	//public void ExecCommand (uint _inst, params System.Object[] _params)
	protected override void BatchCommand (uint _inst, params System.Object[] _params)
	{
		switch (_inst) 
		{
		case AiInst.REGISTER_AI:
			RegisterAi ((uint)_params [0], (int)_params [1]);
			break;

		case AiInst.REMOVE_AI:
			RemoveAi ((uint)_params [0]);
			break;

		case AiInst.RECYCLE_STRATEGY:
			RecycleStrategy ((IStrategy)_params[0]);
			break;

		case AiInst.FREEZE_AI:
			FreezeAi ((uint)_params [0], (bool)_params [1]);
			break;

		default:
			break;
		}
	}

	public System.Object GetParameter (uint _inst, params System.Object[] _params)
	{
		System.Object output = default(System.Object);

		switch (_inst) 
		{
		case AiParam.GET_STRATEGY_FREEZE:
			output = GetStrategy (StrategyIDs.FREEZE);
			break;

		case AiParam.GET_STRATEGY_IDLE:
			output = GetStrategy (StrategyIDs.IDLE);
			break;

		case AiParam.GET_STRATEGY_TRACKING:
			output = GetStrategy (StrategyIDs.TRACKING);
			break;

		case AiParam.GET_STRATEGY_FACE_LOOK_TARGET:
			output = GetStrategy (StrategyIDs.FACE_LOOK_TARGET);
			break;

		case AiParam.GET_STRATEGY_HURT_PLAYER:
			output = GetStrategy (StrategyIDs.HURT_PLAYER);
			break;

		case AiParam.GET_STRATEGY_AIM:
			output = GetStrategy (StrategyIDs.AIM);
			break;

		default:
			break;
		}

		return output;
	}
}
