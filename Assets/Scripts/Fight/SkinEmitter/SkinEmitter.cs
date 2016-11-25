using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkinEmitter : MonoBehaviour 
{
	[SerializeField]
	SkinnedMeshRenderer[] mRenders = null;

	[SerializeField]
	ParticleSystem mParticleSys = null;

	ParticleSystem.Particle[] mParticleList = null;

	List<Vector3> mSkinParticle = new List<Vector3> ();

	// Use this for initialization
	void Start () 
	{
		if (mParticleSys != null) 
		{
			mParticleList = new ParticleSystem.Particle[mParticleSys.maxParticles];
		}
	}

	public void OnEmit(float _startSpeed, float _startLife)
	{
		if (mRenders == null || mParticleList == null)
			return;

		mParticleSys.startSpeed = _startSpeed;
		mParticleSys.startLifetime = _startLife;

		for(int Indx = 0; Indx < mRenders.Length; ++Indx)
		{
			SetParticle(mRenders[Indx]);
		}
	}

	void SetParticle(SkinnedMeshRenderer _skin)
	{
		if (_skin == null)
			return;

		Mesh bake = new Mesh ();

		_skin.BakeMesh (bake);

		Vector3[] meshVertices = bake.vertices;

		for(int Indx = 0; Indx < meshVertices.Length; ++Indx)
		{
			mSkinParticle.Add(_skin.transform.position + (_skin.transform.rotation * meshVertices[Indx]));

			Indx += Random.Range(500,1000);
		}


		int lastCount = mParticleSys.particleCount;

		mParticleSys.Emit (mSkinParticle.Count);

		mParticleSys.GetParticles (mParticleList);

		for(int Indx = 0; Indx < mSkinParticle.Count; ++Indx)
		{
			mParticleList[lastCount + Indx].position = mSkinParticle[Indx];
		}

		mParticleSys.SetParticles (mParticleList, mParticleSys.particleCount);

		mSkinParticle.Clear ();

		GameObject.Destroy (bake);
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyUp (KeyCode.Space) == true) 
		{
			OnEmit(0.5F, 1f);
		}	
	}
}
