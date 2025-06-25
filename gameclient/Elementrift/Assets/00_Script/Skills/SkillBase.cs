using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [SerializeField] protected float _Damage;
    [SerializeField] private float _ManaCost;
    [SerializeField] private float _CoolDownTime;
    [SerializeField] private float _AttackRange;
    [SerializeField] protected PlayerData _Caster;
    [SerializeField] protected EffectorBase _Effector;

    protected Vector3 _DirectionSkill;

    protected SkillManager _SkillManager;

    public void Init(PlayerData caster, Vector3 direction, SkillManager skillManager)
    {
        this._Caster = caster;
        this._DirectionSkill = direction;
        this._SkillManager = skillManager;
    }

    public abstract void activeSkill();

    public PlayerData Caster
    {
        get { return _Caster; }
        set { _Caster = value; }
    }

    public float getCoolDownTime()
    {
        return this._CoolDownTime;
    }
    

}
