using UnityEngine;
using System.Collections;

public interface IUserInterface
{
	IInput Operator { get; }

	void Show(Transform _root);

	void Hide();

	void Localization();

	void Clear();

	void Operation(uint _inst, params System.Object[] _params);
}
