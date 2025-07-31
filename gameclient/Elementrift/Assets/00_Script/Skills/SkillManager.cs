using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private List<SkillBase> _skills = new List<SkillBase>(4);
    private Dictionary<SkillBase, float> _cooldowns = new Dictionary<SkillBase, float>();
    [SerializeField] private SkillBase _skillCurrent;
    private Character _character;

    public void Init(PlayerData caster, Character character)
    {
        if (character == null)
        {
            Debug.LogError("chracter = null");
        }
        if (caster == null)
        {
            Debug.LogError("caster = null");
        }
        foreach (SkillBase skill in _skills)
        {
            skill.Init(caster, this.transform.forward.normalized, this.GetComponent<SkillManager>());
            this._cooldowns[skill] = 0f;
        }
        this.ChangeSkill(1);
        this._character = character;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (SkillBase skill in _skills)
        {
            if (_cooldowns.ContainsKey(skill))
            {
                _cooldowns[skill] -= Time.deltaTime;
                if (_cooldowns[skill] < 0f)
                {
                    _cooldowns[skill] = 0f;
                }
            }
        }
    }

    public void ChangeSkill(int index)
    {
        if (index == 1)
        {
            this._skillCurrent = _skills[0];

        }
        else if (index == 2)
        {
            this._skillCurrent = _skills[1];
        }
        else if (index == 3)
        {
            this._skillCurrent = _skills[2];
        }
        else if (index == 4)
        {
            this._skillCurrent = _skills[3];
        }
    }

    public void UseSkill()
    {
        if (this._cooldowns[_skillCurrent] > 0f)
        {
            Debug.LogWarning("Skill is on cooldown: " + this._cooldowns[_skillCurrent]);
            return;
        }
        this._skillCurrent.activeSkill(this._character.GetCharacterStats()._baseDamage);
        this._character.UseMana(this._skillCurrent.getManaCost());
        _cooldowns[_skillCurrent] = this._skillCurrent.getCoolDownTime();
    }
}
