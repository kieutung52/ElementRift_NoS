using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    private static SceneChange _instance;
    public static SceneChange Instance => _instance;

    private string _currentSceneName;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance.GetInstanceID() != this.GetComponent<SceneChange>().GetInstanceID())
        {
            Destroy(this.GetComponent<SceneChange>());
        }
    }
    private void GoToNextSceneByName()
    {
        // Kiểm tra xem tên scene có rỗng không
        if (string.IsNullOrEmpty(_currentSceneName))
        {
            Debug.LogError("Tên scene mục tiêu không được để trống!");
            return;
        }

        // Chuyển scene bằng tên
        SceneManager.LoadScene(_currentSceneName);

        Debug.Log("Đang chuyển đến scene: " + _currentSceneName);
    }

    public void SetCurrentSceneName(string sceneName)
    {
        _currentSceneName = sceneName;
        Debug.Log("Đã đặt tên scene hiện tại: " + _currentSceneName);
        this.GoToNextSceneByName();
    }
}
