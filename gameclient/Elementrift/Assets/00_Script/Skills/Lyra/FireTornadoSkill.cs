using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTornadoSkill : SkillBase
{
    private GameObject _EnemyTarger;
    [SerializeField] private LayerMask _obstacleMask;
    public override void activeSkill(float Basedamge)
    {
        this._EnemyTarger = this.GetEnemyTarget();
        EffectorBase effectorPool = ObjectPooling.Instant.GetComp<EffectorBase>(_Effector);
        effectorPool.Init(_Caster, _Damage + Basedamge, _EnemyTarger.transform.position, Vector3.zero);
        effectorPool.transform.parent = this._EnemyTarger.transform;
        effectorPool.transform.localPosition = Vector3.zero;
        effectorPool.gameObject.SetActive(true);
    }

    private GameObject GetEnemyTarget()
    {
        GameObject bestEnemy = null;
        float minAngle = float.MaxValue, angle, distance;
        Vector3 dirToEnemy;

        Collider[] colliders = Physics.OverlapSphere(this._SkillManager.transform.position, 500f);

        foreach (Collider enemyCollider in colliders)
        {
            if ((enemyCollider.GetComponentInParent<IGetHit>() == null) && !enemyCollider.CompareTag("Enemy")) continue;

            GameObject enemy = enemyCollider.gameObject;
            dirToEnemy = (enemy.transform.position - this._SkillManager.transform.position).normalized;
            angle = Vector3.Angle(this._SkillManager.transform.forward, dirToEnemy);

            if (angle > 70f) continue;

            distance = Vector3.Distance(this._SkillManager.transform.position, enemy.transform.position);
            Ray ray = new Ray(this._SkillManager.transform.position, dirToEnemy);
            if (Physics.Raycast(ray, out RaycastHit hit, distance, _obstacleMask))
            {
                if (hit.collider.gameObject != enemy)
                {
                    continue;
                }
            }

            if (angle < minAngle)
            {
                minAngle = angle;
                bestEnemy = enemy;
            }
        }

        return bestEnemy;
    }
}
