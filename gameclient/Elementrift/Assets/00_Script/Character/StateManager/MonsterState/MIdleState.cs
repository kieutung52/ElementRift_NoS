using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIdleState : IMonsterState
{
    public void EnterState(MonsterController monster)
    {
        monster.GetAnimator().SetBool("IsIdle", true);
    }

    public void ExitState(MonsterController monster)
    {
        monster.GetAnimator().SetBool("IsIdle", false);
    }

    public void UpdateState(MonsterController monster)
    {
    }
}
