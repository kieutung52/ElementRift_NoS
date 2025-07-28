using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LyraFireTornadoEffect : EffectorBase
{
    [SerializeField] private float _Radius;
    private IGetHit _GetHit;
    private float _Duration = 5f;
    private float _ElapsedTime = 0f;

    void Awake()
    {
        this.transform.parent = this.transform;
        this.transform.localPosition = Vector3.zero;
        this._ElapsedTime = this._Duration;
    }
    public override void activeEffector()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, this._Radius);
        foreach (Collider collider in colliders)
        {
            _GetHit = collider.GetComponentInParent<IGetHit>();
            if (_GetHit != null)
            {
                _GetHit.TakeDamage(this.getCasterInfo(), this.getDamage());
            }
        }
    }

    void Update()
    {
        if (_ElapsedTime <= 0f)
        {
            this.transform.parent = null;
            this._ElapsedTime = this._Duration;
            this.disableEffector();
            return;
        }
        _ElapsedTime -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        _GetHit = other.GetComponentInParent<IGetHit>();
        if (_GetHit != null)
        {
            this.activeEffector();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(this.transform.position, _Radius);
    }
}
