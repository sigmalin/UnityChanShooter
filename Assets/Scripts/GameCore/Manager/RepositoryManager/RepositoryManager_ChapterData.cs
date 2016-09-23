using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public partial class RepositoryManager
{
	ChapterRepository.ChapterData[] mChapterList = null;

	void LoadChapterData(ChapterRepository _chapterData)
	{
		if (_chapterData == null)
			return;

		mChapterList = _chapterData.ChapterDataList;
	}

	System.Object GetChapterData(int _id)
	{
		System.Object res = (System.Object)(default(ChapterRepository.ChapterData));

		if (mChapterList == null)
			return res;

		res = mChapterList.FirstOrDefault (_ => _.ChapterID == _id);

		return res;
	}

	System.Object GetAllChapterData()
	{
		return (System.Object)mChapterList;
	}
}
