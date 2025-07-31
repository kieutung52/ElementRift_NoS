using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeUI : MonoBehaviour
{
    private static HomeUI _instance;
    public static HomeUI Instance => _instance;
    [SerializeField] private GameObject _roomPanel;
    [SerializeField] private GameObject _selectCharacterPanel;
    [SerializeField] private List<SlotData> _slotDataList = new List<SlotData>();
    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance.GetInstanceID() != this.GetComponent<HomeUI>().GetInstanceID())
        {
            Destroy(this.GetComponent<HomeUI>());
        }
    }

    void Start()
    {
        this.Init();
    }
    public void Init()
    {
        _roomPanel.SetActive(true);
        _selectCharacterPanel.SetActive(false);
        foreach (SlotData slotData in _slotDataList)
        {
            slotData.ExitPosition();
        }
        this._slotDataList[0].EnterPosition();
    }

    public void EnterStartGame()
    {
        foreach (SlotData slotData in _slotDataList)
        {
            if (slotData.IsSelected)
            {
                if (slotData.IsTeamA)
                {
                    GameProfile.Instance.AddMemberToTeamA(new TeamMember (slotData.PlayerData,CharacterPrefabsManager.Instance.GetPrefab(slotData.PlayerData.CharacterId),slotData.SpawnPoint ));
                }
                else
                {
                    GameProfile.Instance.AddMemberToTeamB(new TeamMember (slotData.PlayerData,CharacterPrefabsManager.Instance.GetPrefab(slotData.PlayerData.CharacterId),slotData.SpawnPoint));
                }
            }
        }
        // GameProfile.Instance.DebugPrintProfile();
        _selectCharacterPanel.SetActive(false);
        GameProfile.Instance.StartGame();
    }

    public void PlayerSelectCharacter(string characterId)
    {
        foreach (SlotData slotData in _slotDataList)
        {
            if (slotData.IsSelected && slotData.PlayerData.PlayerId.Equals(GameProfile.Instance.GetPlayerData().PlayerId))
            {
                slotData.PlayerData.CharacterId = SanitizeInput(characterId); // Default character selection
                return;
            }
        }
        Debug.LogWarning("No slot is selected for the player.");
    }

    public void EnterSelectCharacter()
    {
        _roomPanel.SetActive(false);
        _selectCharacterPanel.SetActive(true);
        this.SelectCharacterForEnemy();
    }

    private void SelectCharacterForEnemy()
    {
        foreach (SlotData slotData in _slotDataList)
        {
            if (slotData.IsSelected && slotData.PlayerData.PlayerId.Equals("EnemyId"))
            {
                slotData.PlayerData.CharacterId = "Base_001";
            }
        }
        Debug.LogWarning("All slots are already selected for the enemy.");
    }

    public void AddEnemy()
    {
        foreach (SlotData slotData in _slotDataList)
        {
            if (!slotData.IsSelected)
            {
                slotData.EnemyEnterPosition();
                return;
            }
        }
    }

    private void ChangePositionPlayer(int index)
    {
        if (index < 0 || index >= _slotDataList.Count)
        {
            Debug.LogError("Invalid slot index.");
            return;
        }
        foreach (SlotData slotData in _slotDataList)
        {
            if (slotData.IsSelected && slotData.PlayerData.PlayerId.Equals(GameProfile.Instance.GetPlayerData().PlayerId))
            {
                slotData.ExitPosition();
            }
        }
        _slotDataList[index].EnterPosition();
    }

    public void ChangePosition1()
    {
        ChangePositionPlayer(0);
    }
    public void ChangePosition2()
    {
        ChangePositionPlayer(1);
    }
    public void ChangePosition3()
    {
        ChangePositionPlayer(2);
    }
    public void ChangePosition4()
    {
        ChangePositionPlayer(3);
    }

    public void ChangePosition5()
    {
        ChangePositionPlayer(4);
    }
    public void ChangePosition6()
    {
        ChangePositionPlayer(5);
    }
    
    private string SanitizeInput(string input)
    {
        return System.Text.RegularExpressions.Regex.Replace(input, @"[^\w\.\-@]", "").Trim();
    }
}
