using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private static MonsterManager _instance;
    public static MonsterManager Instance => _instance;
    [SerializeField] private List<MonsterController> _monsters;
    private GameObject _monsterPrefab;
    private List<GameObject> _KeyObject = new List<GameObject>();
    private List<GameObject> _MonsterObject = new List<GameObject>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this.GetComponent<MonsterManager>();
        }
        else if (_instance.GetInstanceID() != this.GetComponent<MonsterManager>().GetInstanceID())
        {
            Destroy(this.GetComponent<MonsterManager>());
        }
    }
    public void Init()
    {
        foreach (GameObject keyitem in _KeyObject)
        {
            Destroy(keyitem);
        }
        foreach (GameObject monster in _MonsterObject)
        {
            Destroy(monster);
        }
        foreach (MonsterController monster in _monsters)
        {
            _monsterPrefab = Instantiate(monster.gameObject, monster.GetSpawnPosition(), Quaternion.identity);
            _monsterPrefab.GetComponent<MonsterController>().Init();
            this._MonsterObject.Add(_monsterPrefab);
        }
    }

    public void AddGameObjKey(GameObject key)
    {
        this._KeyObject.Add(key);
    }
}
