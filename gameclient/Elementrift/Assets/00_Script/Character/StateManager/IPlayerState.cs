using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    public void EnterState(PlayerController player);
    public void ExitState(PlayerController player);
    public void UpdateState(PlayerController player);
}
