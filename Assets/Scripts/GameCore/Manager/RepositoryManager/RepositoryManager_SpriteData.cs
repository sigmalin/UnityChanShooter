using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public partial class RepositoryManager
{
	public const string SPRITE_DIGITS = "Digits";
	public const string SPRITE_HITS = "Hits";

	Dictionary<string, Dictionary<char, UnityEngine.Sprite>> mSpriteTable = null;

	void InitialSpriteData()
	{
		ReleaseSpriteData ();

		mSpriteTable = new Dictionary<string, Dictionary<char, UnityEngine.Sprite>> ();
	}

	void ReleaseSpriteData()
	{
		if (mSpriteTable != null) 
		{
			mSpriteTable.Keys.ToObservable ()
				.Select (_ => mSpriteTable [_])
				.Where (_ => _ != null)
				.Subscribe (_ => _.Clear ());

			mSpriteTable.Clear ();

			mSpriteTable = null;
		}
	}

	void AddSpriteData(string _type, char _key, UnityEngine.Sprite _sprite)
	{
		if (mSpriteTable == null)
			return;

		if (mSpriteTable.ContainsKey (_type) == false)
			mSpriteTable.Add (_type, new Dictionary<char, Sprite> ());

		Dictionary<char, Sprite> spriteDB = mSpriteTable [_type];

		if (spriteDB.ContainsKey (_key) == false)
			spriteDB.Add (_key, _sprite);
		else
			spriteDB[_key] = _sprite;
	}

	void LoadSpriteData(string _type, SpriteRepository _spriteData)
	{
		if (_spriteData == null)
			return;

		_spriteData.SpriteDB.ToObservable ()
			.Subscribe (_ => AddSpriteData (_type, _.Key, _.UiSprite));
	}

	void ReleaseSpriteData(string _type)
	{
		if (mSpriteTable == null)
			return;

		if (mSpriteTable.ContainsKey(_type) == false)
			return;

		Dictionary<char, Sprite> spriteDB = mSpriteTable [_type];
		spriteDB.Clear ();

		mSpriteTable.Remove (_type);
	}

	System.Object GetSpriteData(string _type, char _key)
	{
		System.Object res = (System.Object)(default(UnityEngine.Sprite));

		if (mSpriteTable == null)
			return res;

		if (mSpriteTable.ContainsKey(_type) == false)
			return res;

		Dictionary<char, Sprite> spriteDB = mSpriteTable [_type];

		if (spriteDB.ContainsKey(_key) == false)
			return res;

		res = spriteDB[_key];

		return res;
	}
}