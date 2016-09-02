using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class Progress : MonoBehaviour 
{
	[SerializeField]
	UnityEngine.UI.Text mTextTab;

	[SerializeField]
	RectTransform mRectTransPercent;

	float mRatePercent = 0F;

	public float Percent 
	{ 
		set 
		{
			if (mRectTransPercent != null) 
			{
				mRectTransPercent.offsetMax = new Vector2 (
					mRatePercent * (value - 1F),
					mRectTransPercent.offsetMax.y);
			}
		} 
	}

	public string Tab
	{
		set 
		{
			if (mTextTab != null) 
				mTextTab.text = value;
		} 
	}
	
	protected void InitialProgress()
	{
		if (mRectTransPercent != null) 
		{
			mRectTransPercent.anchorMin = new Vector2 (0F,0F);
			mRectTransPercent.anchorMax = new Vector2 (1F,1F);
			mRectTransPercent.pivot = new Vector2 (0.5F, 0.5F);

			mRatePercent = this.GetComponent<RectTransform> ().sizeDelta.x - mRectTransPercent.offsetMin.x;
			Percent = 0.5f;
		}
	}
}
