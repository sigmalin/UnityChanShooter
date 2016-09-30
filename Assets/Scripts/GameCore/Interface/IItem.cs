using UnityEngine;
using System.Collections;
using UniRx;

public interface IItem
{
	void Initial (params System.Object[] _params);

	void Release ();

	void SetReactiveProperty<T> (ReadOnlyReactiveProperty<T> _reactiveProperty);
}
