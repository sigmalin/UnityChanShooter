using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public partial class RepositoryManager 
{
	Dictionary<uint, Material> mPostEffectTable = null;

	void InitialPostEffectData()
	{
		ReleasePostEffectData ();

		mPostEffectTable = new Dictionary<uint, Material> ();
	}

	void ReleasePostEffectData()
	{
		if (mPostEffectTable != null) 
		{
			mPostEffectTable.Clear ();

			mPostEffectTable = null;
		}
	}

	void AddPostEffectData(uint _key, Material _postEffect)
	{
		if (mPostEffectTable == null)
			return;

		if (mPostEffectTable.ContainsKey (_key) == true)
			mPostEffectTable.Remove (_key);

		mPostEffectTable.Add (_key, _postEffect);
	}

	void LoadPostEffectData(PostEffectRepository _postEffectData)
	{
		if (_postEffectData == null)
			return;

		_postEffectData.PostEffectDB.ToObservable ()
			.Subscribe (_ => AddPostEffectData (_.Key, _.PostEffect));
	}

	System.Object GetPostEffectData(uint _key)
	{
		System.Object res = (System.Object)(default(Material));

		if (mPostEffectTable == null)
			return res;

		if (mPostEffectTable.ContainsKey(_key) == false)
			return res;

		res = mPostEffectTable[_key];

		return res;
	}
}
