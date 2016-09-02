using UnityEngine;
using System.Collections;

public class FlowBehaviour : MonoBehaviour, IFlow
{
	[SerializeField]
	GameObject[] mManagerList;

	// Use this for initialization
	void Start () 
	{
		GameCore.SetNextFlow (this);
	}

	public virtual void Enter()
	{
		RegisterManager ();
	}

	public virtual void Exit ()
	{
		UnRegisterManager ();
	}

	public virtual void Event (uint _eventID)
	{
	}

	void RegisterManager()
	{
		if (mManagerList == null)
			return;

		for (int Indx = 0; Indx < mManagerList.Length; ++Indx) 
		{
			GameObject manager = mManagerList [Indx];
			if (manager != null) 
			{
				IRegister[] regs = manager.GetComponents<IRegister> ();

				for (int regIndx = 0; regIndx < regs.Length; ++regIndx) 
				{
					regs [regIndx].OnRegister ();
				}
			}
		}
	}

	void UnRegisterManager()
	{
		if (mManagerList == null)
			return;

		for (int Indx = 0; Indx < mManagerList.Length; ++Indx) 
		{
			GameObject manager = mManagerList [Indx];
			if (manager != null) 
			{
				IRegister[] regs = manager.GetComponents<IRegister> ();

				for (int regIndx = 0; regIndx < regs.Length; ++regIndx) 
				{
					regs [regIndx].OnUnRegister ();
				}
			}
		}
	}
}
