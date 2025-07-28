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
    [SerializeField] private RectTransform _healthFillImage;
    [SerializeField] private RectTransform _manaFillImage;
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

}
