using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private List<SkillBase> _skills = new List<SkillBase>(4);
    private Dictionary<SkillBase, float> _cooldowns = new Dictionary<SkillBase, float>();
    [SerializeField] private SkillBase _skillCurrent;

    public void Init(PlayerData caster)
    {
        foreach (SkillBase skill in _skills)
        {
            skill.Init(caster, this.transform.forward.normalized, this.GetComponent<SkillManager>());
            this._cooldowns[skill] = 0f;
        }
        this.ChangeSkill(1);
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
        this._skillCurrent.activeSkill();
        _cooldowns[_skillCurrent] = this._skillCurrent.getCoolDownTime();
    }

    void OnDrawGizmosSelected()
    {
        Debug.LogError("position: " + this.GetComponent<SkillManager>().transform.position);
        Debug.LogError("Local Postion" + this.GetComponent<SkillManager>().transform.localPosition);
    }
}
