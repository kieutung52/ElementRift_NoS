using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGetHit
{
    public void TakeDamage(Character caster, float dmg);
    public void Heal(float healAmount);
}
