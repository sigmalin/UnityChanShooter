using UnityEngine;
using System.Collections;

public class SpriteRepository : ScriptableObject 
{
	[System.Serializable]
	public struct SpritePair
	{
		public char Key;
		public UnityEngine.Sprite UiSprite;
	}

	[SerializeField]
	SpritePair[] mSpriteDB;

	public SpritePair[] SpriteDB { get { return mSpriteDB; } }
}
