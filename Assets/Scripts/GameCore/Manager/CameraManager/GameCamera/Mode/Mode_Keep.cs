using UnityEngine;
using System.Collections;

public sealed class Mode_Keep : IMode 
{
	uint mTargetID = 0u;

	Vector3 mRelativelyDistance = Vector3.zero;

	public Mode_Keep (uint _targetID) 
	{
		mTargetID = _targetID;	
	}
	
	public void EnterMode(GameCamera _camera)
	{
		if (_camera == null)
			return;

		Transform target = GetTargetTrans ();
		if (target != null)
			mRelativelyDistance = _camera.transform.position - target.position;
	}

	public void LeaveMode(GameCamera _camera)
	{
	}

	public void UpdateMode(GameCamera _camera)
	{
		Transform target = GetTargetTrans ();
		if (target != null)
			_camera.transform.position =  mRelativelyDistance + target.position;

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.CAMERA_FOCUS, mTargetID, _camera.transform.position + (_camera.transform.forward * 8f));
	}

	public void ExecCommand(uint _inst, params System.Object[] _params)
	{
	}

	Transform GetTargetTrans()
	{
		return (Transform)GameCore.GetParameter (ParamGroup.GROUP_PLAYER, PlayerParam.PLAYER_TRANSFORM, mTargetID);
	}
}
