using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    private string _playerId;
    private string _username;
    private string _password;
    private string _fullName;
    public UserData(string username, string password, string fullName)
    {
        _username = username;
        _password = password;
        _fullName = fullName;
    }

    public string Username => _username;
    public string Password => _password;
    public string FullName => _fullName;

    public string PlayerId
    {
        get => _playerId;
        set => _playerId = value;
    }
}
