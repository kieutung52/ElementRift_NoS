using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterState
{
    public void EnterState(MonsterController monster);
    public void ExitState(MonsterController monster);
    public void UpdateState(MonsterController monster);
}
