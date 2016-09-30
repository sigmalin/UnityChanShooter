using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

// rewrite from BM_WaveEffect & BeamWave
public class WaveBeamMesh : MonoBehaviour 
{
	Mesh mInstantMesh;

	Material mInstantMaterial;

	#region BM_WaveEffect
	[System.Serializable]
	public class WaveMesh
	{
		/*[HideInInspector]*/ public float InSize = 0.0f;
		public float OutSize = 1.0f;
		public float Height = 0.0f;
	}
	[SerializeField] WaveMesh mWaveMesh;
	#endregion

	#region BeamWave
	[System.Serializable]
	public class OperateMesh
	{
		public Color mColorWave = new Color(0.5f,0.5f,0.5f,0.5f);
		public float RotateSpd = 3.0f;
		public float AnmSpd = 2.0f;
		public float AnmTime = 0;
		public float MaxScale = 1.0f;
	}
	[SerializeField] OperateMesh mOperateMesh;

	float mDefOutSize;
	#endregion

	// Use this for initialization
	void Start () 
	{
		mInstantMesh = GetComponent<MeshFilter> ().mesh;

		mInstantMaterial = GetComponent<Renderer> ().material;

		mDefOutSize = mWaveMesh.OutSize;

		#region On Enabled
		this.OnEnableAsObservable ()
			.Subscribe (
				_ =>
				{
					mWaveMesh.InSize = 0;
					mWaveMesh.OutSize = mDefOutSize;

					mOperateMesh.AnmTime = 0f;
					//this.transform.Rotate(Vector3.up, Random.Range(0,360.0f));
					mInstantMaterial.SetColor("_Color", Color.black);
				}
		);
		#endregion

		#region FixedUpdate
		this.FixedUpdateAsObservable ()
			.Subscribe ( 
				_ =>
				{
					Vector3[] vertices = mInstantMesh.vertices;
					for (int i = 0; i < vertices.Length; i+=2) 
					{
						float r;
						r = ((float)(i)/(float)vertices.Length)*4*Mathf.PI;
						vertices[i].x = Mathf.Cos(r)*(Mathf.Lerp(0, mWaveMesh.OutSize, mWaveMesh.InSize));
						vertices[i].y = 0;
						vertices[i].z = Mathf.Sin(r)*(Mathf.Lerp(0, mWaveMesh.OutSize, mWaveMesh.InSize));
						vertices[i+1].x = Mathf.Cos(r)*(mWaveMesh.OutSize);
						vertices[i+1].y = mWaveMesh.Height;
						vertices[i+1].z = Mathf.Sin(r)*(mWaveMesh.OutSize);
					}
					mInstantMesh.vertices = vertices;
				}
			);
	
		this.FixedUpdateAsObservable ()
			.Subscribe ( 
				_ =>
				{
					mOperateMesh.AnmTime += Time.fixedDeltaTime * mOperateMesh.AnmSpd;
					this.transform.Rotate(Vector3.up, mOperateMesh.RotateSpd);

					mWaveMesh.InSize = Mathf.Lerp(0, 1, 1-Mathf.Pow(1-mOperateMesh.AnmTime,8));
					mWaveMesh.OutSize = Mathf.Lerp(0, mOperateMesh.MaxScale+ mDefOutSize, 1-Mathf.Pow(1-mOperateMesh.AnmTime, 9));
					mInstantMaterial.SetColor("_Color", mOperateMesh.mColorWave);
				}
			);
		#endregion
	}

	void OnDestroy()
	{
		ClearMesh ();
		ClearMaterial ();
	}

	void ClearMesh()
	{
		if (mInstantMesh != null) 
		{
			Destroy (mInstantMesh);
			mInstantMesh = null;
		}
	}

	void ClearMaterial()
	{
		if (mInstantMaterial != null) 
		{
			Destroy (mInstantMaterial);
			mInstantMaterial = null;
		}
	}
}
