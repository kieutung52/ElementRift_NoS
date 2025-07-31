using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvataManager : MonoBehaviour
{
    private static AvataManager _instance;
    public static AvataManager Instance => _instance;
    [SerializeField] private Sprite _avatarOwnerPrefab;
    [SerializeField] private Sprite _avatarEnemyPrefab;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance.GetInstanceID() != this.GetComponent<AvataManager>().GetInstanceID())
        {
            Destroy(this.GetComponent<AvataManager>());
        }
    }
    public Sprite GetAvatar(bool isOwner)
    {
        if (isOwner)
        {
            return _avatarOwnerPrefab;
        }
        else
        {
            return _avatarEnemyPrefab;
        }
    }
}
