using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    private static BattleUI _instance;
    public static BattleUI Instance => _instance;

    [SerializeField] private TextMeshProUGUI _teamAScoreText;
    [SerializeField] private TextMeshProUGUI _teamBScoreText;
    [SerializeField] private TextMeshProUGUI _gameTimerText;
    [SerializeField] private TextMeshProUGUI _notificationText;
    [SerializeField] private GameObject _notificationPanel;
    [SerializeField] private RectTransform _healthFillImage;
    [SerializeField] private RectTransform _manaFillImage;
    private Queue<string> _notificationQueue = new Queue<string>();
    private float _displayDuration = 3f;
    private float _fadeDuration = 0.5f;
    private bool _isDisplayingNotification = false;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this.GetComponent<BattleUI>();
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance.GetInstanceID() != this.GetComponent<BattleUI>().GetInstanceID())
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (_notificationPanel != null)
        {
            _notificationPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Notification Panel is not assigned in NotificationManager!");
        }

        if (_notificationText == null)
        {
            Debug.LogError("Notification Text is not assigned in NotificationManager!");
        }

        UpdateTeamAScore(0);
        UpdateTeamBScore(0);
        UpdateGameTimer("00:00"); // Thời gian ban đầu
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateTeamAScore(int score)
    {
        _teamAScoreText.text = score.ToString();
    }
    public void UpdateTeamBScore(int score)
    {
        _teamBScoreText.text = score.ToString();
    }
    public void UpdateGameTimer(string time)
    {
        _gameTimerText.text = time;
    }

    public void UpdateHealthBar(float healthPercentage)
    {
        Vector2 healthFillSize = _healthFillImage.sizeDelta;
        healthFillSize.x = healthPercentage;
        _healthFillImage.sizeDelta = healthFillSize;
    }

    public void UpdateManaBar(float manaPercentage)
    {
        Vector2 manaFillSize = _manaFillImage.sizeDelta;
        manaFillSize.x = manaPercentage;
        _manaFillImage.sizeDelta = manaFillSize;
    }

    public void EnqueueNotification(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            Debug.LogWarning("Attempted to enqueue an empty notification message.");
            return;
        }
        _notificationQueue.Enqueue(message);
        Debug.Log($"Notification enqueued: '{message}'. Queue size: {_notificationQueue.Count}");

        // Nếu hiện tại không có thông báo nào đang được hiển thị, bắt đầu hiển thị
        if (!_isDisplayingNotification)
        {
            StartCoroutine(ProcessNotificationQueue());
        }
    }

    private IEnumerator ProcessNotificationQueue()
    {
        _isDisplayingNotification = true;

        while (_notificationQueue.Count > 0)
        {
            string currentMessage = _notificationQueue.Dequeue();
            Debug.Log($"Displaying notification: '{currentMessage}'. Remaining in queue: {_notificationQueue.Count}");

            if (_notificationPanel != null) _notificationPanel.SetActive(true);
            if (_notificationText != null) _notificationText.text = currentMessage;

            yield return FadeCanvasGroup(0f, 1f, _fadeDuration);

            yield return new WaitForSeconds(_displayDuration);

            yield return FadeCanvasGroup(1f, 0f, _fadeDuration);

            if (_notificationPanel != null) _notificationPanel.SetActive(false);

            yield return new WaitForSeconds(0.1f); 
        }

        _isDisplayingNotification = false;
        Debug.Log("Notification queue finished processing.");
    }

    private IEnumerator FadeCanvasGroup(float startAlpha, float endAlpha, float duration)
    {
        CanvasGroup canvasGroup = _notificationPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = _notificationPanel.AddComponent<CanvasGroup>();
        }

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }
}
