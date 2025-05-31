using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharacterStats
{
    public float _health;
    public float _maxHealth;
    public float _armor;

    public float _mana;
    public float _maxMana;
    public float _baseDamage;
    public float _movementSpeed;
    public float _jumpForce;

    public CharacterStats(float health, float maxHealth, float armor, float mana, float maxMana, float baseDamage, float movementSpeed, float jumpForce)
    {
        _health = health;
        _maxHealth = maxHealth;
        _armor = armor;
        _mana = mana;
        _maxMana = maxMana;
        _baseDamage = baseDamage;
        _movementSpeed = movementSpeed;
        _jumpForce = jumpForce;
    }
    public void ResetStats()
    {
        _health = _maxHealth;
        _mana = _maxMana;
    }
}

