using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Ragdoll : ModelBehaviour 
{
	[SerializeField]
	Rigidbody[] mRigidbodies;
	public Rigidbody[] Rigidbodies
	{ 
		get { return mRigidbodies; } 
		#if UNITY_EDITOR
		set { mRigidbodies = value; } 
		#endif
	}

	void Start()
	{
		this.OnDisableAsObservable ()
			.SelectMany (_ => mRigidbodies.ToObservable ())
			.Where (_ => _ != null)
			.Subscribe (_ => _.isKinematic = true);

		this.OnEnableAsObservable()
			.SelectMany (_ => mRigidbodies.ToObservable ())
			.Where (_ => _ != null)
			.Subscribe (_ => _.isKinematic = false);
	}

	public void CopyPose(ModelBehaviour _model)
	{
		if (Bones.Length != _model.Bones.Length)
			return;

		_model.Bones.ToObservable ()
			.Where (_ => _ != null)
			.Select ((_, _indx) => new {Bone = _, Index = _indx})
			.Subscribe (_ => {
				if (Bones [_.Index] != null)
				{
					Bones [_.Index].position = _.Bone.position;
					Bones [_.Index].rotation = _.Bone.rotation;
				}
			});
	}
}
