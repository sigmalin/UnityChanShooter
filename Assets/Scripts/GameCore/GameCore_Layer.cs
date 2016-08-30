using UnityEngine;
using System.Collections;

public partial class GameCore  
{
	int mLayerDefault = 0;
	int mLayerEnemy = 0;
	int mLayerInvisible = 0;

	static public int LAYER_BULLET { get { return mInstance != null ? mInstance.mLayerDefault | mInstance.mLayerEnemy : 0; } }
	static public int LAYER_CAMERA { get { return mInstance != null ? mInstance.mLayerDefault: 0; } }

	// Use this for initialization
	void InitialLayer () 
	{
		mLayerDefault = 1 << LayerMask.NameToLayer("Default");

		mLayerEnemy = 1 << LayerMask.NameToLayer("Enemy");

		mLayerInvisible = 1 << LayerMask.NameToLayer("Invisible");
	}
}
