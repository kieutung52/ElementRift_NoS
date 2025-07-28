using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LyraNormalEffect : EffectorBase
{
    [SerializeField] private float _Radius;
    private IGetHit _GetHit;
    public override void activeEffector()
    {
        this._GetHit.TakeDamage(this.getCasterInfo(), this.getDamage());
        this.disableEffector();
    }

    private void OnTriggerEnter(Collider other)
    {
        _GetHit = other.GetComponentInParent<IGetHit>();
        if ((_GetHit != null) && other.CompareTag("Enemy"))
        {
            this.activeEffector();
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(this.transform.position, this._Radius);
    }
}
