﻿using UnityEngine;
using System.Collections;
using UniRx;

public partial class Status
{
	[SerializeField]
	RectTransform mListRoot;

	[SerializeField]
	GameObject mItemTemplate;

	ResourcePool<GameObject> mItemPool;

	GameObject[] mUsedItems;

	void UpdateList(WeaponManager.WeaponActor _actor)
	{
		RecycleItems ();

		mUsedItems = new GameObject[] {
			// fire
			GetItem((string)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY, RepositoryParam.GET_LOCALIZATION, LocalizationDefine.LOCALIZATION_GROUP_WEAPON, (int)_actor.RefWeaponData.WeaponID), _actor.Charge),
		};
	}

	GameObject GetItem(string _title, ReadOnlyReactiveProperty<float> _reactiveProperty)
	{
		if (mItemPool == null)
			InitialItemPool();

		GameObject itemGO = mItemPool.Produce ();

		itemGO.transform.SetParent (mListRoot);
		itemGO.transform.localRotation = Quaternion.identity;

		IItem item = itemGO.GetComponent<IItem> ();
		if (item != null) 
		{
			item.Initial (_title);
			item.SetReactiveProperty (_reactiveProperty);
		}

		return itemGO;
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
					IItem item = _.GetComponent<IItem> ();
					if (item != null) item.Release();

					_.transform.SetParent(this.transform);
					mItemPool.Recycle (_);
				}
				else
					GameObject.Destroy (_);
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