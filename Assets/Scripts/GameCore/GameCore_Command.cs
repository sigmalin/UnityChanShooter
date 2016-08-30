using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class GameCore
{
	Dictionary<uint, ICommand> mCmdDict = null;

	void InitialCommand()
	{
		if (mCmdDict == null) 
			mCmdDict = new Dictionary<uint, ICommand> ();
	}

	void TransCommand(uint _group, uint _inst, params System.Object[] _params)
	{
		if (mCmdDict == null)
			return;

		if (mCmdDict.ContainsKey (_group) == false)
			return;

		mCmdDict[_group].ExecCommand (_inst, _params);
	}

	static public void RegisterCommand(uint _group, ICommand _cmd)
	{
		if (_cmd == null)
			return;

		if (Instance == null)
			return;

		if (Instance.mCmdDict == null)
			return;

		if (Instance.mCmdDict.ContainsKey (_group) == true)
			Instance.mCmdDict.Remove (_group);

		Instance.mCmdDict.Add (_group, _cmd);
	}

	static public void UnRegisterCommand(uint _group)
	{
		if (Instance == null)
			return;

		if (Instance.mCmdDict == null)
			return;

		if (Instance.mCmdDict.ContainsKey (_group) == true)
			Instance.mCmdDict.Remove (_group);
	}

	static public void SendCommand(uint _group, uint _inst, params System.Object[] _params)
	{
		if (Instance == null)
			return;
		
		Instance.TransCommand (_group, _inst, _params);
	}
}
