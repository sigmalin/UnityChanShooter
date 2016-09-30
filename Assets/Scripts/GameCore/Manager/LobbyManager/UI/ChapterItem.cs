using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class ChapterItem : MonoBehaviour, IItem 
{
	[SerializeField]
	UnityEngine.UI.RawImage mImage;

	[SerializeField]
	UnityEngine.UI.Text mText;

	[SerializeField]
	UnityEngine.UI.Button mButton;

	int mChapterID;

	// Use this for initialization
	void Start () 
	{
		mButton.OnClickAsObservable ()
			.Subscribe (_ => {
				GameCore.SendCommand(CommandGroup.GROUP_LOBBY, LobbyInst.SELECT_CHAPTER, mChapterID);
			});
	}

	public void Initial (params System.Object[] _params)
	{
		mChapterID = (int)_params[0];

		mText.text = (string)GameCore.GetParameter (ParamGroup.GROUP_REPOSITORY, RepositoryParam.GET_LOCALIZATION, LocalizationDefine.LOCALIZATION_GROUP_CHAPTER, mChapterID);

		SetItemImage ();

		if (mButton != null)
			mButton.interactable = true;
	}

	public void Release ()
	{
	}

	public void SetReactiveProperty<T>(ReadOnlyReactiveProperty<T> _reactiveProperty)
	{
	}

	void SetItemImage()
	{
		mImage.texture = (Texture)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_CHAPTER_IMAGE, mChapterID, false);

		if (mImage.texture == null) 
		{
			string[] loadList = new string[] {
				(string)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_CHAPTER_IMAGE_PATH)
			};

			GameCore.AdditionalLoad (
				loadList,
				() => mImage.texture = (Texture)GameCore.GetParameter (ParamGroup.GROUP_CACHE, CacheParam.GET_CHAPTER_IMAGE, mChapterID, false),
				true
			);
		}
	}
}

