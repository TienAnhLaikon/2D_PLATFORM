using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class FlyingEnemy : MonoBehaviour
{
    // Các kẻ địch bay do không thể xử lý check chạm tường hay chạm đất để tuần tra nên ta sẽ tạo
    // 1 danh sách các Waypoint có sẵn để kẻ địch này tuân theo 
    public List<Transform> waypoints;
    public float waypointReachedDistance = 0.1f;
    // thuộc tính chung cho tất cả kẻ địch

    // animation và cơ thể vật lý
    public Rigidbody2D enemyRigidbody;
    public Animator animator;
    public DetectionZone attackZone;
    public DetectionZone detectionZone;
    public Transform playerTransform;

    private Damageable damageable;

    Transform nextWaypoint;
    int waypointNum = 0;
    // xử lý kháng Stun để không bị stun liên tục khi player tấn công
    public int stunThreshold = 1; // số lần kẻ địch có thể bị làm choáng trước khi kháng choáng
    public int currentStunCount = 0;
    public bool isStunImmune = false;
    public float stunImmunityTime = 1.0f; // Thời gian kháng choáng sau khi bị choáng
    public float stunImmunityTimer = 0f;
    //[SerializeField] protected GameObject groundCheck;
    //[SerializeField] protected LayerMask groundLayer;
    //[SerializeField] public bool isGrounded;

    //[SerializeField] protected GameObject wallCheck;
    //[SerializeField] protected LayerMask wallLayer;
    //[SerializeField] public bool isOnWall;

    // thuộc tính
    [SerializeField] public float flightSpeed;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float chaseSpeed;
    public float stopDistance = 0.5f;  // Khoảng cách tối thiểu để dừng lại khi gần Player

    [SerializeField] public Vector2 moveDirection;

    [SerializeField] public bool isFacingRight = true;
    [SerializeField] public bool isMoving;
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
    public EnemyStateMachine enemyStateMachine { set; get; }
    public FlyingEyeFlightState flyingEyeFlightState { get; set; }
    public FlyingEyePatrolState flyingEyePatrolState { get; set; }
    public FlyingEyeAttackState flyingEyeAttackState { get; set; }
    public FlyingEyeHitState flyingEyeHitState { get; set; }
    public FlyingEyeDeathState flyingEyeDeathState { get; set; }
    public FlyingEyeChaseState flyingEyeChaseState { get; set; }
    #endregion

    // Start is called before the first frame update

    protected void Awake()
    {
        damageable = GetComponent<Damageable>();
        // Đăng ký sự kiện OnDeath của Damageable
        damageable.OnDeath += HandleDeath;
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
        nextWaypoint = waypoints[waypointNum];
    }

    // Update is called once per frame
    protected void Update()
    {
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

    public virtual void FlipDirection()
    {
        transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        isFacingRight = !isFacingRight;


    }
    public void CheckMovementDirection()
    {
        // Kiểm tra nếu moveDirection.x đủ lớn để flip
        float direction = Mathf.Sign(moveDirection.x); // -1 hoặc 1 (sử dụng Mathf.Sign để có được hướng)

        if (isFacingRight && direction == -1)
            FlipDirection();  // Nếu đang nhìn phải và cần quay trái
        else if (!isFacingRight && direction == 1)
            FlipDirection();  // Nếu đang nhìn trái và cần quay phải

    }

    public virtual void Flight()
    {
        // Tính toán hướng di chuyển đến waypoint tiếp theo
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        // Chỉ cập nhật moveDirection.x nếu khoảng cách theo trục x lớn hơn một ngưỡng nhất định (ví dụ 0.1f)
        if (Mathf.Abs(directionToWaypoint.x) > 0.1f)
        {
            moveDirection.x = Mathf.Sign(directionToWaypoint.x);
        }
        else
        {
            moveDirection.x = 0; // Đặt về 0 để không gây ra chuyển động dọc không cần thiết
        }

        // Chỉ cập nhật moveDirection.y nếu khoảng cách theo trục y lớn hơn một ngưỡng nhất định (ví dụ 0.1f)
        if (Mathf.Abs(directionToWaypoint.y) > 0.1f)
        {
            moveDirection.y = Mathf.Sign(directionToWaypoint.y);
        }
        else
        {
            moveDirection.y = 0; // Đặt về 0 để không gây ra chuyển động dọc không cần thiết
        }

        // Di chuyển con quái theo hướng đã tính toán và tốc độ bay
        enemyRigidbody.velocity = new Vector2(moveDirection.x * flightSpeed, moveDirection.y * flightSpeed);

        // Kiểm tra xem đã đến gần waypoint hay chưa
        float distance = Vector2.Distance(transform.position, nextWaypoint.position);
        if (distance <= waypointReachedDistance)
        {
            // Chuyển sang waypoint tiếp theo
            waypointNum++;
            if (waypointNum >= waypoints.Count)
            {
                // Quay lại waypoint đầu tiên nếu hết danh sách waypoint
                waypointNum = 0;
            }
            nextWaypoint = waypoints[waypointNum];
        }

        // Cập nhật hướng (flip) nếu cần
        CheckMovementDirection();

        //Debug.Log("Flight in progress, direction: " + moveDirection);
    }
    public virtual void Chase()
    {
        // Tính khoảng cách giữa kẻ địch và Player trên cả trục x và y
        Vector2 distanceToPlayer = new Vector2(
            Mathf.Abs(playerTransform.position.x - transform.position.x),
            Mathf.Abs(playerTransform.position.y - transform.position.y)
        );

        // Nếu khoảng cách lớn hơn stopDistance, kẻ địch sẽ tiếp tục đuổi theo
        if (distanceToPlayer.x > stopDistance || distanceToPlayer.y > stopDistance)
        {
            // Tính toán hướng di chuyển cả trên trục x và trục y
            moveDirection = new Vector2(
                (playerTransform.position.x - transform.position.x),
                (playerTransform.position.y - transform.position.y)
            ).normalized;

            // Di chuyển kẻ địch về phía Player cả trên trục x và trục y
            enemyRigidbody.velocity = new Vector2(moveDirection.x * chaseSpeed, moveDirection.y * chaseSpeed);
        }
        else
        {
            // Nếu đã đến gần Player, dừng di chuyển
            enemyRigidbody.velocity = Vector2.zero;
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
        enemyStateMachine.ChangeState(flyingEyeDeathState);
    }
    public void OnHit(int damage)
    {
        Debug.Log("Change State");
        // Kiểm tra nếu kẻ địch đang trong trạng thái AttackState
        if (enemyStateMachine.currentState == flyingEyeAttackState && isAttackingAnimation)
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
            if (enemyStateMachine.currentState != flyingEyeHitState)
                enemyStateMachine.ChangeState(flyingEyeHitState);
        }
        else
        {
            // Nếu kẻ địch đã bị choáng đủ số lần, kích hoạt kháng choáng
            isStunImmune = true;
            stunImmunityTimer = stunImmunityTime;
            Debug.Log("Enemy has become stun immune.");
        }
    }
    public void OnHitAnimationBegin()
    {
        isHitComplete = false;
        Debug.Log("false");
    }
    public void OnHitAnimationEnd()
    {
        isHitComplete = true;
        Debug.Log("true");

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
