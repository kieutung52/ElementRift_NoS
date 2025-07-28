using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAttackState : IMonsterState
{
    public void EnterState(MonsterController monster)
    {
        monster.GetAnimator().SetBool("IsAttack", true);
    }

    public void ExitState(MonsterController monster)
    {
        monster.GetAnimator().SetBool("IsAttack", false);
    }

    public void UpdateState(MonsterController monster)
    {
        // Logic for updating the attack state, e.g., check if player is in range to attack
        Debug.Log($"{monster.name} is attacking.");
    }
}