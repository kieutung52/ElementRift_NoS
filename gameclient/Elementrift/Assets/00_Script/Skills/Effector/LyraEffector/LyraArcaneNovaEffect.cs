using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LyraArcaneNovaEffect : EffectorBase
{
    [SerializeField] private float _RadiusPrefab;
    [SerializeField] private float _RadiusEffect;
    private IGetHit _GetHit;
    public override void activeEffector()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, this._RadiusEffect);
        
        foreach (Collider collider in colliders)
        {
            _GetHit = collider.GetComponentInParent<IGetHit>();
            if ((_GetHit != null) && collider.CompareTag("Enemy"))
            {
                _GetHit.TakeDamage(this.getCasterInfo(), this.getDamage());
            }
        }
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
        Gizmos.DrawSphere(this.transform.position, _RadiusPrefab);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.transform.position, _RadiusEffect);
    }
}
