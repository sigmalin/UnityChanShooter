using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class Progress : MonoBehaviour 
{
	[SerializeField]
	UnityEngine.UI.Text mTextTab;

	[SerializeField]
	UnityEngine.UI.Image mImgPercent;

	float mRatePercent = 0F;

	public float Percent 
	{ 
		set 
		{
			if (mImgPercent != null) 
			{
				mImgPercent.fillAmount = value;
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
}
