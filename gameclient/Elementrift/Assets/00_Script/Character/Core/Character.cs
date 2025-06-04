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
        throw new System.NotImplementedException();
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
    
    public CharacterStats GetCharacterStats()
    {
        return _characterStats;
    }
}
