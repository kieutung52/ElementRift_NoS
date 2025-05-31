using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [SerializeField] private string _playerName;
    [SerializeField] private string _playerId;

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
    public PlayerData(string playerName, string playerId)
    {
        _playerName = playerName;
        _playerId = playerId;
    }
}
