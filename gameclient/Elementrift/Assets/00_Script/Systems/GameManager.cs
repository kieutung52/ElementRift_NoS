using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [SerializeField] List<TeamMember> TeamA = new List<TeamMember>(3);
    [SerializeField] List<TeamMember> TeamB = new List<TeamMember>(3);

    [SerializeField] bool isKey1, isKey2, isKey3;
    [SerializeField] int scoreA, scoreB, ownerKeyA, ownerKeyB;
    private float _GameDuration = 600f;
    private float _Timer;
    private GameStats _gameStats;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this.GetComponent<GameManager>();
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance.GetInstanceID() != this.GetComponent<GameManager>().GetInstanceID())
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update

    public void Init(List<TeamMember> TeamA, List<TeamMember> TeamB)
    {
        MonsterManager.Instance.Init();
        GateManager.Instance.Init();
        this.TeamA = TeamA;
        this.TeamB = TeamB;
        this.isKey1 = this.isKey2 = this.isKey3 = false;
        this.scoreA = this.scoreB = 0;
        this.ownerKeyA = this.ownerKeyB = 0;
        this._Timer = _GameDuration;
        this._gameStats = GameStats.Start;
        InitializeCharacters(TeamA, this.IsAlly(TeamA));
        InitializeCharacters(TeamB, this.IsAlly(TeamB));
    }

    private void InitializeCharacters(List<TeamMember> team, bool isAlly)
    {
        GameObject characterPrefab;
        foreach (TeamMember member in team)
        {
            characterPrefab = Instantiate(member.CharacterPrefab, member.SpawnPoint.position, Quaternion.identity);
            if (isAlly)
            {
                if (member.PlayerData.PlayerId.Equals(GameProfile.Instance.GetPlayerData().PlayerId))
                {
                    characterPrefab.tag = "Player";
                    characterPrefab.GetComponent<PlayerController>().SetIsOwner(true);
                    characterPrefab.GetComponent<PlayerController>().Init(member.SpawnPoint.position, member.PlayerData);
                    continue;
                }
                characterPrefab.tag = "Ally";
            }
            else
            {
                characterPrefab.tag = "Enemy";
            }
            characterPrefab.GetComponent<PlayerController>().SetIsOwner(false);
            characterPrefab.GetComponent<PlayerController>().Init(member.SpawnPoint.position, member.PlayerData);
        }

    }

    public void ResetGame()
    {
        this.isKey1 = this.isKey2 = this.isKey3 = false;
        this.ownerKeyA = this.ownerKeyB = 0;
        this._Timer = _GameDuration;
        this._gameStats = GameStats.Start;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameStats == GameStats.Start)
        {
            _Timer -= Time.deltaTime;
            if (_Timer <= 0)
            {
                _gameStats = GameStats.End;
                Debug.Log("Game Over! Time's up.");
                Summary();
            }
            if (isKey1 && isKey2 && isKey3)
            {
                Debug.Log("All keys have been accessed. Game can proceed to the next stage.");
                GateManager.Instance.OpenGate();
            }
            this.UpdateTimeUI();
        }

    }
    public void Winner(PlayerController player)
    {
        if (IsPlayerInTeam(player))
        {
            scoreA++;
            BattleUI.Instance.UpdateTeamAScore(scoreA);
            Debug.Log($"Team A wins! Score: {scoreA}");
        }
        else
        {
            scoreB++;
            BattleUI.Instance.UpdateTeamBScore(scoreB);
            Debug.Log($"Team B wins! Score: {scoreB}");
        }
        _gameStats = GameStats.End;

        // Reset the game or handle the win logic
        ResetGame();
        GateManager.Instance.Init();
    }

    public void AccessKey(PlayerController player)
    {
        if (IsPlayerInTeam(player))
        {
            ownerKeyA++;
        }
        else
        {
            ownerKeyB++;
        }


        if (!isKey1)
        {
            isKey1 = true;
            Debug.Log("Key 1 has been accessed.");
        }
        else if (!isKey2)
        {
            isKey2 = true;
            Debug.Log("Key 2 has been accessed.");
        }
        else if (!isKey3)
        {
            isKey3 = true;
            Debug.Log("Key 3 has been accessed.");
        }
        else
        {
            Debug.Log("All keys have already been accessed.");
        }
    }

    // If retrun true then player is in the TeamA
    // If return false then player is in the TeamB
    private bool IsPlayerInTeam(PlayerController player)
    {
        foreach (TeamMember teamMember in TeamA)
        {
            if (teamMember.PlayerData.PlayerId.Equals(player.GetPlayerData().PlayerId))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsAlly(List<TeamMember> team)
    {
        foreach (TeamMember teamMember in team)
        {
            if (teamMember.PlayerData.PlayerId.Equals(GameProfile.Instance.GetPlayerData().PlayerId))
            {
                return true;
            }
        }
        return false;
    }

    public void Summary()
    {
        // Destroy all monsters and reset the game state
    }
    
    private void UpdateTimeUI()
    {
        float minutes = Mathf.FloorToInt(_Timer / 60);
        float seconds = Mathf.FloorToInt(_Timer % 60);
        string time = string.Format("{0:00}:{1:00}", minutes, seconds);
        BattleUI.Instance.UpdateGameTimer(time);
    }
}
