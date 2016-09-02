using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class GameCore
{
	Stack<IUserInterface> mInterfaceStack = null; 

	void InitialInput()
	{
		if (mInterfaceStack == null)
			mInterfaceStack = new Stack<IUserInterface> ();

		mInterfaceStack.Clear ();
	}

	void HandleInput()
	{
		if (mInterfaceStack == null)
			return;

		if (mInterfaceStack.Count == 0)
			return;

		IUserInterface userInterface = mInterfaceStack.Pop ();

		if (userInterface == null) 
		{
			HandleInput ();
		} 
		else 
		{
			if (userInterface.Operator == null || userInterface.Operator.HandleInput() == false) 
			{
				HandleInput ();
			}				

			mInterfaceStack.Push (userInterface);
		}
	}

	void ExecPushInterface(IUserInterface _interface)
	{
		if (mInterfaceStack == null)
			return;

		if (mInterfaceStack.Contains (_interface) == true)
			return;

		mInterfaceStack.Push (_interface);

		_interface.Show (mCanvas);
	}

	void ExecPopInterface(IUserInterface _interface)
	{
		if (mInterfaceStack == null)
			return;

		if (mInterfaceStack.Count == 0)
			return;

		IUserInterface userInterface = mInterfaceStack.Pop ();

		if (_interface == userInterface) 
		{
			userInterface.Hide ();
		} 
		else 
		{
			ExecPopInterface (_interface);

			mInterfaceStack.Push (_interface);
		}
	}

	static public void PushInterface(IUserInterface _interface)
	{
		if (Instance == null || _interface == null)
			return;

		Instance.ExecPushInterface (_interface);
	}

	static public void PopPopInterface(IUserInterface _interface)
	{
		if (Instance == null || _interface == null)
			return;

		Instance.ExecPopInterface (_interface);
	}
}
