using UnityEngine;
using System.Collections;

public class PostEffectRepository : ScriptableObject 
{
	public class PostEffectType
	{
		public const uint BLUE_HUE = 1u;
		public const uint INVERT = 2u;
		public const uint SHOCK = 3u;
		public const uint SHAKE = 4u;
		public const uint BLOOD = 5u;
	}

	[System.Serializable]
	public struct PostEffectPair
	{
		public uint Key;
		public Material PostEffect;
	}

	[SerializeField]
	PostEffectPair[] mPostEffectDB;

	public PostEffectPair[] PostEffectDB { get { return mPostEffectDB; } }
}
