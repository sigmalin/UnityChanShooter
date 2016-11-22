using UnityEngine;
using System.Collections;
using UniRx;

public interface IItem
{
	void Initial (params System.Object[] _params);

	void Release ();

	void SetReactiveProperty (ReadOnlyReactiveProperty<float> _reactiveProperty);

	void SetReactiveProperty (ReadOnlyReactiveProperty<bool> _reactiveProperty);
}
