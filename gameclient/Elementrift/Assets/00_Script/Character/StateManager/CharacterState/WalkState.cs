using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        player.GetAnimator().SetBool(CONSTANT.PLAYER_PARAMETER_IsWalking, true);
    }

    public void ExitState(PlayerController player)
    {
        player.GetAnimator().SetBool(CONSTANT.PLAYER_PARAMETER_IsWalking, false);
    }

    public void UpdateState(PlayerController player)
    {
        if (player.GetInputDirection().magnitude < 0.1f)
        {
            player.changeState(new IdleState());
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            player.changeState(new RunState());
        }
    }
}
