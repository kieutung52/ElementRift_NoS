using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour, IMonster, IGetHit
{
    [SerializeField] private string _monsterName;
    [SerializeField] private float _health;
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _speed;
    [SerializeField] private ItemBase _lootItem;
    [SerializeField] private float _attackRange = 50f;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private float _attackCooldown = 1.5f;
    [SerializeField] protected Animator _animator;
    [SerializeField] private Vector3 _spawnPosition;
    [SerializeField] private Rigidbody _rigidbody;

    private IMonsterState _currentState;
    [SerializeField] private PlayerController _targetPlayer;
    private float _limitDistance = 300f;
    private float _attackTimer;
    [SerializeField] private Vector3 _targetPosition;
    private Vector3 _positionItem;

    void Awake()
    {
        this._rigidbody = this.GetComponent<Rigidbody>();
        this._targetPlayer = null;
    }

    void Start()
    {
        changeState(new MIdleState());
    }

    public void Init()
    {
        this.transform.position = this._spawnPosition;
        this._targetPosition = this._spawnPosition;
        this._attackTimer = 0;
        this._positionItem = this._spawnPosition;
    }

    void Update()
    {
        this._currentState?.UpdateState(this);
        this._spawnPosition.y = this.transform.position.y;

        if (_attackTimer > 0)
        {
            _attackTimer -= Time.deltaTime;
        }

        HandleMonsterLogic();
    }

    void FixedUpdate()
    {
        if ((this._currentState is MWalkState) || (this._currentState is MIdleState))
        {
            MoveTowards();
        }
        else
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }

    private bool IsObstacleBetween()
    {
        if (_targetPlayer == null) return false;
        Vector3 dir = (_targetPlayer.transform.position - transform.position).normalized;
        float dist = Vector3.Distance(transform.position, _targetPlayer.transform.position);
        if (Physics.Raycast(transform.position + new Vector3(0,10,0), dir, out RaycastHit hit, dist, _obstacleMask))
        {
            return true;
        }
        return false;
    }

    private bool IsTooFarFromSpawn()
    {
        return Vector3.Distance(transform.position, _spawnPosition) > _limitDistance;
    }

    private void HandleMonsterLogic()
    {
        // Kiểm tra nên từ bỏ mục tiêu hiện tại không
        if ((_targetPlayer != null) && (!_targetPlayer.IsPlayerAlive() || IsObstacleBetween() || IsTooFarFromSpawn()))
        {
            _targetPlayer = null;
            _targetPosition = _spawnPosition;
            if (this.transform.position != _spawnPosition)
            {
                changeState(new MWalkState());
            }
        }

        // Nếu không có mục tiêu, quay về vị trí spawn
        if (_targetPlayer == null)
        {
            _targetPosition = _spawnPosition;

            // Kiểm tra đã về tới spawn chưa
            if (Vector3.Distance(transform.position, _spawnPosition) <= 0.2f)
            {
                // Nếu đang ở spawn thì chuyển sang trạng thái idle
                if (!(_currentState is MIdleState))
                {
                    changeState(new MIdleState());
                }
            }
            else
            {
                // Nếu chưa về tới spawn và đang không ở trạng thái walk thì chuyển sang walk
                if (!(_currentState is MWalkState))
                {
                    changeState(new MWalkState());
                }
            }
        }
        else
        {
            // Có mục tiêu, di chuyển đến mục tiêu
            _targetPosition = _targetPlayer.transform.position;
            float distanceToPlayer = Vector3.Distance(transform.position, _targetPlayer.transform.position);

            // Nếu trong tầm tấn công thì tấn công
            if (distanceToPlayer <= _attackRange)
            {
                AttackPlayer(_targetPlayer);
            }
            else
            {
                // Nếu không trong tầm tấn công và đang không ở trạng thái walk thì chuyển sang walk
                if (!(_currentState is MWalkState))
                {
                    changeState(new MWalkState());
                }
            }
        }

        RotateTowards(_targetPosition);
    }

    private void MoveTowards()
    {
        if (Vector3.Distance(transform.position, _targetPosition) < 0.2f)
        {
            _rigidbody.velocity = Vector3.zero;
            changeState(new MIdleState());
            return;
        }

        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        moveDirection.y = 0;
        _rigidbody.velocity = moveDirection * _speed;
    }

    private void RotateTowards(Vector3 target)
    {

        if (Vector3.Distance(transform.position, target) < 0.1f) return;

        Vector3 direction = (target - transform.position).normalized;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _rigidbody.MoveRotation(Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 5f
            ));
        }
    }

    public void changeState(IMonsterState newState)
    {
        Debug.Log($"{newState.GetType()} state entered for {_monsterName}");
        if (_currentState?.GetType() == newState.GetType()) return;
        _currentState?.ExitState(this);
        _currentState = newState;
        _currentState.EnterState(this);
    }

    public void AttackPlayer(PlayerController target)
    {
        if (_attackTimer > 0) return;

        changeState(new MAttackState());

        IGetHit hitComponent = target.gameObject.GetComponent<IGetHit>();
        hitComponent?.TakeDamage(null, _attackDamage);

        _attackTimer = _attackCooldown;
    }

    public void Die()
    {
        changeState(new MDeadState());
        Debug.Log($"{_monsterName} has died.");
        Destroy(this.gameObject, 2f);
        if (_lootItem != null)
        {
            GameObject loot = Instantiate(_lootItem.gameObject, this._positionItem, Quaternion.identity);
            loot.GetComponent<ItemBase>().Init(this._positionItem);
            MonsterManager.Instance.AddGameObjKey(loot);
        }
    }

    public Animator GetAnimator() => _animator;
    public Vector3 GetSpawnPosition() => _spawnPosition;

    public void TakeDamage(PlayerData caster, float dmg)
    {
        this._health -= dmg;
        if (_health <= 0)
        {
            Die();
            return; // Dừng hàm nếu đã chết
        }

        // Nếu đã có mục tiêu và mục tiêu đó chính là người tấn công thì không cần tìm lại
        if (_targetPlayer != null && _targetPlayer.GetPlayerData().PlayerId.Equals(caster?.PlayerId))
        {
            return;
        }

        PlayerController attacker = null;
        // Ưu tiên tìm đúng người chơi đã tấn công mình
        if (caster != null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 100f, _playerLayer);
            foreach (Collider collider in colliders)
            {
                PlayerController player = collider.GetComponentInParent<PlayerController>();
                if (player != null && player.IsPlayerAlive() && player.GetPlayerData().PlayerId.Equals(caster.PlayerId))
                {
                    attacker = player;
                    break;
                }
            }
        }

        // Nếu không tìm thấy caster cụ thể (hoặc caster là null), tìm player gần nhất
        if (attacker == null)
        {
            // Bạn có thể thêm logic tìm player gần nhất ở đây nếu muốn
            // Hiện tại, code của bạn sẽ lấy player đầu tiên tìm thấy
            Collider[] colliders = Physics.OverlapSphere(transform.position, 100f, _playerLayer);
            foreach (Collider collider in colliders)
            {
                PlayerController player = collider.GetComponentInParent<PlayerController>();
                if (player != null && player.IsPlayerAlive())
                {
                    attacker = player;
                    break;
                }
            }
        }

        // Nếu tìm được kẻ tấn công
        if (attacker != null)
        {
            this._targetPlayer = attacker;
            // Bắt buộc monster phải hành động ngay lập tức!
            // Nếu ở ngoài tầm đánh -> đuổi theo. Nếu trong tầm -> logic trong HandleMonsterLogic sẽ xử lý tấn công.
            if (Vector3.Distance(transform.position, attacker.transform.position) > _attackRange)
            {
                changeState(new MWalkState());
            }
        }
    }

    public void Heal(float healAmount)
    {
        this._health += healAmount;
        if (this._health > 700)
        {
            this._health = 700;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 100f);

        // Gizmos.color = Color.blue;
        // Vector3 dir = (_targetPlayer.transform.position - transform.position).normalized;
        // Gizmos.DrawRay(transform.position, dir * Vector3.Distance(transform.position, _targetPlayer.transform.position));
    }
}
