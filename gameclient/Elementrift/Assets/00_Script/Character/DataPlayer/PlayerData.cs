using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [SerializeField] private string _playerName;
    [SerializeField] private string _playerId;

    [SerializeField] private string _characterId;

    public string PlayerName
    {
        get => _playerName;
        set => _playerName = value;
    }
    public string PlayerId
    {
        get => _playerId;
        set => _playerId = value;
    }

    public string CharacterId
    {
        get => _characterId;
        set => _characterId = value;
    }
    public PlayerData(string playerName, string playerId, string characterId)
    {
        _playerName = playerName;
        _playerId = playerId;
        _characterId = characterId;
    }
}
