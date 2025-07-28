using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        player.GetAnimator().SetBool(CONSTANT.PLAYER_PARAMETER_IsIdle, true);
    }

    public void ExitState(PlayerController player)
    {
        player.GetAnimator().SetBool(CONSTANT.PLAYER_PARAMETER_IsIdle, false);
    }

    public void UpdateState(PlayerController player)
    {
        if (player.GetInputDirection().magnitude > 0.1f)
        {
            player.changeState(new WalkState());
        }
    }
}
