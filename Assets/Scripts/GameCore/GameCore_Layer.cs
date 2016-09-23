using UnityEngine;
using System.Collections;

public partial class GameCore  
{
	int mLayerDefault = 0;
	int mLayerPlayer = 0;
	int mLayerEnemy = 0;
	int mLayerInvisible = 0;

	static public int LAYER_DEFAULT   { get { return mInstance != null ? mInstance.mLayerDefault   : 0; } }
	static public int LAYER_PLAYER    { get { return mInstance != null ? mInstance.mLayerPlayer    : 0; } }
	static public int LAYER_ENEMY     { get { return mInstance != null ? mInstance.mLayerEnemy     : 0; } }
	static public int LAYER_INVISIBLE { get { return mInstance != null ? mInstance.mLayerInvisible : 0; } }

	// Use this for initialization
	void InitialLayer () 
	{
		mLayerDefault = LayerMask.NameToLayer("Default");

		mLayerPlayer = LayerMask.NameToLayer("Player");

		mLayerEnemy = LayerMask.NameToLayer("Enemy");

		mLayerInvisible = LayerMask.NameToLayer("Invisible");
	}

	static public int GetRaycastLayer(int _layer)
	{
		return 1 << _layer;
	}
}
