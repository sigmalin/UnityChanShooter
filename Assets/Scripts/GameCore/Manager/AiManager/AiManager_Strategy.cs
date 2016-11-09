using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public sealed partial class AiManager 
{
	Dictionary<uint, ResourcePool<IStrategy>> mStrategyTable = null;

	public class StrategyIDs
	{
		public const uint FREEZE = 0;
		public const uint IDLE = 1;
		public const uint TRACKING = 2;
		public const uint FACE_LOOK_TARGET = 3;
		public const uint HURT_PLAYER = 4;
		public const uint AIM = 5;
	}

	void InitialStrategyTable()
	{
		if (mStrategyTable == null)
			mStrategyTable = new Dictionary<uint, ResourcePool<IStrategy>> ();
	}

	void ReleaseStrategyTable()
	{
		uint[] keys = mStrategyTable.Keys.ToArray ();

		for (int Indx = 0; Indx < keys.Length; ++Indx) 
		{
			if (mStrategyTable [keys[Indx]] != null) 
			{
				mStrategyTable [keys[Indx]].Destroy ();
				mStrategyTable [keys[Indx]] = null;
			}
		}

		mStrategyTable.Clear ();
	}

	System.Object GetStrategy(uint _key)
	{
		System.Object res = null;

		if (mStrategyTable == null)
			return res;

		if (mStrategyTable.ContainsKey (_key) == false)
			ProduceStrategyResPool(_key);

		res = (System.Object)mStrategyTable [_key].Produce ();

		return res;
	}

	void RecycleStrategy(IStrategy _res)
	{
		if (mStrategyTable == null || _res == null)
			return;

		if (mStrategyTable.ContainsKey (_res.StrategyID) == false)
			ProduceStrategyResPool(_res.StrategyID);

		mStrategyTable [_res.StrategyID].Recycle (_res);
	}

	IStrategy LoadStrategy(uint _key)
	{
		IStrategy strategy = null;

		switch (_key) 
		{
		case StrategyIDs.FREEZE:
			strategy = new Strategy_Freeze ();
			break;

		case StrategyIDs.IDLE:
			strategy = new Strategy_Idle ();
			break;

		case StrategyIDs.TRACKING:
			strategy = new Strategy_Tracking ();
			break;

		case StrategyIDs.FACE_LOOK_TARGET:
			strategy = new Strategy_FaceLockTarget ();
			break;

		case StrategyIDs.HURT_PLAYER:
			strategy = new Strategy_HurtMainPlayer ();
			break;

		case StrategyIDs.AIM:
			strategy = new Strategy_Aim ();
			break;
		}

		if (strategy != null)
			strategy.StrategyID = _key;

		return strategy;
	}

	void ProduceStrategyResPool(uint _key)
	{
		ResourcePool<IStrategy> _pool = new ResourcePool<IStrategy> (
			() => {	return LoadStrategy (_key); },
			null,
			null,
			4
		);

		mStrategyTable.Add (_key, _pool);			
	}

}
