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
    private float _Timer = 600f;
    private GameStats _gameStats;
    private List<GameObject> _spawnedCharacters = new List<GameObject>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Init(List<TeamMember> teamAData, List<TeamMember> teamBData)
    {
        Debug.Log("GameManager: Initializing game...");

        if (MonsterManager.Instance != null)
        {
            MonsterManager.Instance.Init();
            Debug.Log("GameManager: MonsterManager Initialized.");
        }
        else
        {
            Debug.LogError("GameManager: MonsterManager.Instance is NULL! Ensure it's in GameScene and Awake() is properly setting its instance.");
        }

        if (GateManager.Instance != null)
        {
            GateManager.Instance.Init();
            Debug.Log("GameManager: GateManager Initialized.");
        }
        else
        {
            Debug.LogError("GameManager: GateManager.Instance is NULL! Ensure it's in GameScene and Awake() is properly setting its instance.");
        }

        this.TeamA = teamAData;
        this.TeamB = teamBData;

        this.isKey1 = this.isKey2 = this.isKey3 = false;
        this.scoreA = 0;
        this.scoreB = 0;
        this.ownerKeyA = 0;
        this.ownerKeyB = 0;
        this._Timer = _GameDuration;
        this._gameStats = GameStats.Start;

        if (BattleUI.Instance != null)
        {
            BattleUI.Instance.UpdateTeamAScore(scoreA);
            BattleUI.Instance.UpdateTeamBScore(scoreB);
            BattleUI.Instance.UpdateGameTimer(FormatTime(_Timer));
        }

        // Sinh ra các nhân vật cho cả hai đội
        InitializeCharacters(this.TeamA, IsAlly(this.TeamA));
        InitializeCharacters(this.TeamB, IsAlly(this.TeamB));
        Debug.Log("GameManager: Game initialization complete. Characters spawned.");
    }

    private void InitializeCharacters(List<TeamMember> team, bool isAlly)
    {
        Debug.Log($"GameManager: Spawning characters for {(isAlly ? "Ally" : "Enemy")} team...");

        foreach (TeamMember member in team)
        {
            if (member.CharacterPrefab == null)
            {
                Debug.LogError($"GameManager: CharacterPrefab is NULL for team member '{member.PlayerData?.PlayerName ?? "Unknown"}'. Cannot instantiate.");
                continue;
            }

            GameObject characterPrefabInstance = Instantiate(member.CharacterPrefab, member.SpawnPoint, Quaternion.identity);
            _spawnedCharacters.Add(characterPrefabInstance);

            PlayerController playerController = characterPrefabInstance.GetComponent<PlayerController>();
            if (playerController == null)
            {
                Debug.LogError($"GameManager: PlayerController component missing on prefab '{member.CharacterPrefab.name}'. Destroying instance.");
                Destroy(characterPrefabInstance);
                continue;
            }

            if (isAlly)
            {
                if (GameProfile.Instance == null || GameProfile.Instance.GetPlayerData() == null)
                {
                    Debug.LogError("GameManager: GameProfile.Instance or PlayerData is NULL. Cannot set player ownership correctly.");
                    characterPrefabInstance.tag = "Ally";
                    playerController.SetIsOwner(false);
                }
                else if (member.PlayerData != null && member.PlayerData.PlayerId.Equals(GameProfile.Instance.GetPlayerData().PlayerId))
                {
                    characterPrefabInstance.tag = "Player";
                    playerController.SetIsOwner(true);
                }
                else
                {
                    characterPrefabInstance.tag = "Ally";
                    playerController.SetIsOwner(false);
                }
            }
            else
            {
                characterPrefabInstance.tag = "Enemy";
                playerController.SetIsOwner(false);
            }

            playerController.Init(member.SpawnPoint, member.PlayerData);
            Debug.Log($"GameManager: Spawned {characterPrefabInstance.tag} '{member.PlayerData?.PlayerName ?? "N/A"}' at {member.SpawnPoint}.");
        }
    }

    public void ResetGame()
    {
        
        NotificationUI("Game resetting...");

        Debug.Log("GameManager: Destroying old characters...");
        foreach (GameObject character in _spawnedCharacters)
        {
            if (character != null)
            {
                Destroy(character);
            }
        }
        _spawnedCharacters.Clear();

        Debug.Log("GameManager: Resetting game state variables.");
        this.isKey1 = this.isKey2 = this.isKey3 = false;
        this.ownerKeyA = 0;
        this.ownerKeyB = 0;
        this._Timer = _GameDuration;
        this._gameStats = GameStats.Start;

        if (BattleUI.Instance != null)
        {
            BattleUI.Instance.UpdateTeamAScore(scoreA);
            BattleUI.Instance.UpdateTeamBScore(scoreB);
            BattleUI.Instance.UpdateGameTimer(FormatTime(_Timer));
        }

        Debug.Log("GameManager: Spawning new characters for reset game.");
        InitializeCharacters(this.TeamA, IsAlly(this.TeamA));
        InitializeCharacters(this.TeamB, IsAlly(this.TeamB));
        GateManager.Instance.Init();
        MonsterManager.Instance.Init();
        Debug.Log("GameManager: Game reset complete.");
    }

    void Update()
    {
        if (_gameStats == GameStats.Start)
        {
            _Timer -= Time.deltaTime;

            if (_Timer <= 0)
            {
                _gameStats = GameStats.End;
                Debug.Log("GameManager: Game Over! Time's up.");
                Summary();
            }

            // Kiểm tra nếu tất cả các key đã được truy cập
            // if (isKey1 && isKey2 && isKey3)
            // {
            //     Debug.Log("GameManager: All keys have been accessed. Proceeding to next stage/opening gate.");
            //     if (GateManager.Instance != null)
            //     {
            //         GateManager.Instance.OpenGate(); // Yêu cầu GateManager mở cổng
            //     }
            //     else
            //     {
            //         Debug.LogWarning("GameManager: GateManager.Instance is NULL. Cannot open gate.");
            //     }
            // }

            // Test
            if (isKey1 || isKey2 || isKey3)
            {
                Debug.Log("GameManager: All keys have been accessed. Proceeding to next stage/opening gate.");
                if (GateManager.Instance != null)
                {
                    GateManager.Instance.OpenGate(); // Yêu cầu GateManager mở cổng
                }
                else
                {
                    Debug.LogWarning("GameManager: GateManager.Instance is NULL. Cannot open gate.");
                }
            }

            UpdateTimeUI();

            if ((this.scoreA == 2) || (this.scoreB == 2))
            {
                this.EndGame();
            }
        }
    }

    public void Winner(PlayerController player)
    {
        _gameStats = GameStats.End; // Ngay lập tức đặt trạng thái game kết thúc

        if (IsPlayerInTeam(player)) // Kiểm tra xem người chơi thuộc đội nào
        {
            scoreA++; // Tăng điểm cho Team A
            if (BattleUI.Instance != null) BattleUI.Instance.UpdateTeamAScore(scoreA);
            Debug.Log($"GameManager: Team A wins! Current Score: {scoreA}");
            NotificationUI("Team A wins!"); // Thông báo Team A chiến thắng
        }
        else
        {
            scoreB++; // Tăng điểm cho Team B
            if (BattleUI.Instance != null) BattleUI.Instance.UpdateTeamBScore(scoreB);
            Debug.Log($"GameManager: Team B wins! Current Score: {scoreB}");
            NotificationUI("Team B wins!"); // Thông báo Team B chiến thắng
        }

        NotificationUI("Game will reset in 10 seconds..."); // Thông báo đếm ngược trước khi reset

        // Bắt đầu Coroutine để đợi 10 giây trước khi reset game
        StartCoroutine(ResetGameAfterDelay(10f));
    }

    private IEnumerator ResetGameAfterDelay(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds); // Chờ X giây

        Debug.Log("GameManager: Initiating game reset after delay.");
        ResetGame(); // Gọi phương thức reset game
        // Khởi tạo lại GateManager (nếu cần thiết cho vòng chơi mới)
        if (GateManager.Instance != null)
        {
            GateManager.Instance.Init();
        }
        else
        {
            Debug.LogWarning("GameManager: GateManager.Instance is NULL during reset. Cannot re-initialize.");
        }
    }

    public void AccessKey(PlayerController player)
    {
        if (IsPlayerInTeam(player)) // Xác định đội của người chơi
        {
            ownerKeyA++;
            this.NotificationUI("Team A accessed a key!");
        }
        else
        {
            ownerKeyB++;
            this.NotificationUI("Team B accessed a key!"); // Đã sửa logic thông báo cho Team B
        }

        // Đánh dấu key nào đã được truy cập (logic này có thể cần được cải tiến để xác định key cụ thể)
        if (!isKey1)
        {
            isKey1 = true;
            Debug.Log("GameManager: Key 1 has been accessed.");
        }
        else if (!isKey2)
        {
            isKey2 = true;
            Debug.Log("GameManager: Key 2 has been accessed.");
        }
        else if (!isKey3)
        {
            isKey3 = true;
            Debug.Log("GameManager: Key 3 has been accessed.");
        }
        else
        {
            Debug.Log("GameManager: All keys have already been accessed (or unexpected key access).");
        }
    }

    /// <returns>True nếu người chơi ở Team A, False nếu ở Team B.</returns>
    private bool IsPlayerInTeam(PlayerController player)
    {
        if (player == null || player.GetPlayerData() == null)
        {
            Debug.LogWarning("GameManager: Player or PlayerData is NULL when checking team. Returning false.");
            return false;
        }

        // Duyệt qua Team A để tìm PlayerId
        foreach (TeamMember teamMember in TeamA)
        {
            if (teamMember.PlayerData != null && teamMember.PlayerData.PlayerId.Equals(player.GetPlayerData().PlayerId))
            {
                return true;
            }
        }
        return false; // Nếu không tìm thấy trong Team A, giả định là Team B (hoặc không thuộc team nào)
    }

    /// <returns>True nếu đội là đồng minh (chứa người chơi hiện tại).</returns>
    private bool IsAlly(List<TeamMember> team)
    {
        // Đảm bảo GameProfile và PlayerData của người chơi hiện tại không null
        if (GameProfile.Instance == null || GameProfile.Instance.GetPlayerData() == null)
        {
            Debug.LogWarning("GameManager: GameProfile.Instance or PlayerData is NULL in IsAlly. Defaulting to false (cannot determine ally).");
            return false;
        }

        // Kiểm tra xem PlayerId của người chơi hiện tại có tồn tại trong danh sách đội không
        foreach (TeamMember teamMember in team)
        {
            if (teamMember.PlayerData != null && teamMember.PlayerData.PlayerId.Equals(GameProfile.Instance.GetPlayerData().PlayerId))
            {
                return true;
            }
        }
        return false;
    }

    public void Summary()
    {
        Debug.Log("GameManager: Game Summary invoked (Time's Up!).");
        NotificationUI("Game Over! Returning to lobby...");
        ResetGame();
    }

    private void UpdateTimeUI()
    {
        if (BattleUI.Instance != null)
        {
            BattleUI.Instance.UpdateGameTimer(FormatTime(_Timer));
        }
        else
        {
            Debug.LogWarning("GameManager: BattleUI.Instance is NULL. Cannot update game timer UI.");
        }
    }

    /// <param name="notification">Nội dung thông báo.</param>
    private void NotificationUI(string notification)
    {
        if (BattleUI.Instance != null)
        {
            BattleUI.Instance.EnqueueNotification(notification);
        }
        else
        {
            Debug.LogWarning("GameManager: BattleUI.Instance is NULL. Cannot display notification: " + notification);
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        float minutes = Mathf.FloorToInt(timeInSeconds / 60);
        float seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void EndGame()
    {
        this._gameStats = GameStats.End;
        GameProfile.Instance.LoadHomeScene();
    }
}