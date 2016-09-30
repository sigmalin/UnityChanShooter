using UnityEngine;
using System.Collections;

// rewrite from Create_MyLineMesh
public class BeamMesh : MonoBehaviour 
{
	private Mesh mMesh;

	[SerializeField]
	int mSplitMesh = 1;

	// Use this for initialization
	void Awake () 
	{
		mMesh = new Mesh(); 
		Vector3[] vertices = new Vector3[mSplitMesh<<2];
		Vector2[] uv= new Vector2[mSplitMesh<<2];
		int[] tri = new int[6*mSplitMesh];

		for(int Indx = 0; Indx < (mSplitMesh << 2); Indx += 2)
		{
			vertices[Indx] = new Vector3(-0.5f, 0, Indx>>1);
			vertices[Indx+1] = new Vector3(0.5f, 0, Indx>>1);
			uv[Indx] = new Vector2((float)(Indx>>1) / (float)mSplitMesh,0);
			uv[Indx+1] = new Vector2((float)(Indx>>1) / (float)mSplitMesh,1);
		}
	
		for(int i = 0, j = 0; i < 6 * mSplitMesh; i += 6, j += 2)
		{
			tri[i] = 0+j;	tri[i+1] = 2+j; tri[i+2] = 1+j;
			tri[i+3] = 2+j;	tri[i+4] = 3+j; tri[i+5] = 1+j;
		}

		mMesh.vertices  = vertices;
		mMesh.uv        = uv;
		mMesh.triangles = tri;

		GetComponent<MeshFilter>().sharedMesh = mMesh;
		GetComponent<MeshFilter>().sharedMesh.name = "BeamMesh";
	}

	void OnDestroy()
	{
		if (mMesh != null) 
		{
			Destroy (mMesh);
			mMesh = null;
		}
	}
}
