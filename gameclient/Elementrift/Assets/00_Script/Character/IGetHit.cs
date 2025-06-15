using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGetHit
{
    public void TakeDamage(PlayerData caster, float dmg);
    public void Heal(float healAmount);
    // public void Attack();
}
