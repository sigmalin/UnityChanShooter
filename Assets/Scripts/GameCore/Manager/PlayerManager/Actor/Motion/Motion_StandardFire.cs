using UnityEngine;
using System.Collections;

public sealed class Motion_StandardFire : IMotion 
{
	public void EnterMotion(PlayerManager.PlayerData _playerData)
	{
	}

	public void LeaveMotion(PlayerManager.PlayerData _playerData)
	{
	}

	public void UpdateMotion(PlayerManager.PlayerData _playerData)
	{
		float speed = _playerData.RefActor.Actordata.Anim.GetFloat(GameCore.AnimID_fSpeed);

		speed = Mathf.Lerp (speed, _playerData.Speed, Time.deltaTime * 5F);

		_playerData.RefActor.Actordata.Anim.SetFloat(GameCore.AnimID_fSpeed, speed);
	}

	public void AnimMoveMotion(PlayerManager.PlayerData _playerData)
	{
		Movement (_playerData);

		_playerData.RefActor.Actordata.Anim.SetFloat (GameCore.AnimID_fJumpForce, _playerData.RefActor.Actordata.Rigid.velocity.y);
		_playerData.RefActor.Actordata.Anim.SetBool (GameCore.AnimID_isGroundID, _playerData.RefActor.Actordata.Col.IsGround());
	}

	public void AnimIKMotion(PlayerManager.PlayerData _playerData)
	{
		_playerData.RefActor.Actordata.Anim.SetLookAtWeight (1F, 	// weight 
			0.3F,	// body
			0.4F,	// head
			0.2F,	// eye
			0.5F);	// clamp


		bool isRightSide = _playerData.RefActor.transform.IsPointRightSide (_playerData.LookAt);
		AvatarIKGoal goal = isRightSide ? AvatarIKGoal.RightHand : AvatarIKGoal.LeftHand;
		IArm arm = isRightSide ? _playerData.RefActor.Launcher.RightArm : _playerData.RefActor.Launcher.LeftArm;


		_playerData.RefActor.Actordata.Anim.SetIKPositionWeight (goal, 1F);

		_playerData.RefActor.Actordata.Anim.SetLookAtPosition (_playerData.LookAt);

		_playerData.RefActor.Actordata.Anim.SetIKPosition (goal, _playerData.LookAt);

		GameCore.SendCommand (CommandGroup.GROUP_WEAPON, WeaponInst.ARM_FIRE, _playerData.ID, arm);
	}

	void Movement(PlayerManager.PlayerData _playerData)
	{
		_playerData.RefActor.Actordata.Rigid.velocity = new Vector3 (_playerData.Move.x * _playerData.Speed, _playerData.RefActor.Actordata.Rigid.velocity.y, _playerData.Move.z * _playerData.Speed);
	}
}
