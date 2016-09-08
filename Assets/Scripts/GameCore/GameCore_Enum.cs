using UnityEngine;
using System.Collections;

public partial class GameCore 
{
	int mAnimID_isMove = 0;
	static public int AnimID_isMoveID { get { return mInstance != null ? mInstance.mAnimID_isMove : 0; } }

	int mAnimID_isGround = 0;
	static public int AnimID_isGroundID { get { return mInstance != null ? mInstance.mAnimID_isGround : 0; } }

	int mAnimID_fSpeed = 0;
	static public int AnimID_fSpeed { get { return mInstance != null ? mInstance.mAnimID_fSpeed : 0; } }

	int mAnimID_fJumpForce = 0;
	static public int AnimID_fJumpForce { get { return mInstance != null ? mInstance.mAnimID_fJumpForce : 0; } }

	int mAnimID_iLobbyState = 0;
	static public int AnimID_iLobbyState { get { return mInstance != null ? mInstance.mAnimID_iLobbyState : 0; } }

	// Use this for initialization
	void InitialAnimID () 
	{
		mAnimID_isMove = Animator.StringToHash ("isMove");
	
		mAnimID_isGround = Animator.StringToHash ("isGround");

		mAnimID_fSpeed = Animator.StringToHash ("fSpeed");

		mAnimID_fJumpForce = Animator.StringToHash ("fJumpForce");

		mAnimID_iLobbyState = Animator.StringToHash ("iLobbyState");
	}
}
