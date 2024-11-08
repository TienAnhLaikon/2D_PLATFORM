using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Attribute of player")]   
    
    public Rigidbody2D myRigidbody;
    public Animator animator;
    public Damageable damageable;

    [SerializeField] // Hoặc bạn có thể dùng public thay vì SerializeField
    private Vector2 playerVelocity; // Biến theo dõi vận tốc

    public Vector3 attackDirection;
    [Header("Jump")]
    [Range(1,10)]
    public float jumpForce;
    // đây là số lượng ta sẽ nhân lên vào gravity khi nhân vật đang rơi xử lý việc nhân vật khi rơi sẽ nhanh hơn liên tục
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public bool isJumping;
    public bool isFalling;

    [Header("Wall Jump")]
    public bool isWallJumping;
    public float wallJumpForce;
    public Vector2 wallJumpDirection;

    [Header("Movement")]
    public Vector2 moveDirection;
    public float moveSpeed = 4f;
    public float acceleration;
    public float decceleration;
    public float velPower;
    public float frictionAmount;
    public bool isFacingRight = true;
    public bool isMoving;

    [Header("Roll")]
    // các biến xử lý Roll (lộn vòng)
    public float rollSpeed = 10f;
    public bool isRolling;
    public float rollDuration = 0.5f;
    public bool canRoll;
    public float rollCooldown;


    [Header("Slide")]
    // Các biến xử lý Slide (trượt dài)
    public float slideSpeed;
    public bool isSliding;
    public bool canSlide;

    [Header("Wall Slide")]
    public float wallSlideSpeed;
    public bool isWallSliding;
    public bool canWallSlide;
    public float fastWallSlideSpeed = 10f; // Tốc độ trượt nhanh hơn
    public float slowWallSlideSpeed = 2f;  // Tốc độ trượt chậm hơn

    [Header("Dash")]
    public float dashForce;
    public float dashTime;
    public bool isDashing;
    public bool hasDashed; // để xử lý khi rơi chỉ có thể dash được 1 lần
    
    [Header("Heal")]
    // các biến xử lý heal (hồi máu)
    public int healthHeal;
    public bool isHealing;
    public bool canHeal;
    public int estucFlasks; // số lượng bình máu hiện tại
    public GameObject healingEffectPrefab; // prefab của hiệu ứng hồi máu
    private GameObject healingEffectInstance;
    [Header("Attack")]
    public bool isAttacking;
    public int comboStep = 0;
    public int maxComboStep = 2;
    public bool isKeepCombo;
    public float comboTimer = 0f;
    public float comboDuration = 1f;
    [Header("Hit")]
    public Vector2 knockBackForce = Vector2.zero;
    public bool isHitFromRightSide;

    [Header("Collision Info")]
    public LayerMask groundLayer; // Lớp của mặt đất
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public bool isGrounded;

    public Transform wallCheck;
    public float wallCheckDistance;
    public bool isOnWall;

    public Transform ledgeCheck;
    public bool canClimbLedge = false;
    public bool ledgeDetected;
    public Vector2 ledgePosBot;
    public Vector2 ledgePos1;
    public Vector2 ledgePos2;
    public float ledgeClimbXOffset1 = 0;
    public float ledgeClimbYOffset1 = 0;
    public float ledgeClimbXOffset2 = 0;
    public float ledgeClimbYOffset2 = 0;
    public bool isLedge;

    [Header("Stamina System")]
    public float maxStamina = 100f; // Lượng Stamina tối đa
    public float currentStamina; // Lượng Stamina hiện tại
    public float staminaRecoveryRate = 10f; // Tốc độ hồi phục Stamina mỗi giây
    public float staminaWaitTime = 1f; // Thời gian chờ trước khi hồi phục
    private float staminaRecoveryTimer; // Bộ đếm thời gian chờ hồi phục

    [Header("Stamina Cost")]
    public float jumpStaminaCost = 15f;
    public float dashStaminaCost = 20f;
    public float rollStaminaCost = 10f;
    public float attackStaminaCost = 10f;

    #region States Variables
    public PlayerStateMachine playerStateMachine { get; set; }
    public IdleState idleState { get; set; }
    public RunState runState { get; set; }
    public JumpState jumpState { get; set; }
    public FallState fallState { get; set; }   
    public AttackState attackState { get; set; }
    public HitState hitState { get; set; }
    public RollState rollState { get; set; }
    public SlideState slideState { get; set; }
    public LedgeClimbState ledgeClimbState { get; set; }
    public WallSlideState wallSlideState { get; set; }
    public WallJumpState wallJumpState { get; set; }
    public PrayState prayState { get; set; }
    public HealState healState { get; set; }
    public DashState dashState { get; set; }
    public DeathState deathState { get; set; }
    public CrouchState crouchState { get; set; }    
    public CrouchAttackState crouchAttackState { get; set; }
    #endregion
    private void Awake()
    {
        playerStateMachine = new PlayerStateMachine();
        idleState = new IdleState(this, playerStateMachine);
        runState = new RunState(this, playerStateMachine);
        jumpState = new JumpState(this, playerStateMachine);
        fallState = new FallState(this, playerStateMachine);
        attackState = new AttackState(this, playerStateMachine);
        hitState = new HitState(this, playerStateMachine);
        rollState = new RollState(this, playerStateMachine);
        slideState = new SlideState(this, playerStateMachine);
        ledgeClimbState = new LedgeClimbState(this, playerStateMachine);  
        wallSlideState = new WallSlideState(this, playerStateMachine);
        prayState = new PrayState(this, playerStateMachine);
        healState = new HealState(this, playerStateMachine);
        wallJumpState = new WallJumpState(this, playerStateMachine);
        dashState = new DashState(this, playerStateMachine);
        deathState = new DeathState(this, playerStateMachine);
        crouchState = new CrouchState(this,playerStateMachine);
        crouchAttackState = new CrouchAttackState(this, playerStateMachine);

        damageable = GetComponent<Damageable>();
        // Đăng ký sự kiện OnDeath của Damageable
        damageable.OnDeath += HandleDeath;


    }
    void Start()
    {
        playerStateMachine.Initialize(idleState);
        currentStamina = maxStamina; // Đặt Stamina bắt đầu ở mức tối đa
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDashing || !isAttacking)
        {
            GetInput();
        }
        CheckIsPlayerOnGround();
        CheckIsPlayerOnWall();
        CheckIsPlayerTouchLedge();
        CheckLedgeClimb();
        HandleStaminaRecovery(); // Gọi hàm hồi phục Stamina trong Update
        if (isGrounded)
        {
            hasDashed = false;
        }
        playerVelocity = myRigidbody.velocity;
        playerStateMachine.currentState.Update();
    }
    private void FixedUpdate()
    {
        playerStateMachine.currentState.FixedUpdate();

        if (isGrounded && Mathf.Abs(moveDirection.x) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(myRigidbody.velocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(myRigidbody.velocity.x);
            myRigidbody.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }
    void Flip()
    {
        if ((isFacingRight && moveDirection.x < 0) || (!isFacingRight && moveDirection.x > 0)) FlipDirection();
    }
    public void FlipDirection()
    {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        wallCheck.localScale = new Vector3(-wallCheck.localScale.x, wallCheck.localScale.y, wallCheck.localScale.y);
        isFacingRight = !isFacingRight;
    }
    public void GetInput()
    {
        // get our input here

        moveDirection.x = Input.GetAxisRaw("Horizontal"); // Input trục ngang
        moveDirection.y = Input.GetAxisRaw("Vertical");   // Input trục dọc (dùng cho cơ chế leo tường sau này)
         CheckIsPlayerMoving();
    }
    public void CheckMovementDirection()
    {
        if (isFacingRight && moveDirection.x == -1) Flip();
        else if (!isFacingRight && moveDirection.x == 1) Flip();
        // set animation cho di chuyen
        animator.SetFloat("speed", Mathf.Abs(moveDirection.x));
    }
    public void CheckIsPlayerMoving()
    {
        isMoving = moveDirection.x != 0;
    }
    public void PlayerMove()
    {
        #region Run

        // tính toán hướng mà ta muốn di chhuyeenr và vận tốc mong muốn
        float targetSpeed = moveDirection.x * moveSpeed;

        // tính toán sự khác biệt giữa vận tốc hiện tại và vận tốc mong muốn
        /*Giải thích về SpeedDif
            Tưởng tượng ta đang lái một xe ô tô. Ta muốn đi với vận tốc 60km/h
        Nhưng xe hiện tại đang đi 40km/h
        Ví dụ thực tế ở đây là
        Tốc Độ Mong Muốn (Targetspeed): 60km/h
        Tốc Độ Hiện Tại (myRigidbody.velocity.x): 40km/h
        Tính toán sự khác biệt (speedDif)
        Sự khác biệt = tốc độ mong muốn - tốc độ hiện tại
        speedDif = 60 - 40 = 20km/h
        speedDif đây là khoảng cách ta phải tăng tốc lên để đạt được tốc độ mong muốn 
        Nếu không có speedDif ta sẽ không biết phải tăng tốc bao nhiêu.

        Vậy Speed Dif là viết tắt của Speed Difference là biến lưu trữ sự khác biệt giữa tốc độ mong muốn
        và tốc độ hiện tại
        Mục Đích của speedDif

        Tính Toán Sự Khác Biệt Tốc Độ: speedDif cho biết khoảng cách mà tốc độ hiện tại của nhân vật cần điều chỉnh để đạt được tốc độ mong muốn. Nếu speedDif là dương, nhân vật cần gia tốc để tăng tốc độ. Nếu speedDif là âm, nhân vật cần giảm tốc độ.

        Điều Chỉnh Gia Tốc: Biến này giúp xác định lượng lực cần thiết để điều chỉnh tốc độ hiện tại sao cho khớp với tốc độ mong muốn. Từ speedDif, bạn có thể tính toán lượng gia tốc hoặc giảm tốc cần áp dụng để đạt được tốc độ mục tiêu một cách mượt mà.
        Cần Thiết Không?

Cần Thiết: Có speedDif là cần thiết để điều chỉnh tốc độ của nhân vật một cách chính xác. Nếu không có nó, bạn sẽ không thể tính toán sự khác biệt giữa tốc độ hiện tại và tốc độ mong muốn, và do đó không thể áp dụng lực phù hợp để đạt được tốc độ mục tiêu.

Nếu Không Có speedDif: Nếu bạn bỏ qua speedDif, bạn sẽ không biết cần phải gia tốc hoặc giảm tốc bao nhiêu để điều chỉnh tốc độ của nhân vật. Điều này có thể dẫn đến việc nhân vật di chuyển quá nhanh hoặc quá chậm, hoặc không phản hồi đúng cách với đầu vào của người chơi.
        */
        float speedDif = targetSpeed - myRigidbody.velocity.x;

        // thay đổi tốc độ tăng tốc dựa vào tình huống
        // cái này để xử lý khi nhân vật đi đúng hướng nó sẽ dùng ít Lực
        // còn khi nhân vật quay ngoắt đầu lại thì sẽ dùng nhiều lực hơn để tạo mượt mà
        /* GIẢI THÍCH VỀ ACCEL RATE
         Ta đang lái xe ô tô với 2 chế độ lái khác nhau:
        Khi ta tăng tốc và khi ta giảm tốc. Để làm điều này
        ta cần điều chỉnh cách ta nhấn chân ga hoặc phanh

        Ví dụ Thực Tế
        Khi bạn đang tăng tốc:
        Bạn muốn tăng tốc nhanh chóng đến tốc độ mục tiêu, vì vậy bạn cần một chế độ lái mà bạn có thể gia tốc nhanh hơn.
        Khi bạn đang giảm tốc:
        Bạn muốn giảm tốc độ một cách từ từ để dừng lại mượt mà, vì vậy bạn cần một chế độ lái mà bạn giảm tốc chậm hơn.
        Mathf.Abs(targetSpeed) > 0.01f: Đây là điều kiện kiểm tra nếu tốc độ mục tiêu lớn hơn một ngưỡng nhỏ (0.01f). Đây là cách để kiểm tra nếu người chơi đang thực sự di chuyển.
        
        Nếu tốc độ mục tiêu target speed lớn hơn 0.01 tức là người chơi đang di chuyển thì gia tốc sẽ sử dụng
        Nếu tốc độ nhỏ hơn 0.01 tức là không di chuyển hoặc đang dừng thì giảm tốc sẽ sử dụng
        Ví Dụ Thực Tế
Tăng Tốc: Nếu bạn muốn đạt được tốc độ 60 km/h từ 0 km/h, bạn sẽ cần gia tốc nhanh hơn, vì vậy acceleration sẽ được sử dụng.

Giảm Tốc: Nếu bạn muốn giảm từ 60 km/h xuống 0 km/h, bạn sẽ muốn giảm tốc từ từ, vì vậy decceleration sẽ được sử dụng.

        Nói chung Accel Rate để kiểm tra ta đang Gia Tốc hay GIảm tốc
         */
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;

        /*áp dụng gia tốc vào chênh lệch tốc độ, tăng lên một công suất thiết lập để gia tốc tăng theo tốc độ cao hơn cuối cùng nhân với dấu để trả lời hướng
         */
        /*GIẢI THÍCH movement
         1. Bạn đang đứng yên và quyết định đẩy xe trượt để đi tới.
        * targetSpeed: Đây là tốc độ bạn muốn đạt được, ví dụ: 10 km/h.
        * myRigidbody.velocity.x: Đây là tốc độ hiện tại của xe trượt, ví dụ: 0 km/h.
        * speedDif: Là sự khác biệt giữa tốc độ mong muốn và tốc độ hiện tại. Trong trường hợp này, speedDif là 10 km/h - 0 km/h = 10 km/h.
         2. Bạn cần quyết định bao nhiêu lực cần áp dụng để đạt được tốc độ mong muốn.
        * accelRate: Là tốc độ mà bạn gia tăng vận tốc, ví dụ: 5.
        * velPower: Là mức độ ảnh hưởng của gia tốc, ví dụ: 2. Điều này có nghĩa là bạn cần lực gia tăng theo cấp số nhân để cảm nhận tốc độ.
         3. Tính toán mức độ lực cần áp dụng.

        * Mathf.Abs(speedDif): Lấy giá trị tuyệt đối của speedDif, tức là 10.
        * Mathf.Abs(speedDif) * accelRate: Tính toán lực cần thiết, tức là 10 * 5 = 50.
        * Mathf.Pow(..., velPower): Làm cho lực này mạnh hơn theo cấp số nhân, tức là 50^2 = 2500.
        * Mathf.Sign(speedDif): Xác định hướng lực (tăng tốc hay giảm tốc). Trong trường hợp này, hướng là dương (+1) vì bạn muốn gia tăng tốc độ.

         */
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        myRigidbody.AddForce(movement * Vector2.right);
        #endregion



    }
    public void CheckIsPlayerOnGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;
        animator.SetBool("Jump", !isGrounded);
        animator.SetBool("IsGrounded", isGrounded);

    }
    public void CheckIsPlayerTouchLedge()
    {
        // Kiểm tra hướng dựa vào biến isFacingRight
        Vector2 playerDirection = isFacingRight ? Vector2.right : Vector2.left;
        isLedge = Physics2D.Raycast(ledgeCheck.position, playerDirection, wallCheckDistance, groundLayer);
    if(isOnWall && !isLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgePosBot = wallCheck.position;
        }
    else
        {
            ledgeDetected = false;
        }
    }
    public void FinishLedgeClimb()
    {
        canClimbLedge = false;
        transform.position = ledgePos2;
        ledgeDetected = false;
        animator.SetBool("CanClimbLedge", canClimbLedge);
        playerStateMachine.ChangeState(idleState);
    }
    private void CheckLedgeClimb()
    {
        if(ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;
            if (isFacingRight)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance)- ledgeClimbXOffset1,Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance)+ ledgeClimbXOffset2,Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            else
            {
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            playerStateMachine.ChangeState(ledgeClimbState);
        }
        if (canClimbLedge)
        {
            transform.position = ledgePos1;
        }
    }
    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(transform.position- transform.up * groundCheckRadius, boxSize);
        Gizmos.DrawLine(wallCheck.position,new Vector3(wallCheck .position.x + wallCheckDistance, wallCheck.position.y,wallCheck.position.z));
        
    }
    public void CheckIsPlayerOnWall()
    {
        // Kiểm tra hướng dựa vào biến isFacingRight
        Vector2 playerDirection = isFacingRight ? Vector2.right : Vector2.left;
        isOnWall = Physics2D.Raycast(wallCheck.position,playerDirection, wallCheckDistance, groundLayer);

    }
    public void CheckIsPlayerCelling()
    {
    }
    public void Jump()
    {
        // Giữ nguyên vận tốc trục x và chỉ thay đổi vận tốc trục y
        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
    }

    public void Attack()
    {

    }
    public void Roll()
    {
        if(isFacingRight) myRigidbody.velocity = new Vector2(rollSpeed, myRigidbody.velocity.y); 
        else if(!isFacingRight) myRigidbody.velocity = new Vector2(-(rollSpeed), myRigidbody.velocity.y);
    }
    public void Slide()
    {
        if (isFacingRight)  myRigidbody.velocity = new Vector2(slideSpeed, myRigidbody.velocity.y);
        else if (!isFacingRight) myRigidbody.velocity = new Vector2(-(slideSpeed), myRigidbody.velocity.y);
    }
    public void Heal()
    {

    }
    public void WallSlide(float slideSpeed)
    {
        myRigidbody.velocity = new Vector2(0, Mathf.Clamp(myRigidbody.velocity.y, -slideSpeed, 0));

    }
    public void OnAttackBegin()
    {
        myRigidbody.velocity = Vector2.zero;
        UseStamina(attackStaminaCost);

    }
    // Bạn có thể dùng sự kiện animation hoặc thời gian để chuyển lại trạng thái khác
    public void OnAttackEnd()// Hàm được gọi khi animation kết thúc
    {
        if (comboStep < maxComboStep && isKeepCombo)
        {
            //Debug.Log("Keep Combo");
            comboStep++;
            return;

        }
        else if ((comboStep < maxComboStep && !isKeepCombo) || comboStep == 2)
        {
            //Debug.Log("Attack");
            // idle 
            if (!isMoving)
            {
                playerStateMachine.ChangeState(idleState);
            }
            if (isMoving)
            {
                playerStateMachine.ChangeState(runState);
            }
        }
    }
    private void HandleDeath()
    {
        // Khi chết, chuyển sang DeathState
        playerStateMachine.ChangeState(deathState);
    }
    public void OnHit(int damage, Vector2 knockBackForce)
    {
        //Debug.Log("Change State");
       // Debug.Log("On Hit");
        Debug.Log("damage" + damage);
        UIManager.Instance.SetHealth(damageable.CurrentHealth);
        this.knockBackForce = knockBackForce;
    
        playerStateMachine.ChangeState(hitState);
    }
    public void IsHealingComplete()
    {
        isHealing = false;
    }
    public void OnRollBegin()
    {

    }
    private void KnockBack()
    {
        // Kiểm tra hướng knockback dựa trên việc bị tấn công từ bên phải hay bên trái
        float knockBackDirection = isHitFromRightSide ? -knockBackForce.x : knockBackForce.x;
        myRigidbody.velocity = new Vector2(knockBackDirection, myRigidbody.velocity.y + knockBackForce.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isRolling)
        {
            // Kiểm tra xem va chạm có phải là từ kẻ địch hay không
            if (collision.CompareTag("EnemyHitBox") && damageable.IsAlive)
            {
                // Lấy GameObject cha của đối tượng EnemyHitBox (là Skeleton)
                Transform enemyTransform = collision.transform.root; // lấy gốc root (cha lớn nhất)

                // Lấy vị trí của Skeleton
                Vector2 enemyPosition = enemyTransform.position;

                // Gọi KnockBack với vị trí của kẻ địch (Skeleton)
                // Kiểm tra xem nhân vật có bị đánh từ phía bên phải hay bên trái
                if (transform.position.x < enemyPosition.x)
                {
                    // Kẻ địch ở bên phải nhân vật
                    isHitFromRightSide = true;
                }
                else if (transform.position.x > enemyPosition.x)
                {
                    // Kẻ địch ở bên trái nhân vật
                    isHitFromRightSide = false;
                }
                Debug.Log("Hit");
                KnockBack();
            }
        }
        //// kiểm tra xem có chạm vào kẻ địch hay là không
        //if(collision.CompareTag("Enemy") && damageable.IsAlive)
        //{
        //    // Lấy GameObject cha của đối tượng EnemyHitBox (là Skeleton)
        //    Transform enemyTransform = collision.transform.root; // lấy gốc root (cha lớn nhất)

        //    // Lấy vị trí của Skeleton
        //    Vector2 enemyPosition = enemyTransform.position;

        //    // Gọi KnockBack với vị trí của kẻ địch (Skeleton)
        //    // Kiểm tra xem nhân vật có bị đánh từ phía bên phải hay bên trái
        //    if (transform.position.x < enemyPosition.x)
        //    {
        //        // Kẻ địch ở bên phải nhân vật
        //        isHitFromRightSide = true;
        //    }
        //    else if (transform.position.x > enemyPosition.x)
        //    {
        //        // Kẻ địch ở bên trái nhân vật
        //        isHitFromRightSide = false;
        //    }
        //    Debug.Log("Va cham");
        //    damageable.TakeDamage(20, new Vector2(5, 2));
        //    KnockBack();
        //}
    }

    public void setCurrentAttackDirection(string direction)
    {
        if(direction == "up")
        {
            attackDirection = Vector3.up;
        }
        else if(direction == "down") {
            if (isFacingRight) attackDirection = new Vector3(1, -1, 0);
           else attackDirection = new Vector3(-1, -1, 0);

        }
        else if(direction == "right")
        {
            if(isFacingRight)
            attackDirection = Vector3.right;
            else attackDirection = Vector3.left;
        }
        else if(direction == "left")
        {
            if (isFacingRight)
                attackDirection = Vector3.left;
            else attackDirection = Vector3.right;
        }


    }

    public bool CanPerformAction(float staminaCost)
    {
        // Kiểm tra xem Stamina có đủ để thực hiện hành động không
        return currentStamina >= staminaCost;
    }
    public void UseStamina(float amount)
    {
        if (currentStamina > 0)
        {
            currentStamina -= amount;
            staminaRecoveryTimer = 0f; // Đặt lại thời gian chờ hồi phục
            UIManager.Instance.SetStamina(currentStamina);
        }
    }
    private void HandleStaminaRecovery()
    {
        if (currentStamina < maxStamina)
        {
            if (staminaRecoveryTimer >= staminaWaitTime)
            {
                currentStamina += staminaRecoveryRate * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina); // Giới hạn Stamina không vượt quá max
            }
            else
            {
                staminaRecoveryTimer += Time.deltaTime;
            }
            UIManager.Instance.SetStamina(currentStamina);
        }
    }
}
