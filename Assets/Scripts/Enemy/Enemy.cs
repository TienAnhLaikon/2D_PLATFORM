using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Enemy : MonoBehaviour
{
    // thuộc tính chung cho tất cả kẻ địch

    // animation và cơ thể vật lý
    public Rigidbody2D enemyRigidbody;
    public Animator animator;
    public DetectionZone attackZone;
    public DetectionZone detectionZone;
    public Transform playerTransform;

    private Damageable damageable;

    // xử lý kháng Stun để không bị stun liên tục khi player tấn công
    public int stunThreshold = 2; // số lần kẻ địch có thể bị làm choáng trước khi kháng choáng
    public int currentStunCount = 0;
    public bool isStunImmune = false;
    public float stunImmunityTime = 3.0f; // Thời gian kháng choáng sau khi bị choáng
    public float stunImmunityTimer = 0f;
    [SerializeField] protected GameObject groundCheck;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] public bool isGrounded;
    public float circleRadius;

    [SerializeField] protected GameObject wallCheck;
    [SerializeField] protected LayerMask wallLayer;
    [SerializeField] public bool isOnWall;

    // thuộc tính
    [SerializeField] protected float health;
    [SerializeField] public float moveSpeed;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float damage;
    [SerializeField] protected float chaseSpeed;
    public float stopDistance = 0.5f;  // Khoảng cách tối thiểu để dừng lại khi gần Player

    [SerializeField] public Vector2 moveDirection;

    [SerializeField] public bool isFacingRight = true;
    [SerializeField] public bool isMoving;

    public float attackDuration;
    public Vector3 attackDirection;
    public float attackDistance; // minium distance for attack
    public bool isAttackComplete = false;
    public bool isAttackingAnimation = false;
    public float timer; // timer for cooldown between attacks;
    public float distance; // store the distance beetween enemy and the player

    public bool isPlayerInAttackRange = false;
    public bool isPlayerInDetectionRange = false;

    public bool isHitComplete = false;
    #region States Variables

    #endregion
    public EnemyStateMachine enemyStateMachine { set; get; }
    public EnemyIdleState idleState { get; set; }
    public EnemyPatrolState patrolState { get; set; }
    public EnemyAttackState attackState { get; set; }
    public EnemyDeathState deathState { get; set; }
    public EnemyChaseState chaseState { get; set; }
    public EnemyHitState hitState { get; set; }
    // Start is called before the first frame update

    protected void Awake()
    {
        damageable = GetComponent<Damageable>();
        // Đăng ký sự kiện OnDeath của Damageable
        damageable.OnDeath += HandleDeath;
        enemyStateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this, enemyStateMachine);
        patrolState = new EnemyPatrolState(this, enemyStateMachine);
        attackState = new EnemyAttackState(this, enemyStateMachine);
        deathState = new EnemyDeathState(this, enemyStateMachine);
        hitState = new EnemyHitState(this, enemyStateMachine);
        chaseState = new EnemyChaseState(this, enemyStateMachine, playerTransform);
        
        GameObject player = GameObject.Find("Player");

        // Nếu tìm thấy, lấy component CameraManager
        if (player != null)
        {
            playerTransform = player.GetComponent<Transform>();
        }
        else
        {
            Debug.LogWarning("player not found! Make sure it is named 'player'.");
        }
    }
    protected void Start()
    {
        enemyStateMachine.Initialize(patrolState);
        SetRandomMoveDirectionLeftOrRight();
        isGrounded = true;
        if (moveDirection.x > 0 && !isFacingRight)
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            isFacingRight = !isFacingRight;
        }
        else if (moveDirection.x < 0 && isFacingRight)
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            isFacingRight = !isFacingRight;
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        enemyStateMachine.currentState.Update();
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, circleRadius, groundLayer);
        isOnWall = Physics2D.OverlapCircle(wallCheck.transform.position, circleRadius, wallLayer);

        isPlayerInAttackRange = attackZone.detectedColliders.Count > 0;
        isPlayerInDetectionRange = detectionZone.detectedColliders.Count > 0;

        if (isStunImmune)
        {
            stunImmunityTimer -= Time.deltaTime;
            if (stunImmunityTimer <= 0)
            {
                isStunImmune = false;
                currentStunCount = 0;  // Reset số lần bị choáng sau khi hết thời gian kháng
            }
        }
    }
    protected void FixedUpdate()
    {
        enemyStateMachine.currentState.FixedUpdate();
    }

    public virtual void FlipDirection()
    {
        transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        isFacingRight = !isFacingRight;
        // cái này để xử lý trường hợp nếu con quái đi đến cuối vách của tile thì đổi hướng di chuyển để tránh rơi xuống dưới
        if (!isGrounded || isOnWall) moveDirection.x = -moveDirection.x;


    }
    public void CheckMovementDirection()
    {
        if (isFacingRight && moveDirection.x == -1) FlipDirection();
        else if (!isFacingRight && moveDirection.x == 1) FlipDirection();

    }

    public virtual void Move()
    {

    }
    public virtual void Chase()
    {
        // Tính khoảng cách giữa kẻ địch và Player trên trục x
        float distanceToPlayerX = Mathf.Abs(playerTransform.position.x - transform.position.x);

        // Nếu khoảng cách lớn hơn stopDistance, kẻ địch sẽ tiếp tục đuổi theo
        if (distanceToPlayerX > stopDistance)
        {
            // Tính toán hướng di chuyển chỉ trên trục x
            moveDirection = new Vector2((playerTransform.position.x - transform.position.x), 0).normalized;

            // Di chuyển kẻ địch về phía Player chỉ trên trục x
            enemyRigidbody.velocity = new Vector2(moveDirection.x * chaseSpeed, enemyRigidbody.velocity.y);
        }
        else
        {
            // Nếu đã đến gần Player, dừng di chuyển
            enemyRigidbody.velocity = new Vector2(0, enemyRigidbody.velocity.y);
        }
    }
    public virtual void Attack()
    {

    }
    public void Die()
    {

    }
    public virtual void Patrol()
    {
        enemyRigidbody.velocity = Vector2.right * moveSpeed * moveDirection;
    }
    public void TakeDamage(float damageAmount)
    {

    }
    public void SetRandomMoveDirectionLeftOrRight()
    {
        // Random số 1 hoặc -1
        int direction = Random.Range(0, 2) * 2 - 1; // Random(0, 2) trả về 0 hoặc 1, sau đó nhân 2 rồi trừ 1 để thành -1 hoặc 1
        // Đặt giá trị direction cho moveDirection.x
        moveDirection = new Vector2(direction, 0);  // Di chuyển theo hướng ngẫu nhiên (trái hoặc phải)
    }
    public void OnAttackEnd()
    {
        isAttackComplete = true;
        isAttackingAnimation = false;
    }
    public void OnAttackBegin()
    {
        isAttackComplete = false;
        isAttackingAnimation = true;
    }
    private void HandleDeath()
    {
        // Khi chết, chuyển sang DeathState
        enemyStateMachine.ChangeState(deathState);
    }
    public void OnHit(int damage)
    {
        //Debug.Log("Change State");
        // Kiểm tra nếu kẻ địch đang trong trạng thái AttackState
        if (enemyStateMachine.currentState == attackState && isAttackingAnimation)
        {
            // Nếu kẻ địch đang tấn công, chỉ giảm máu mà không chuyển sang HitState
            return;
        }
        // Nếu kẻ địch chưa bị choáng quá số lần cho phép, thì choáng tiếp
        // kiểm tra xem nếu kẻ địch đang kháng choáng
        if (isStunImmune)
        {
            return;
        }
        // Nếu kẻ địch chưa bị choáng quá số lần cho phép, thì choáng tiếp
        if (currentStunCount < stunThreshold)
        {
            currentStunCount++;
            if (enemyStateMachine.currentState != hitState)
                enemyStateMachine.ChangeState(hitState);
        }
        else
        {
            // Nếu kẻ địch đã bị choáng đủ số lần, kích hoạt kháng choáng
            isStunImmune = true;
            stunImmunityTimer = stunImmunityTime;
            //Debug.Log("Enemy has become stun immune.");
        }
    }
    public void OnHitAnimationBegin()
    {
        isHitComplete = false;
        //Debug.Log("False");
    }
    public void OnHitAnimationEnd()
    {
        isHitComplete = true;
        //Debug.Log("True");

    }
    public void setCurrentAttackDirection(string direction)
    {
        if (direction == "up")
        {
            attackDirection = Vector3.up;
        }
        else if (direction == "down")
        {
            attackDirection = Vector3.down;
        }
        else if (direction == "right")
        {
            if (isFacingRight)
                attackDirection = Vector3.right;
            else attackDirection = Vector3.left;
        }
        else if (direction == "left")
        {
            if (isFacingRight)
                attackDirection = Vector3.left;
            else attackDirection = Vector3.right;
        }
    }
}
