using UnityEngine;
using System.Collections;

public interface IMotion
{
	void EnterMotion(PlayerManager.PlayerData _playerData);

	void LeaveMotion(PlayerManager.PlayerData _playerData);

	void UpdateMotion(PlayerManager.PlayerData _playerData);

	void AnimMoveMotion(PlayerManager.PlayerData _playerData);

	void AnimIKMotion(PlayerManager.PlayerData _playerData);
}
