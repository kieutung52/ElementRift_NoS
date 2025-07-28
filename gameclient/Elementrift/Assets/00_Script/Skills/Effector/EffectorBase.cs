using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public abstract class EffectorBase : MonoBehaviour
{
    [SerializeField] private PlayerData _Caster;
    [SerializeField] private float _Damage;
    [SerializeField] private float _LifeTime;
    [SerializeField] private Rigidbody _Rb;
    [SerializeField] private float _SpeedEffector;
    [SerializeField] private float _Distance;
    private float _Timer;
    private Vector3 _Direction;

    public abstract void activeEffector();

    public void disableEffector()
    {
        this._Rb.velocity = Vector3.zero;
        this.gameObject.SetActive(false);
    }

    public void Init(PlayerData caster, float damage, Vector3 Position, Vector3 direction)
    {
        this._Caster = caster;
        this._Damage = damage;
        this._LifeTime = _Distance / _SpeedEffector;
        this._Timer = _LifeTime;
        this.transform.localPosition = Position;
        this._Direction = direction;
    }

    void Update()
    {
        _Timer -= Time.deltaTime;
        if (_Timer < 0)
        {
            this.disableEffector();
        }
        if (Vector3.Angle(transform.forward, _Direction) > 1f)
        {
            transform.rotation = Quaternion.LookRotation(_Direction);
        }
    }

    void FixedUpdate()
    {
        this._Rb.velocity = _Direction * _SpeedEffector;
    }

    public PlayerData getCasterInfo()
    {
        return _Caster;
    }

    public float getDamage()
    {
        return _Damage;
    } 
}
