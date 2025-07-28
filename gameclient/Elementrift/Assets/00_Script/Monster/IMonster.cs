using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonster
{
    void Die();
    void AttackPlayer(PlayerController target);
}
