using UnityEngine;
using System.Collections;

public sealed partial class Page_Character 
{
	[SerializeField]
	RenderTexture mAnalysisGraphic;

	void CreateAnalysisGraphic()
	{
		GameCore.AddGL (() => GraphUtil.DrawAnalysisGraphic (mAnalysisGraphic, Color.red, new float[] { 1, 1, 1, 1, 1 }));
	}
}
