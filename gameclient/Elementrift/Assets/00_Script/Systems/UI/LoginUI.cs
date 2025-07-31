using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginUI : MonoBehaviour
{
    [SerializeField] private GameObject _loginPanel;
    [SerializeField] private GameObject _registerPanel;
    [SerializeField] private TextMeshProUGUI _submitTitle;
    [SerializeField] private TextMeshProUGUI _togglePanelTitle;
    [SerializeField] private TextMeshProUGUI _UsernameLogin;
    [SerializeField] private TextMeshProUGUI _PasswordLogin;

    [SerializeField] private TextMeshProUGUI _UsernameRegister;
    [SerializeField] private TextMeshProUGUI _PasswordRegister;
    [SerializeField] private TextMeshProUGUI _FullNameRegister;
    private List<UserData> _userDataList = new List<UserData>();

    void Start()
    {
        this.Init();
    }

    void Init()
    {
        Debug.Log("Initializing LoginUI...");
        _loginPanel.SetActive(false);
        this.TogglePanel();

        // Pre-populate with a test user
        UserData testUser = new UserData("testuser", "123456", "Test User");
        testUser.PlayerId = System.Guid.NewGuid().ToString();
        _userDataList.Add(testUser);
        Debug.Log($"Test user added: {testUser.Username}, Player ID: {testUser.PlayerId}");
    }
    public void SubmitForm()
    {
        if (_loginPanel.activeSelf)
        {
            string username = SanitizeInput(_UsernameLogin.text);
            string password = SanitizeInput(_PasswordLogin.text);

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Debug.LogError("Username or Password cannot be empty.");
                return;
            }
            this.Login(username, password);
        }
        else if (_registerPanel.activeSelf)
        {
            string username = SanitizeInput(_UsernameRegister.text);
            string password = SanitizeInput(_PasswordRegister.text);
            string fullName = SanitizeInput(_FullNameRegister.text);

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fullName))
            {
                Debug.LogError("Username, Password, or Full Name cannot be empty.");
                return;
            }
            this.Register(username, password, fullName);
        }
    }

    public void TogglePanel()
    {
        if (_loginPanel.activeSelf)
        {
            _loginPanel.SetActive(false);
            _registerPanel.SetActive(true);
            _submitTitle.text = "Register";
            _togglePanelTitle.text = "Already have an account? Login";
        }
        else
        {
            _loginPanel.SetActive(true);
            _registerPanel.SetActive(false);
            _submitTitle.text = "Login";
            _togglePanelTitle.text = "Don't have an account? Register";
        }
    }

    private void Login(string username, string password)
    {
        Debug.Log($"Attempting to login with Username: {username}, Password: {password}");
        foreach (UserData user in _userDataList)
        {
            Debug.Log($"Checking user: {user.Username}");
            if (user.Username.Equals(username) && user.Password.Equals(password))
            {
                Debug.Log($"Login successful for user: {username}");
                GameProfile.Instance.Init(new PlayerData(user.Username, user.PlayerId, null));
                // GameProfile.Instance.DebugPrintProfile();
                GameProfile.Instance.LoadHomeScene();
                return;
            }
        }
    }

    private void Register(string username, string password, string fullName)
    {
        foreach (UserData user in _userDataList)
        {
            if (user.Username.Equals(username))
            {
                Debug.LogError($"Username {username} is already taken.");
                return;
            }
        }

        UserData newUser = new UserData(username, password, fullName);
        newUser.PlayerId = System.Guid.NewGuid().ToString();
        this._userDataList.Add(newUser);
        this.TogglePanel();
    }

    private string SanitizeInput(string input)
    {
        return System.Text.RegularExpressions.Regex.Replace(input, @"[^\w\.\-@]", "").Trim();
    }
}
