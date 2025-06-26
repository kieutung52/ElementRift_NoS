using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralBarageSkill : SkillBase
{
    public override void activeSkill()
    {
        EffectorBase effectorPool = ObjectPooling.Instant.GetComp<EffectorBase>(_Effector);
        effectorPool.Init(_Caster, _Damage, this._SkillManager.transform.position, this._SkillManager.transform.forward.normalized);
        effectorPool.gameObject.SetActive(true);
    }
}
