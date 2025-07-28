using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LyraNormalAttack : SkillBase
{
    public override void activeSkill(float Basedamge)
    {
        EffectorBase effectorPool = ObjectPooling.Instant.GetComp<EffectorBase>(_Effector);
        effectorPool.Init(_Caster, _Damage + Basedamge, this._SkillManager.transform.position, this._SkillManager.transform.forward.normalized);
        effectorPool.gameObject.SetActive(true);
    }
}
