using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    // ta dùng UnityEvenet để gửi sự kiện này khi nó xảy ra đến lớp player hoặc enemy để sử dụng hàm ở phương thức đó
    // Chứ ở lớp này ta không muốn xử lý logic hay vật lý của đối tượng
    public UnityEvent<int,Vector2> damageableHit;
    /* DAMAGEABLE 
     Đây là lớp để xử lý việc Player hay kẻ địch có thể bị nhận sát thương từ đòn đánh (Attack)
    Và từ đó ta có thể xử lý trừ máu để chuyển vào trạng thái Hit State (bị đánh)
    hoặc từ đây ta cũng có thể xử lý được player hoặc enemy chuyển vào trạng thái death state nếu đã hết máu

     */
    [SerializeField]
    private int _maxHealth = 100; // máu tối đa 
    public int MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }
    [SerializeField]
    private int _currentHealth = 100; // máu hiện tại
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            _currentHealth = value;
            // nhưng nếu máu tuột xuống dưới 0 thì đối tượng này sẽ chết}
            if (_currentHealth <= 0)
            {
                IsAlive = false;
                // khi chết thì gọi OnDeath
                OnDeath();
            }
        }
    }
    [SerializeField]
    private bool _isAlive = true; // biến ta khởi tạo vì mặc nhiên kẻ địch sẽ luôn sống lúc đầu game chứ đúng không?
    [SerializeField]
    private bool isInvincible;

    public bool IsHit { get; private set; }

    public float timeSinceHit = 0;
    public float invincibilityTimer = 0.25f;

    // Sự kiện khi đối tượng chết
    public event System.Action OnDeath = delegate { };  // Có thể dùng Action để gọi sự kiện chết
    public bool IsAlive
    {
        get { return _isAlive; }
        set { _isAlive = value;
            //animator.SetBool("IsAlive", value);
            //Debug.Log("IsAlive set " +  value);
        }
    }
    private void Awake()
    {
    }
    private void Update()
    {
        if (isInvincible)
        {
            if ((timeSinceHit > invincibilityTimer))
            {
                // remove invincibility
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }

    }
    // hàm xử lý việc nhận damage
    public bool TakeDamage(int amountDamage, Vector2 knockBackForce)
    {
        // Lấy đối tượng cha có script Player
        PlayerScript parentPlayer = GetComponentInParent<PlayerScript>();

        // Nếu đối tượng cha là Player, kiểm tra trạng thái IsRolling
        if (parentPlayer != null) // Kiểm tra có phải là Player không
        {
            if (parentPlayer.isRolling) // Nếu Player đang lăn (IsRolling)
            {
                Debug.Log("Not take damage");
                return false; // Không nhận damage
            }
        }


        if (IsAlive&& !isInvincible)
        {
            CurrentHealth -= amountDamage;
            isInvincible = true;
            IsHit = true;
            damageableHit?.Invoke(amountDamage,knockBackForce);

            return true;
        }
        return false;
    }
    public void Heal(int health)
    {
        if (IsAlive)
        {
            int maxHeal = Mathf.Max(MaxHealth - CurrentHealth, 0);
            int actualHeal = Mathf.Min(maxHeal, health);
            CurrentHealth += actualHeal;
        }
    }
}
