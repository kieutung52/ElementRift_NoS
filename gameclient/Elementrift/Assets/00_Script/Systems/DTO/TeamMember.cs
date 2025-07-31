using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TeamMember
{
    public PlayerData PlayerData;
    public GameObject CharacterPrefab;

    public Vector3 SpawnPoint;

    public TeamMember(PlayerData player, GameObject go, Vector3 Spawn)
    {
        this.PlayerData = player;
        this.CharacterPrefab = go;
        this.SpawnPoint = Spawn;
    }
    public void Display()
    {
        Debug.LogWarning($"Du lieu cua {PlayerData.PlayerName}");
        Debug.LogWarning($"{PlayerData.PlayerId}");
        Debug.LogWarning($"{PlayerData.CharacterId}");
        Debug.LogWarning($"{CharacterPrefab.gameObject.name}");
        Debug.LogWarning($"{SpawnPoint}");
    }
}
