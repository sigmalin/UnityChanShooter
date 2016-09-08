using UnityEngine;
using System.Collections;

public interface IUserInterface
{
	IInput Operator { get; }

	void Show(Transform _root);

	void Hide();

	void SendCommand(uint _inst, params System.Object[] _params);
}
