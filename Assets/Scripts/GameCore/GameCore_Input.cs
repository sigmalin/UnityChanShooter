using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class GameCore
{
	Stack<IInput> mInputStack = null; 

	void InitialInput()
	{
		if (mInputStack == null)
			mInputStack = new Stack<IInput> ();

		mInputStack.Clear ();
	}

	void HandleInput()
	{
		if (mInputStack == null)
			return;

		if (mInputStack.Count == 0)
			return;

		IInput input = mInputStack.Pop ();

		if (input == null) 
		{
			HandleInput ();
		} 
		else 
		{
			if (input.HandleInput () == false) 
				HandleInput ();

			mInputStack.Push (input);
		}
	}

	void ExecPushInput(IInput _input)
	{
		if (mInputStack == null)
			return;
		
		if (_input == null)
			return;

		mInputStack.Push (_input);

		if (_input.UserInterface != null)
			_input.UserInterface.Show ();
	}

	IInput ExecPopInput()
	{
		if (mInputStack == null)
			return null;

		if (mInputStack.Count == 0)
			return null;

		IInput input = mInputStack.Pop ();

		if (input.UserInterface != null)
			input.UserInterface.Hide ();

		return input;
	}

	static public void PushInput(IInput _input)
	{
		if (_input == null)
			return;

		if (Instance == null)
			return;

		Instance.ExecPushInput (_input);
	}

	static public void PopInput()
	{
		if (Instance == null)
			return;

		Instance.ExecPopInput ();
	}

	static public void PopInputUntil(IInput _input)
	{
		if (Instance == null)
			return;

		IInput result = null;

		do {
			result = Instance.ExecPopInput ();
		} while (result != null && result != _input);
	}
}
