using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    private static GateManager _instance;
    public static GateManager Instance => _instance;

    private float _Timer;
    private bool _isGateOpen = false;
    private float _gateOpenDuration = 30f; // Duration for which the gate remains open
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this.GetComponent<GateManager>();
        }
        else if (_instance.GetInstanceID() != this.GetComponent<GateManager>().GetInstanceID())
        {
            Destroy(this.GetComponent<GateManager>());
        }
    }

    public void Init()
    {
        this._Timer = _gateOpenDuration;
        this._isGateOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGateOpen)
        {
            _Timer -= Time.deltaTime;
        }
    }

    public void EnterGate(PlayerController player)
    {
        if (_isGateOpen)
        {
            if (_Timer <= 0)
            {
                // Logic for entering the gate
                Debug.Log("Player has entered the gate.");
                GameManager.Instance.Winner(player);
                return;
            }
        }
        else
        {
            Debug.Log("Gate is closed. Cannot enter.");
        }
    }

    public void AccessKeyRequired(PlayerController player)
    {
        if (_isGateOpen)
        {
            return;
        }
        GameManager.Instance.AccessKey(player);
        Debug.Log("Access key is required to open the gate.");
    }
    
    public void OpenGate()
    {
        _isGateOpen = true;
        Debug.Log("Gate is now open.");
    }
}
