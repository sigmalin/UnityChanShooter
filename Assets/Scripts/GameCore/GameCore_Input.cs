using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public partial class GameCore
{
	Stack<IUserInterface> mInterfaceStack = null; 

	void InitialInput()
	{
		ReleaseInput ();

		mInterfaceStack = new Stack<IUserInterface> ();
	}

	void ReleaseInput()
	{
		if (mInterfaceStack != null) 
		{
			mInterfaceStack.Clear ();

			mInterfaceStack = null;
		}
	}

	void HandleInput()
	{
		if (mInterfaceStack == null)
			return;

		IUserInterface[] interfaceList = mInterfaceStack.ToArray ();

		bool needRepack = false;

		interfaceList.ToObservable()
			.Do(_ => { if(_ == null) needRepack = true; }) 
			.Where(_ => _ != null)
			.TakeWhile(_ => _.Operator == null || _.Operator.HandleInput() == false)
			.Subscribe(_ => {});

		if (needRepack == true)
			RepackInterfaceStack ();

		/*
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
		*/
	}

	void RepackInterfaceStack()
	{
		if (mInterfaceStack == null)
			return;

		Stack<IUserInterface> temp = new Stack<IUserInterface> ();

		while (mInterfaceStack.Count != 0) 
		{
			IUserInterface ui = mInterfaceStack.Pop ();
			if(ui != null) temp.Push (ui);
		}

		while (temp.Count != 0) 
		{
			IUserInterface ui = temp.Pop ();
			if(ui != null) mInterfaceStack.Push (ui);
		}
	}

	void ExecPushInterface(IUserInterface _interface)
	{
		if (mInterfaceStack == null)
			return;

		if (mInterfaceStack.Contains (_interface) == true)
			return;

		if (mInterfaceStack.Count != 0)
			mInterfaceStack.Peek ().Clear ();

		mInterfaceStack.Push (_interface);

		_interface.Localization ();

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

			mInterfaceStack.Push (userInterface);
		}
	}

	void ExecLocalization()
	{
		if (mInterfaceStack == null)
			return;

		IUserInterface[] interfaceList = mInterfaceStack.ToArray ();

		interfaceList.ToObservable ()
			.Where (_ => _ != null)
			.Subscribe (_ => _.Localization ());
	}

	static public void PushInterface(IUserInterface _interface)
	{
		if (Instance == null || _interface == null)
			return;

		Instance.ExecPushInterface (_interface);
	}

	static public void PopInterface(IUserInterface _interface)
	{
		if (Instance == null || _interface == null)
			return;

		Instance.ExecPopInterface (_interface);
	}

	static public void Localization()
	{
		if (Instance == null)
			return;

		Instance.ExecLocalization ();
	}
}
