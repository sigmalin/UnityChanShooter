using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

// rewrite from BeamParam, BeamLine, GetColor_Particle, BillBoard, flash, GetColor_Flash
public class BeamRay : MonoBehaviour 
{
	#region BeamParam
	[System.Serializable]
	public class BeamParam
	{
		public Color BeamColor = Color.white;
		public float AnimationSpd = 0.1f;
		public float Scale = 1.0f;

		public bool IsEnd = false;
		public bool IsGero = false;
	}

	[SerializeField]
	BeamParam mBeamParam;
	#endregion

	#region BeamLine
	[System.Serializable]
	public class BeamLineParam
	{
		public float StartSize = 1.0f;
		public float AnimationSpd = 0.1f;

		public LineRenderer Line;

		[HideInInspector] public float NowAnm = 0f;
		[HideInInspector] public float NowLength = 0f;
	}

	[SerializeField]
	BeamLineParam mBeamLineParam;
	#endregion

	#region GetColor_Particle
	[SerializeField]
	ParticleSystem mRayParticleSys;
	#endregion

	#region BillBoard & flash & GetColor_Flash
	[System.Serializable]
	public class BeamLineFlash
	{
		public Transform BeamFlash;
		public Light BeamLight;
		public SpriteRenderer Sprite;

		public float MaxSize = 1.0f;
		public float AnimationSpd = 0.1f;

		[HideInInspector] public float NowAnm = 0;
		[HideInInspector] public float IntensityParam = 0;
	}

	[SerializeField]
	BeamLineFlash mBeamLineFlash;
	private float RndRotate = 0f;
	#endregion

	ReactiveProperty<Vector3> BeamLineEnd { get; set; }
	public Vector3 BeamEnd 
	{ 
		set 
		{
			if (BeamLineEnd == null)
				BeamLineEnd = new ReactiveProperty<Vector3> (this.transform.position);
			BeamLineEnd.Value = value; 
		} 
	}

	// Use this for initialization
	void Start () 
	{
		mBeamLineParam.Line.SetColors (mBeamParam.BeamColor, mBeamParam.BeamColor);

		mRayParticleSys.startColor = mBeamParam.BeamColor;
		mRayParticleSys.startSize = mBeamParam.Scale;

		mBeamLineFlash.BeamLight.color = mBeamParam.BeamColor;
		mBeamLineFlash.BeamLight.range = mBeamParam.Scale;
		mBeamLineFlash.Sprite.color = mBeamParam.BeamColor;

		RndRotate = Random.value * 360.0f;

		if (BeamLineEnd == null)
			BeamLineEnd = new ReactiveProperty<Vector3> (this.transform.position);

		BeamLineEnd.Subscribe (_ => 
			{
				mBeamLineParam.Line.SetPosition(0, this.transform.position);
				mBeamLineParam.Line.SetPosition(1, _);

				mBeamLineFlash.BeamFlash.position = _;
				mRayParticleSys.transform.position = _;
			});

		this.OnEnableAsObservable ()
			.Subscribe (_ => 
				{
					mBeamLineParam.NowAnm = 0f;

					mBeamLineFlash.BeamFlash.localScale = Vector3.zero;
					mBeamLineFlash.NowAnm = 0f;
					mBeamLineFlash.IntensityParam = 0f;

					RndRotate = Random.value * 360.0f;
				});

		//this.FixedUpdateAsObservable ()
		this.UpdateAsObservable ()
			.Subscribe (_ =>
				{
					mBeamLineParam.NowAnm += mBeamParam.AnimationSpd;
					float width = Mathf.Lerp(mBeamLineParam.StartSize,0,mBeamLineParam.NowAnm);
					mBeamLineParam.Line.SetWidth(width, width);
				});
		
		this.UpdateAsObservable ()
			.Subscribe (_ =>
				{
					Vector3 vec = new Vector3( 0f, Camera.main.transform.position.y - mBeamLineFlash.BeamFlash.position.y, 0f);
					mBeamLineFlash.BeamFlash.LookAt(Camera.main.transform.position - vec);
					mBeamLineFlash.BeamFlash.Rotate(Vector3.forward, RndRotate);
				});

		this.FixedUpdateAsObservable ()
			.Subscribe (_ =>
				{
					float intensity = Mathf.Lerp(0, mBeamLineFlash.MaxSize,Mathf.Min(mBeamLineFlash.IntensityParam, 1.0f));
					intensity = Mathf.Lerp(intensity, mBeamLineFlash.MaxSize/2, Mathf.Min(mBeamLineFlash.IntensityParam-1.0f,1.0f));
					intensity = Mathf.Lerp(intensity, 0.0f, mBeamLineFlash.NowAnm);
					mBeamLineFlash.IntensityParam += 0.25f;

					Vector3 m_scale = new Vector3(intensity, intensity, intensity);
					mBeamLineFlash.BeamFlash.localScale = m_scale;

					if(mBeamLineFlash.BeamLight != null)
					{
						mBeamLineFlash.BeamLight.intensity = intensity * 0.1f;
					}

					mBeamLineFlash.NowAnm += mBeamLineFlash.AnimationSpd;
				});

	}
}
