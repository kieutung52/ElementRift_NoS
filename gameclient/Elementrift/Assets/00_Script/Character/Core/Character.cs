using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IGetHit
{
    [SerializeField] private string _characterId;
    [SerializeField] private CharacterClass _characterClass;
    [SerializeField] private ElementType _elementType;

    [SerializeField] private CharacterStats _characterStats;


    public void Init(CharacterStats statsBase)
    {
        _characterStats = statsBase;
    }

    public bool IsAlive()
    {
        return _characterStats._health > 0;
    }


    public void TakeDamage(PlayerData caster, float dmg)
    {
        float damageAfterArmor = dmg - this._characterStats._armor;
        if (damageAfterArmor < 0)
        {
            damageAfterArmor = 0;
            this._characterStats._armor -= dmg; // Reduce armor if damage is less than armor
        }
        _characterStats._health -= damageAfterArmor;
        if (_characterStats._health <= 0)
        {
            _characterStats._health = 0;
            Debug.Log($"{this.name} has died.");
            /** 
            ==============================
                Can add death logic here
                <GameManager.Instance.NotifyDeath(this, caster);>
            ==============================
            **/
        }
        else
        {
            if (caster == null)
            {
                Debug.LogWarning("Caster is monster");
                return;
            }
            Debug.Log($"{this.name} took {damageAfterArmor} damage from {caster.PlayerName}. Remaining health: {_characterStats._health}");
        }
    }

    public void Heal(float amount)
    {
        if (!IsAlive())
        {
            Debug.LogWarning("Cannot heal a dead character.");
            return;
        }
        _characterStats._health += amount;
        if (_characterStats._health > _characterStats._maxHealth)
        {
            _characterStats._health = _characterStats._maxHealth;
        }
    }

    public void UseMana(float amount)
    {
        if (_characterStats._mana >= amount)
        {
            _characterStats._mana -= amount;
        }
        else
        {
            Debug.LogWarning("Not enough mana to perform this action.");
        }
    }

    public void RestoreMana(float amount)
    {
        _characterStats._mana += amount;
        if (_characterStats._mana > _characterStats._maxMana)
        {
            _characterStats._mana = _characterStats._maxMana;
        }
    }
    
    public CharacterStats GetCharacterStats()
    {
        return _characterStats;
    }
}
