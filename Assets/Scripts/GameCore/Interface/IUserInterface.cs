using UnityEngine;
using System.Collections;

public class UiInst
{
	public const uint SET_PROGRESS_TEXT = 0;
	public const uint SET_PROGRESS_PERCENT = 1;
}

public interface IUserInterface
{
	IInput Operator { get; }

	void Show(Transform _root);

	void Hide();

	void SendCommand(uint _inst, params System.Object[] _params);
}
