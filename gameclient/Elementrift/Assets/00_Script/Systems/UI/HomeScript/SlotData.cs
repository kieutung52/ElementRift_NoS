using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotData : MonoBehaviour
{
    private PlayerData _playerData;
    private PlayerData _enemyPlayerData = new PlayerData("Enemy", "EnemyId", "EnemyCharacterId");
    private bool _isSelected = false;
    [SerializeField] private bool _isTeamA = true;
    [SerializeField] private Vector3 _spawnPoint;
    [SerializeField] private GameObject _Button;
    [SerializeField] private Image _Avatar;

    void Awake()
    {
        _Button.SetActive(true);
        _Avatar.gameObject.SetActive(false);
    }
    public void EnterPosition()
    {
        if (_isSelected)
        {
            Debug.LogWarning("This position is already selected.");
            return;
        }
        this._playerData = GameProfile.Instance.GetPlayerData();
        // Logic to handle the selection of this slot
        _isSelected = true;
        _Button.SetActive(false);
        _Avatar.sprite = AvataManager.Instance.GetAvatar(this._playerData.PlayerId.Equals(GameProfile.Instance.GetPlayerData().PlayerId));
        _Avatar.gameObject.SetActive(true);
        // Additional logic for selecting the position can be added here
    }

    public void ExitPosition()
    {
        this._playerData = null;
        // Logic to handle the deselection of this slot
        _isSelected = false;
        _Button.SetActive(true);
        _Avatar.gameObject.SetActive(false);
    }

    public void EnemyEnterPosition()
    {
        if (_isSelected)
        {
            Debug.LogWarning("This position is already selected.");
            return;
        }
        _isSelected = true;
        _Button.SetActive(false);
        _Avatar.sprite = AvataManager.Instance.GetAvatar(false);
        _Avatar.gameObject.SetActive(true);
        _playerData = _enemyPlayerData;
    }
    public bool IsSelected => _isSelected;
    public PlayerData PlayerData => _playerData;
    public bool IsTeamA => _isTeamA;
    public Vector3 SpawnPoint => _spawnPoint;
}
