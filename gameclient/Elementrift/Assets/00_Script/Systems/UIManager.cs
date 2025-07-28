using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;

    [Header("Scene UI mapping")]
    public List<SceneUIEntry> sceneUIEntries;

    private Dictionary<string, GameObject> sceneUIMap;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeUIMap();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (_instance.GetInstanceID() != this.GetComponent<UIManager>().GetInstanceID())
        {
            Destroy(this.GetComponent<UIManager>());
        }
    }

    private void InitializeUIMap()
    {
        sceneUIMap = new Dictionary<string, GameObject>();
        foreach (var entry in sceneUIEntries)
        {
            sceneUIMap.Add(entry.sceneName, entry.uiRoot);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (var kvp in sceneUIMap)
        {
            kvp.Value.SetActive(kvp.Key == scene.name); // Chỉ bật UI của Scene hiện tại
        }
    }
}

[System.Serializable]
public class SceneUIEntry
{
    public string sceneName;
    public GameObject uiRoot;
}
