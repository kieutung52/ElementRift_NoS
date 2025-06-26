using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LyraFireTornadoEffect : EffectorBase
{
    [SerializeField] private float _Radius;
    private IGetHit _GetHit;
    public override void activeEffector()
    {
        
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, this._Radius);
        foreach (Collider collider in colliders)
        {
            _GetHit = collider.gameObject.GetComponent<IGetHit>();
            if (_GetHit != null)
            {
                _GetHit.TakeDamage(this.getCasterInfo(), this.getDamage());
            }
        }
        this.disableEffector();
    }

    private void OnTriggerEnter(Collider other)
    {
        _GetHit = other.GetComponent<IGetHit>();
        if (_GetHit != null)
        {
            this.activeEffector();
        }
        // this.disableEffector();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(this.transform.position, this._Radius);
    }
}
