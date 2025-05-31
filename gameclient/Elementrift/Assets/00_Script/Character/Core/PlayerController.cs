using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
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
    [SerializeField] private float _rayLength = 1f;
    [SerializeField] private int _frontRayCount = 10;
    [SerializeField] private float _frontRaySpacing = 0.8f;
    [SerializeField] private float _frontRayLength = 2.5f;
    [SerializeField] private Transform _playerBody;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _extraGravity = 20f;

    private IPlayerState _currentState;
    private Rigidbody _rb;
    private RaycastHit _hitDown;

    public Animator _animator;
    private Vector3 _inputDirection;

    void Start()
    {
        _moveSpeed = this.GetComponent<Character>().GetCharacterStats()._movementSpeed;
        _jumpForce = this.GetComponent<Character>().GetCharacterStats()._jumpForce;
        _rb = this.GetComponent<Rigidbody>();
        this.changeState(new IdleState());
        // this._animator = this.GetComponentInChildren<Animator>();
    }

    public void Init(Vector3 position)
    {
        _playerTransform.position = position;
        _moveSpeed = this.GetComponent<Character>().GetCharacterStats()._movementSpeed;
        _jumpForce = this.GetComponent<Character>().GetCharacterStats()._jumpForce;
        _rb = this.GetComponent<Rigidbody>();
        this.changeState(new IdleState());
        this._animator = this.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleInput();
        _currentState?.UpdateState(this);
    }

    void FixedUpdate()
    {
        CheckGround();
        ApplyMovement();
        ApplyExtraGravity();
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

    public bool IsMoving()
    {
        float h = Input.GetAxis(CONSTANT.GETAXIS_HORIZONTAL);
        float v = Input.GetAxis(CONSTANT.GETAXIS_VERTICAL);
        return Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;
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
            Vector3 move = transform.TransformDirection(_inputDirection) * _moveSpeed * Time.fixedDeltaTime;
            _rb.MovePosition(_rb.position + move);
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

    public Animator GetAnimator() => _animator;
    public bool IsGrounded() => _isGrounded;
    public Vector3 GetInputDirection() => _inputDirection;
    public Rigidbody GetRigidbody() => _rb;
    public float GetMoveSpeed() => _moveSpeed;
    public void SetMoveSpeed(float speed) => _moveSpeed = speed;
}