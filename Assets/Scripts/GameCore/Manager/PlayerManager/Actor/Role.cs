using UnityEngine;
using System.Collections;
using UniRx;

[RequireComponent(typeof(Animator))]
public class Role : ModelBehaviour 
{
	Subject<Unit> mOnAnimatorMoveSubject = new Subject<Unit>();
	public IObservable<Unit> OnAnimatorMoveAsObservable { get { return mOnAnimatorMoveSubject.AsObservable (); } }

	Subject<Unit> mOnAnimatorIKSubject = new Subject<Unit>();
	public IObservable<Unit> OnAnimatorIKAsObservable { get { return mOnAnimatorIKSubject.AsObservable (); } }

	Subject<string> mOnAnimatorEventSubject = new Subject<string>();
	public IObservable<string> OnAnimatorEventAsObservable { get { return mOnAnimatorEventSubject.AsObservable (); } }

	void OnAnimatorMove()
	{
		mOnAnimatorMoveSubject.OnNext (Unit.Default);
	}

	void OnAnimatorIK()
	{
		mOnAnimatorIKSubject.OnNext (Unit.Default);
	}

	void AnimEvent(string _event)
	{
		mOnAnimatorEventSubject.OnNext (_event);
	}
}
