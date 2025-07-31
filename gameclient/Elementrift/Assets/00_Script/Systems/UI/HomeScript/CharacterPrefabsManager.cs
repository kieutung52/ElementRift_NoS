using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPrefabsManager : MonoBehaviour
{
    private static CharacterPrefabsManager _instance;
    public static CharacterPrefabsManager Instance => _instance;

    // Thay thế Dictionary bằng List của lớp tùy chỉnh
    [SerializeField] private List<CharacterPrefabEntry> _characterPrefabEntries;

    private Dictionary<string, GameObject> _characterPrefabsRuntime = new Dictionary<string, GameObject>();

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance.GetInstanceID() != this.GetInstanceID()) // Sử dụng GetInstanceID() trên this
        {
            Destroy(gameObject);
            return; // Quan trọng: Đảm bảo không chạy tiếp code khởi tạo
        }

        // Khởi tạo Dictionary từ List đã được điền trong Editor
        foreach (CharacterPrefabEntry entry in _characterPrefabEntries)
        {
            if (!_characterPrefabsRuntime.ContainsKey(entry.characterId))
            {
                _characterPrefabsRuntime.Add(entry.characterId, entry.characterPrefab);
            }
            else
            {
                Debug.LogWarning($"Duplicate Character ID found: {entry.characterId}. Only the first entry will be used.");
            }
        }
    }
    
    public GameObject GetPrefab(string characterId)
    {
        if (_characterPrefabsRuntime.ContainsKey(characterId)) // Sử dụng dictionary runtime
        {
            return _characterPrefabsRuntime[characterId];
        }
        Debug.LogWarning($"Character Prefab with ID '{characterId}' not found.");
        return null;
    }
}

// Lớp tùy chỉnh cần phải có [System.Serializable]
[System.Serializable]
public class CharacterPrefabEntry
{
    public string characterId;
    public GameObject characterPrefab;
}