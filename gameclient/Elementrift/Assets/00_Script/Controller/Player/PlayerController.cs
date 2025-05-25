using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class PlayerController : MonoBehaviour, IsGetHit
{
    private string playerId;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float playerHealth = 100f;
    [SerializeField] private float playerMaxHealth = 100f;
    [SerializeField] private float playerArmor = 50f;
    [SerializeField] private float playerMana = 200f;
    [SerializeField] private float playerMaxMana = 200f;
    [SerializeField] private float moveSpeed = CONSTANT.PLAYER_MOVE_SPEED;
    [SerializeField] private float jumpForce = CONSTANT.PLAYER_JUMP_FORCE;
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private float rayLength = 5.5f;
    [SerializeField] private int frontRayCount = 10;
    [SerializeField] private float frontRaySpacing = 1f;
    [SerializeField] private float frontRayLength = 6f;
    [SerializeField] private float climbSpeed = 1.5f;
    [SerializeField] private Transform playerBody;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private RaycastHit hitDown;
    [SerializeField] private PlayerState currentState;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing from the PlayerController GameObject.");
        }
        currentState = PlayerState.Idle;
    }

    public void Init(float health, float armor, float mana, Vector3 position)
    {
        playerTransform.position = position;
        playerHealth = health;
        playerArmor = armor;
        playerMana = mana;
    }

    void Update()
    {
        CheckGround();

        switch (currentState)
        {
            case PlayerState.Idle:
                HandleIdle();
                break;
            case PlayerState.Walking:
                HandleWalking();
                break;
            case PlayerState.Climbing:
                HandleClimbing();
                break;
            case PlayerState.Jumping:
                // Implement jumping if needed
                break;
            case PlayerState.Falling:
                // Implement falling if needed
                break;
            case PlayerState.ClimbingToTop:
                // Optional climb to top transition
                break;
            case PlayerState.Dead:
                break;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log($"Player ID: {playerId}");
            Debug.Log($"Health: {playerHealth}/{playerMaxHealth}, Armor: {playerArmor}, Mana: {playerMana}/{playerMaxMana}");
            Debug.Log($"Is Grounded: {isGrounded}, State: {currentState}");
        }
    }

    void FixedUpdate()
    {
        if (currentState == PlayerState.Walking)
        {
            MovingPlayer();
        }
    }

    void SwitchState(PlayerState newState)
    {
        if (currentState == newState) return;
        Debug.Log($"Switching state: {currentState} -> {newState}");
        currentState = newState;
    }

    void HandleIdle()
    {
        if (IsMoving())
            SwitchState(PlayerState.Walking);
        else if (IsHangingOnWall())
            SwitchState(PlayerState.Climbing);
    }

    void HandleWalking()
    {
        if (!IsMoving())
            SwitchState(PlayerState.Idle);
        else if (IsHangingOnWall())
            SwitchState(PlayerState.Climbing);
    }

    void HandleClimbing()
    {
        rb.useGravity = false;
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 climbDir = transform.forward + Vector3.up;
            rb.velocity = climbDir.normalized * climbSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        if (!IsHangingOnWall())
        {
            rb.useGravity = true;
            SwitchState(PlayerState.Idle);
        }
    }

    bool IsMoving()
    {
        float h = Input.GetAxis(CONSTANT.GETAXIS_HORIZONTAL);
        float v = Input.GetAxis(CONSTANT.GETAXIS_VERTICAL);
        return Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;
    }

    void MovingPlayer()
    {
        float moveHorizontal = Input.GetAxis(CONSTANT.GETAXIS_HORIZONTAL);
        float moveVertical = Input.GetAxis(CONSTANT.GETAXIS_VERTICAL);

        Vector3 inputDir = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
        if (inputDir.magnitude > 0.1f)
        {
            Vector3 move = transform.TransformDirection(inputDir) * moveSpeed;
            move.y = rb.velocity.y;
            rb.velocity = move;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void CheckGround()
    {
        Vector3 origin = playerBody != null ? playerBody.position : transform.position;
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayLength, groundLayer))
        {
            isGrounded = hit.collider.CompareTag(CONSTANT.PLATFORM_TAG);
            hitDown = hit;
        }
        else
        {
            isGrounded = false;
            hitDown = new RaycastHit();
        }
    }

    bool IsHangingOnWall()
    {
        int hitCount = 0;
        Vector3 origin = playerBody.position;
        Vector3 direction = transform.forward;

        for (int i = 0; i < frontRayCount; i++)
        {
            float offsetY = -((frontRayCount - 1) / 2f - i) * frontRaySpacing;
            Vector3 rayOrigin = origin + new Vector3(0, offsetY, 0);
            if (Physics.Raycast(rayOrigin, direction, out RaycastHit hit, frontRayLength, groundLayer))
            {
                if (hit.collider.CompareTag(CONSTANT.PLATFORM_TAG))
                {
                    hitCount++;
                }
            }
        }

        return hitCount >= 7;
    }

    public void TakeDamage(float dmg)
    {
        dmg = Mathf.Max(dmg - playerArmor, 0f);
        playerHealth -= dmg;

        if (playerHealth <= 0)
        {
            Debug.Log("Player is dead.");
            SwitchState(PlayerState.Dead);
        }
    }

    public void Heal(float amount)
    {
        playerHealth = Mathf.Min(playerHealth + amount, playerMaxHealth);
    }

    public void SetPlayerId(string id) => playerId = id;
    public string GetPlayerId() => playerId;

    void OnDrawGizmosSelected()
    {
        Vector3 origin = playerBody != null ? playerBody.position : transform.position;
        Vector3 direction = Vector3.down;
        bool didHit = Physics.Raycast(origin, direction, out hitDown, rayLength);

        Gizmos.color = (didHit && hitDown.collider.CompareTag("Platform")) ? Color.green :
                       (didHit ? Color.yellow : Color.red);
        Gizmos.DrawLine(origin, origin + direction * rayLength);
        if (didHit) Gizmos.DrawSphere(hitDown.point, 0.1f);

        Vector3 forward = playerBody != null ? playerBody.forward : transform.forward;
        for (int i = 0; i < frontRayCount; i++)
        {
            float offsetY = -((frontRayCount - 1) / 2f - i) * frontRaySpacing;
            Vector3 rayOrigin = origin + new Vector3(0, offsetY, 0);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(rayOrigin, rayOrigin + forward * frontRayLength);
        }
    }
}