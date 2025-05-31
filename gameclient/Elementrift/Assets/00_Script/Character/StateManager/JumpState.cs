using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IPlayerState
{
    private bool _jumped = false;

    public void EnterState(PlayerController player)
    {
        player.GetAnimator().SetTrigger(CONSTANT.PLAYER_PARAMETER_IsJumping);
        player.PerformJump();
        _jumped = true;
    }

    public void ExitState(PlayerController player)
    {
        player.GetAnimator().ResetTrigger(CONSTANT.PLAYER_PARAMETER_IsJumping);
    }

    public void UpdateState(PlayerController player)
    {
        if (_jumped && player.IsGrounded())
        {
            _jumped = false;
            if (player.GetInputDirection().magnitude > 0.1f)
                player.changeState(new WalkState());
            else
                player.changeState(new IdleState());
        }
    }
}