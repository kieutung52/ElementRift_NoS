using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProfile : MonoBehaviour
{
    private static GameProfile _instance;
    public static GameProfile Instance => _instance;

    private PlayerData _playerData;

    [SerializeField] private List<TeamMember> _teamMemberA = new List<TeamMember>();
    [SerializeField] private List<TeamMember> _teamMemberB = new List<TeamMember>();

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
        SceneManager.LoadScene("GameScene");
        
        // Bắt đầu một coroutine để đợi 1 frame trước khi khởi tạo GameManager
        StartCoroutine(InitializeGameManagerAfterSceneLoad());
    }

    private IEnumerator InitializeGameManagerAfterSceneLoad()
    {
        // Chờ 1 frame để tất cả các Awake() của Scene mới được chạy
        yield return null; 

        if (GameManager.Instance != null)
        {
            GameManager.Instance.Init(this._teamMemberA, this._teamMemberB);
            Debug.Log("GameManager Initialized.");
        }
        else
        {
            Debug.LogError("GameManager.Instance is null! Make sure it's in GameScene and has Awake() setup correctly.");
        }
    }

    public void LoadHomeScene()
    {
        SceneManager.LoadScene("HomeScene");
        _teamMemberA.Clear();
        _teamMemberB.Clear();
    }

    public PlayerData GetPlayerData()
    {
        return _playerData;
    }

    public void DebugPrintProfile()
    {
        Debug.LogError("DEBUG");
        Debug.LogWarning($"Player Name: {_playerData.PlayerName}, Player ID: {_playerData.PlayerId}, Character ID: {_playerData.CharacterId}");
        Debug.LogError("team A:");
        foreach (TeamMember memeber in _teamMemberA)
        {
            memeber.Display();
        }
        Debug.LogError("team A:");
        foreach (TeamMember memeber in _teamMemberB)
        {
            memeber.Display();
        }
    }
}
