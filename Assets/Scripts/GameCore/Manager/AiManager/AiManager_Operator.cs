using System.Collections.Generic;
using System.Linq;
using UniRx;

public sealed partial class AiManager
{
	void FreezeAi(uint _playerID, bool _isEnable)
	{
		IAi ai = GetAiData(_playerID);
		if (ai == null)
			return;

		ai.Freeze (_isEnable);
	}
}
