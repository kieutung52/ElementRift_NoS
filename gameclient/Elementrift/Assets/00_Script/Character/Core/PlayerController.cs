using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _jumpRequested;
    [SerializeField] private float _rayLength = 2f;
    [SerializeField] private int _frontRayCount = 10;
    [SerializeField] private float _frontRaySpacing = 2f;
    [SerializeField] private float _frontRayLength = 2.5f;
    [SerializeField] private Transform _playerBody;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _itemLayer;
    [SerializeField] private float _extraGravity = 20f;

    [SerializeField] private SkillManager _skillManager;

    private IPlayerState _currentState;
    private Rigidbody _rb;
    private RaycastHit _hitDown;

    public Animator _animator;
    private Vector3 _inputDirection;
    private Character _character;
    [SerializeField] private TextMeshPro _playerNameText;
    [SerializeField] private bool _IsOwner = true;
    [SerializeField] private GameObject _cameraObject;

    void Awake()
    {
        _rb = this.GetComponent<Rigidbody>();
        this._character = this.GetComponent<Character>();
    }
    // void Start()
    // {
    //     _moveSpeed = this._character.GetCharacterStats()._movementSpeed;
    //     _jumpForce = this._character.GetCharacterStats()._jumpForce;
    //     this.changeState(new IdleState());
    //     _playerData = new PlayerData("Kieu Tung", "null", "null"); // test data

    //     this._skillManager.Init(_playerData, this._character);
    //     this._playerNameText.SetText(_playerData.PlayerName);
    // }

    public void Init(Vector3 position, PlayerData playerData)
    {
        _playerTransform.position = position;
        _playerData = playerData;
        _moveSpeed = this._character.GetCharacterStats()._movementSpeed;
        _jumpForce = this._character.GetCharacterStats()._jumpForce;
        this.changeState(new IdleState());
        this._skillManager.Init(_playerData, this._character);
        this._playerNameText.SetText(_playerData.PlayerName);
        if (!_IsOwner)
        {
            this._cameraObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!_IsOwner) return;
        this.HandleInput();
        _currentState?.UpdateState(this);
        this.Attack();
        this.SwitchSKill();
        this.AccessKeyRequired();
        this.UpdateUI();
    }

    void FixedUpdate()
    {
        if (!_IsOwner) return;
        CheckGround();
        ApplyMovement();
        ApplyExtraGravity();
    }

    private void UpdateUI()
    {
        BattleUI.Instance.UpdateHealthBar(_character.GetCharacterStats()._health / _character.GetCharacterStats()._maxHealth * 100f);
        BattleUI.Instance.UpdateManaBar(_character.GetCharacterStats()._mana / _character.GetCharacterStats()._maxMana * 100f);
    }
    private void HandleInput()
    {
        float h = Input.GetAxisRaw(CONSTANT.GETAXIS_HORIZONTAL);
        float v = Input.GetAxisRaw(CONSTANT.GETAXIS_VERTICAL);
        _inputDirection = new Vector3(h, 0, v).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _jumpRequested = true;
            this.changeState(new JumpState());
        }
    }

    public void CheckGround()
    {
        Vector3 origin = _playerBody != null ? _playerBody.position : transform.position;
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, _rayLength, _groundLayer))
        {
            _isGrounded = hit.collider.CompareTag(CONSTANT.PLATFORM_TAG);
            _hitDown = hit;
        }
        else
        {
            _isGrounded = false;
            _hitDown = new RaycastHit();
        }
    }
    void OnDrawGizmosSelected()
    {
        Vector3 origin = _playerBody != null ? _playerBody.position : transform.position;
        Vector3 direction = Vector3.down;
        bool didHit = Physics.Raycast(origin, direction, out _hitDown, _rayLength);

        Gizmos.color = (didHit && _hitDown.collider.CompareTag("Platform")) ? Color.green :
                       (didHit ? Color.yellow : Color.red);
        Gizmos.DrawLine(origin, origin + direction * _rayLength);
        if (didHit) Gizmos.DrawSphere(_hitDown.point, 0.1f);

        Vector3 forward = _playerBody != null ? _playerBody.forward : transform.forward;
        for (int i = 0; i < _frontRayCount; i++)
        {
            float offsetY = i * _frontRaySpacing;
            Vector3 rayOrigin = origin + new Vector3(0, offsetY, 0);
            if (i == _frontRayCount - 1)
                Gizmos.color = Color.magenta;
            else
                Gizmos.color = Color.cyan;
            Gizmos.DrawLine(rayOrigin, rayOrigin + forward * _frontRayLength);
        }
    }


    private void ApplyMovement()
    {
        if (_inputDirection.magnitude > 0.1f)
        {
            Vector3 desiredMove = transform.TransformDirection(_inputDirection).normalized;

            if (_isGrounded)
            {
                desiredMove = Vector3.ProjectOnPlane(desiredMove, _hitDown.normal).normalized;
            }

            Vector3 finalVelocity = desiredMove * _moveSpeed;
            finalVelocity.y = _rb.velocity.y;

            _rb.velocity = finalVelocity;
        }
        else
        {
            Vector3 stopVelocity = new Vector3(0, _rb.velocity.y, 0);
            _rb.velocity = stopVelocity;
        }
    }


    private void ApplyExtraGravity()
    {
        if (!_isGrounded && !_jumpRequested)
        {
            _rb.AddForce(Vector3.down * _extraGravity, ForceMode.Force);
        }
    }

    public void PerformJump()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _jumpRequested = false;
    }

    public void changeState(IPlayerState newState)
    {
        _currentState?.ExitState(this);
        _currentState = newState;
        _currentState.EnterState(this);
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _skillManager.UseSkill();
        }
    }

    private void AccessKeyRequired()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, 30f, _itemLayer);
        foreach (Collider col in collider)
        {
            if (col.CompareTag("Key") && Input.GetKeyDown(KeyCode.F))
            {
                KeyItem keyItem = col.GetComponent<KeyItem>();
                if (keyItem != null)
                {
                    keyItem.UseItem(this);
                    return;
                }
            } else if (col.CompareTag("Gate") && Input.GetKeyDown(KeyCode.F))
            {
                GateManager.Instance.EnterGate(this);
                return;
            }
        }

    }

    private void SwitchSKill()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this._skillManager.ChangeSkill(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this._skillManager.ChangeSkill(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            this._skillManager.ChangeSkill(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            this._skillManager.ChangeSkill(4);
        }
    }

    public Animator GetAnimator() => _animator;
    public bool IsGrounded() => _isGrounded;
    public Vector3 GetInputDirection() => _inputDirection;
    public Rigidbody GetRigidbody() => _rb;
    public float GetMoveSpeed() => _moveSpeed;
    public void SetMoveSpeed(float speed) => _moveSpeed = speed;
    public PlayerData GetPlayerData() => _playerData;

    public bool IsPlayerAlive() => this._character.IsAlive();
    public void SetIsOwner(bool isOwner) => _IsOwner = isOwner;
}