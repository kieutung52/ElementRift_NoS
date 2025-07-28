using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProfile : MonoBehaviour
{
    private static GameProfile _instance;
    public static GameProfile Instance => _instance;

    [SerializeField] private GameObject _GameManger;
    private PlayerData _playerData;
    private string _roomID;

    private List<TeamMember> _teamMemberA = new List<TeamMember>();
    private List<TeamMember> _teamMemberB = new List<TeamMember>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance.GetInstanceID() != this.GetComponent<GameProfile>().GetInstanceID())
        {
            Destroy(this.GetComponent<GameProfile>());
        }
    }

    public void Init(PlayerData playerData)
    {
        _playerData = playerData;
    }

    public void SetRoomID(string roomID)
    {
        _roomID = roomID;
    }

    public void AddMemberToTeamA(TeamMember member)
    {
        if (_teamMemberA.Count < 3)
        {
            _teamMemberA.Add(member);
        }
        else
        {
            Debug.LogError("Team A is full!");
        }
    }

    public void AddMemberToTeamB(TeamMember member)
    {
        if (_teamMemberB.Count < 3)
        {
            _teamMemberB.Add(member);
        }
        else
        {
            Debug.LogError("Team B is full!");
        }
    }

    public void StartGame()
    {
        this._GameManger.SetActive(true);
        GameManager.Instance.Init(_teamMemberA, _teamMemberB);
    }
    
    public PlayerData GetPlayerData()
    {
        return _playerData;
    }
}
