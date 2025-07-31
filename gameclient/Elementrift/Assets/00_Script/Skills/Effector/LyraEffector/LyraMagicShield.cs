using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LyraMagicShield : EffectorBase
{
    [SerializeField] private float _AmountDef;
    public override void activeEffector()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        EffectorBase _effector = other.GetComponent<EffectorBase>();
        if (_effector == null) return;
        if (_effector.getDamage() > _AmountDef)
        {
            this.disableEffector();
        }
        else
        {
            this._AmountDef -= _effector.getDamage();
            _effector.disableEffector();
        }

    }

    void OnEnable()
    {
        this.transform.localPosition = Vector3.zero;
    }
}
