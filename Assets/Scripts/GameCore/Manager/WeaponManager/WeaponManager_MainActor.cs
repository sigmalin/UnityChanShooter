using UnityEngine;
using System.Collections;
using UniRx;

public sealed partial class WeaponManager
{
	uint mMainActorID = 0;

	void SetMainActor(uint _id)
	{
		WeaponActor actor = GetWeaponActor (_id);
		if (actor == null)
			return;

		mMainActorID = _id;

		SetWeaponInterface (actor);

		SetMainCamera (actor);

		uint[] actorIDs = GetAllActorID ();

		actorIDs.ToObservable ()
			.Select(_ => GetWeaponActor(_))
			.Where(_ => _ != null)
			.Subscribe (_ => SetActorLayer (_));
	}

	void SetActorLayer(WeaponActor _actor)
	{
		int layer = GameCore.LAYER_DEFAULT;

		WeaponActor mainActor = GetWeaponActor (mMainActorID);

		if (mainActor != null)
			layer = mainActor.Team == _actor.Team ? GameCore.LAYER_PLAYER : GameCore.LAYER_ENEMY;

		GameCore.SendCommand (CommandGroup.GROUP_PLAYER, PlayerInst.SET_LAYER, _actor.ActorID, layer);
	}
}
