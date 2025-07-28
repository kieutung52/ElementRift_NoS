using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using Unity.VisualScripting;
using UnityEngine;

public class MagicShieldSkill : SkillBase
{
    public override void activeSkill(float Basedamge)
    {
        EffectorBase effectorPool = ObjectPooling.Instant.GetComp<EffectorBase>(_Effector);
        effectorPool.Init(_Caster, _Damage, this._SkillManager.transform.position, Vector3.zero);
        effectorPool.transform.parent = this._SkillManager.transform;
        effectorPool.gameObject.SetActive(true);
    }
}
