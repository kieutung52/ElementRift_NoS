using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    private static ObjectPooling _instance;
    public static ObjectPooling Instance => _instance;

    [SerializeField] private Dictionary<GameObject, List<GameObject>> _pool = new Dictionary<GameObject, List<GameObject>>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public virtual GameObject GetObj(GameObject prefab)
    {
        List<GameObject> listObj = new List<GameObject>();
        if (_pool.ContainsKey(prefab))
            listObj = _pool[prefab];
        else
        {
            _pool.Add(prefab, listObj);
        }

        foreach (GameObject g in listObj)
        {
            if (g.activeSelf)
                continue;
            return g;
        }

        GameObject g2 = Instantiate(prefab, this.transform.position, Quaternion.identity);
        listObj.Add(g2);

        return g2;
    }

    public virtual T GetComp<T>(T prefab) where T : MonoBehaviour
    {
        return this.GetObj(prefab.gameObject).GetComponent<T>();
    }
}
