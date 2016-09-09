﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public sealed partial class Page_CharacterList
{
	[SerializeField]
	Transform mView;

	[SerializeField]
	GameObject mItemTemplate;

	ResourcePool<GameObject> mItemPool;

	GameObject[] mUsedItems;

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

	void ReleaseItemPool()
	{
		RecycleItems ();

		if (mItemPool != null) 
		{
			mItemPool.ReleaseAllResource ();

			mItemPool = null;
		}
	}

	void UpdateView()
	{
		RecycleItems ();

		mUsedItems = GameCore.UserProfile.HoldCharacterList
			.Select (_ => GetItem (_)).ToArray ();
	}

	GameObject GetItem(uint _id)
	{
		if (mItemPool == null)
			InitialItemPool();

		GameObject itemGO = mItemPool.Produce ();

		itemGO.transform.SetParent (mView);

		IItem item = itemGO.GetComponent<IItem> ();
		if (item != null)
			item.Initial (_id);
		
		return itemGO;
	}

	void RecycleItems()
	{
		if (mUsedItems == null)
			return;

		mUsedItems.ToObservable ()
			.Subscribe (_ => {
				if (mItemPool != null) 
				{
					_.transform.SetParent(this.transform);
					mItemPool.Recycle (_);
				}
				else
					GameObject.Destroy (_);
			});

		mUsedItems = null;
	}
}