using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Using ObjectPooling to optimize performance
public class ObjectPooling : MonoBehaviour
{
    // using design pattern Singleton
    private static ObjectPooling _instant;
    public static ObjectPooling Instant => _instant;

    private Dictionary<GameObject, List<GameObject>> _pools = new Dictionary<GameObject, List<GameObject>>();

    private void Awake()
    {
        if (_instant == null)
        {
            _instant = this.GetComponent<ObjectPooling>();
        }
        else if (_instant.GetInstanceID() != this.GetComponent<ObjectPooling>().GetInstanceID())
        {
            Destroy(this.GetComponent<ObjectPooling>());
        }
    }


    // Initialize all effects to optimize performance
    public void Init(List<GameObject> prefabs)
    {
        foreach (GameObject gameObjectprefabsItem in prefabs)
        {
            gameObjectprefabsItem.SetActive(false);
            List<GameObject> gameObjectsprefabsList = new List<GameObject>();
            _pools.Add(gameObjectprefabsItem, gameObjectsprefabsList);
            gameObjectsprefabsList.Add(gameObjectprefabsItem);
        }
    }

    public virtual GameObject GetObject(GameObject prefab)
    {
        List<GameObject> listObject = new List<GameObject>();

        if (_pools.ContainsKey(prefab))
        {
            listObject = _pools[prefab];
        }
        else
        {
            _pools.Add(prefab, listObject);
        }


        foreach (GameObject go in listObject)
        {
            if (go.activeSelf)
            {
                continue;
            }
            return go;
        }

        GameObject go2 = Instantiate(prefab, this.transform.position, Quaternion.identity);
        listObject.Add(go2);
        return go2;
    }

    public virtual T GetComp<T>(T prefab) where T : MonoBehaviour
    {
        return this.GetObject(prefab.gameObject).GetComponent<T>();
    }
}
