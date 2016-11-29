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
		//this.OnDisableAsObservable ()
		//	.SelectMany (_ => mRigidbodies.ToObservable ())
		//	.Where (_ => _ != null)
		//	.Subscribe (_ => _.isKinematic = true);

		this.OnEnableAsObservable()
			.SelectMany (_ => mRigidbodies.ToObservable ())
			.Where (_ => _ != null)
			.Subscribe (_ => 
				{
					_.isKinematic = true;
					_.interpolation = RigidbodyInterpolation.None;
				});
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

		mRigidbodies.ToObservable ()
			.Where (_ => _ != null)
			.Subscribe (_ => 
				{
					_.isKinematic = false;
					_.interpolation = RigidbodyInterpolation.Interpolate;
				});
	}

	public void AddImpact(float _impact, Vector3 _hitPt)
	{
		mRigidbodies.ToObservable ()
			.Where (_ => _ != null)
			.First ()
			//.Subscribe (_ => _.AddExplosionForce(20f, _hitPt, 1f, 20f));
			.Subscribe (_ => _.AddForce(new Vector3(_.position.x - _hitPt.x, 5f, _.position.z - _hitPt.z) * Mathf.Clamp(_impact, 0f, 30f), ForceMode.Impulse));
	}
}
