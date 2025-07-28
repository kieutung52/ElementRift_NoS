using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MDeadState : IMonsterState
{
    public void EnterState(MonsterController monster)
    {
        monster.GetAnimator().SetBool("IsDie", true);
    }

    public void ExitState(MonsterController monster)
    {
        monster.GetAnimator().SetBool("IsDie", false);
    }

    public void UpdateState(MonsterController monster)
    {
    }
}
