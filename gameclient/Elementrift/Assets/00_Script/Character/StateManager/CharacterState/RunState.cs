using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : IPlayerState
{
    private float _originalSpeed;
    private const float RUN_SPEED_INCREASE = 15f;

    public void EnterState(PlayerController player)
    {
        _originalSpeed = player.GetMoveSpeed();
        player.GetAnimator().SetBool(CONSTANT.PLAYER_PARAMETER_IsRunning, true);
        player.SetMoveSpeed(_originalSpeed + RUN_SPEED_INCREASE);
    }

    public void ExitState(PlayerController player)
    {
        player.GetAnimator().SetBool(CONSTANT.PLAYER_PARAMETER_IsRunning, false);
        player.SetMoveSpeed(_originalSpeed);
    }

    public void UpdateState(PlayerController player)
    {
        if (player.GetInputDirection().magnitude < 0.1f)
        {
            player.changeState(new IdleState());
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {
            player.changeState(new WalkState());
        }
    }
}