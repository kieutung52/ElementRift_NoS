using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MWalkState : IMonsterState
{
    public void EnterState(MonsterController monster)
    {
        monster.GetAnimator().SetBool("IsWalk", true);
    }

    public void ExitState(MonsterController monster)
    {
        monster.GetAnimator().SetBool("IsWalk", false);
    }

    public void UpdateState(MonsterController monster)
    {
    }
}
