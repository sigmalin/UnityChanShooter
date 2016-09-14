using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed partial class LobbyDialog 
{
	[SerializeField]
	UnityEngine.UI.Text mTitle;

	[SerializeField]
	UnityEngine.UI.Text mContext;

	struct DialogLocalization
	{
		public string Group;
		public int ID;
	}

	struct DialogData
	{
		public DialogLocalization Title;
		public DialogLocalization Context;
	}

	Stack<DialogData> mStack = null;

	void InitialDialogStack()
	{
		ReleaseDialogStack ();

		mStack = new Stack<DialogData> ();
	}

	void ReleaseDialogStack()
	{
		if (mStack != null) 
		{
			mStack.Clear ();

			mStack = null;
		}
	}

	void AddDialogData(string _titleGroup, int _titleID, string _contextGroup, int _contextID)
	{
		if (mStack == null)
			InitialDialogStack();

		mStack.Push(new DialogData() { 
			Title = new DialogLocalization(){ Group = _titleGroup, ID = _titleID },
			Context = new DialogLocalization(){ Group = _contextGroup, ID = _contextID } });

		UpdateDialog ();
	}

	void RemoveDialogData()
	{
		if (mStack == null) 
		{
			GameCore.SendCommand (CommandGroup.GROUP_LOBBY, LobbyInst.HIDE_LOBBY_DIALOG);
		} 
		else 
		{
			mStack.Pop ();

			if (mStack.Count == 0)
				GameCore.SendCommand (CommandGroup.GROUP_LOBBY, LobbyInst.HIDE_LOBBY_DIALOG);
			else
				UpdateDialog ();
		}
	}

	void UpdateDialog()
	{
		if (mStack == null)
			return;

		if (mStack.Count == 0)
			return;
		
		DialogData top = mStack.Peek ();

		mTitle.text = (string)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY,
			RepositoryParam.GET_LOCALIZATION,
			top.Title.Group, top.Title.ID);
		
		mContext.text = (string)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY,
			RepositoryParam.GET_LOCALIZATION,
			top.Context.Group, top.Context.ID);
	}
}
