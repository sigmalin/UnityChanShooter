using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public class SpriteList : MonoBehaviour 
{
	[SerializeField]
	GameObject mItemTemplate;

	[SerializeField]
	RectTransform mListRoot;

	ResourcePool<GameObject> mItemPool;

	UnityEngine.UI.Image[] mUsedItems;

	public delegate UnityEngine.Sprite GetSpriteCallBack(char _key);

	void OnDestroy()
	{
		ReleaseItemPool ();
	}

	public void SetSpriteList(string _text, Color _color, GetSpriteCallBack _callback)
	{
		if (mItemPool == null)
			InitialItemPool();
		
		if (string.IsNullOrEmpty (_text) == true || _callback == null)
			RecycleItems ();
		else 
		{
			char[] chars = _text.ToCharArray ();

			if (mUsedItems == null || chars.Length != mUsedItems.Length) 
			{
				RecycleItems ();

				mUsedItems = chars.Select (_ => _callback (_))
					.Where (_ => _ != null)
					.Select (_ => {
						GameObject itemGO = mItemPool.Produce ();

						itemGO.transform.SetParent (mListRoot);
						itemGO.transform.localRotation = Quaternion.identity;
						itemGO.transform.localScale = Vector3.one;
						itemGO.transform.localPosition = Vector3.zero;

						UnityEngine.UI.Image image = itemGO.GetComponent<UnityEngine.UI.Image> ();
						if (image != null)
						{
							image.sprite = _;
							image.color = _color;
						}

						return image;
					}).ToArray ();
			} 
			else 
			{
				chars.ToObservable ()
					.Select ((_value, _index) => new { Item = mUsedItems[_index], Sprite = _callback (_value) })
					.Subscribe (_ => 
						{
							_.Item.sprite = _.Sprite;
							_.Item.color = _color;
						});
			}
		}
	}

	void ReleaseItemPool()
	{
		RecycleItems ();

		if (mItemPool != null) 
		{
			mItemPool.ReleaseAllResource ();

			mItemPool = null;
		}
	}

	void RecycleItems()
	{
		if (mUsedItems == null)
			return;

		mUsedItems.ToObservable ()
			.Subscribe (_ => {
				if (mItemPool != null) 
				{
					_.sprite = null;

					_.transform.SetParent(this.transform);
					mItemPool.Recycle (_.gameObject);
				}
				else
					GameObject.Destroy (_.gameObject);
			});

		mUsedItems = null;
	}

	void InitialItemPool()
	{
		ReleaseItemPool ();

		mItemPool = new ResourcePool<GameObject> (
			() => { 
				GameObject res = GameObject.Instantiate<GameObject> (mItemTemplate);
				res.SetActive(true);
				return res;
			},
			_ => { 
				if (_ != null) 
					GameObject.Destroy (_); 
			},
			(_res, active) => _res.SetActive (active)
		);
	}
}
