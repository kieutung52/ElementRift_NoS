using System.Collections.Generic;
using UnityEngine;

public class EnemiesInRange : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private LayerMask _obstacleMask;

    private List<GameObject> enemiesInRange = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if ((other.GetComponentInParent<IGetHit>() != null) && other.CompareTag("Enemy"))
        {
            if (!enemiesInRange.Contains(other.gameObject))
                enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.GetComponentInParent<IGetHit>() != null) && other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    public GameObject GetClosestEnemy()
    {
        if (enemiesInRange.Count == 0) return null;

        GameObject closest = null;
        float minDist = float.MaxValue;

        foreach (GameObject enemy in enemiesInRange)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = enemy;
            }
        }
        return closest;
    }

    public GameObject GetBestVisibleEnemy()
    {
        GameObject bestEnemy = null;
        float minAngle = float.MaxValue;

        foreach (GameObject enemy in enemiesInRange)
        {
            Vector3 dirToEnemy = (enemy.transform.position - _playerTransform.position).normalized;
            float angle = Vector3.Angle(_playerTransform.forward, dirToEnemy);
            if (angle > 70f) continue;

            float distance = Vector3.Distance(_playerTransform.position, enemy.transform.position);
            Ray ray = new Ray(_playerTransform.position, dirToEnemy);
            if (Physics.Raycast(ray, out RaycastHit hit, distance, _obstacleMask))
            {
                if ((hit.collider.GetComponentInParent<IGetHit>() == null) || !hit.collider.CompareTag("Enemy"))
                {
                    // Bị che bởi vật cản, không phải enemy
                    continue;
                }
            }

            if (angle < minAngle)
            {
                minAngle = angle;
                bestEnemy = enemy;
            }
        }
        Debug.LogError($"Best Enemy: {bestEnemy?.name}, Angle: {minAngle}");
        return bestEnemy;
    }
}
