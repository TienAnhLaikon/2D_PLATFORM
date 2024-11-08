using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField]
    private Collider2D attackHitBox;
    public int attackDamage = 10;
    public Vector2 knockBackForce = Vector2.zero;
    public float shakeIntensity; // cường độ rung 

    public GameObject hitEffectPrefab; // prefab của hiệu ứng hồi máu
    private GameObject hitEffectInstance;

    public GameObject bloodEffectPrefab;
    public CameraManager cameraManager;
    public HitStop hitStop;
    public Vector3 attackDirection = Vector3.zero;
    public Transform attackPosition; // biến này để xử lý vị trí sinh ra effect của những animation quá là khó nhai
    public bool isNeedAttackPosition = false; // biến này để biết ta có cần biến tranform trên không. Có những hiệu ứng ta sẽ sinh ra ở vị trí kẻ địch hoặc va chạm collider nhưng cũng cóc animation hiệu ta muốn nó sinh ra tại vị trí cụ thể

    // Start is called before the first frame update
    private void Awake()
    {
       
        attackHitBox = GetComponent<Collider2D>();
       hitStop = GetComponent<HitStop>();
        // Tìm GameObject Main Camera
        GameObject mainCamera = GameObject.Find("Main Camera");

        // Nếu tìm thấy, lấy component CameraManager
        if (mainCamera != null)
        {
            cameraManager = mainCamera.GetComponent<CameraManager>();
        }
        else
        {
            Debug.LogWarning("Main Camera not found! Make sure it is named 'Main Camera'.");
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // see if it can be hit
        // kiểm tra xem đối tượng va chạm có thể damagealbe hay không?
        Damageable objectDamageable = collision.GetComponent<Damageable>();
        if (objectDamageable != null) {
            bool gotHit = objectDamageable.TakeDamage(attackDamage,knockBackForce);
            if (gotHit)
            {
                // tạo hiệu ứng tại vị trí đối phương
                if (isNeedAttackPosition) HitReaction(attackPosition.position);
                else HitReaction(collision.transform.position);
                hitStop.Stop(0.05f);

            }
        }
    }

    public void HitReaction(Vector3 position)
    {
        // tìm đối tượng của AttackHitBox
        GameObject parentObject = transform.parent.gameObject;
       
        // Kiểm tra xem parent là Player hay Enemy
        if (parentObject.CompareTag("Player"))
        {
            // nếu parrent là player
            attackDirection = parentObject.GetComponent<PlayerScript>().attackDirection;
            cameraManager.StartShake(attackDirection, 0.1f, shakeIntensity);
            //Debug.Log("Player Shake");
        }
        else if (parentObject.CompareTag("Enemy"))
        {
            // nếu parrent là enemy
            attackDirection = parentObject.GetComponent<Enemy>().attackDirection;
            cameraManager.StartShake(attackDirection, 0.1f, shakeIntensity);
            //Debug.Log("Enemy Shake");
        }
        else if (parentObject.CompareTag("FlyingEnemy"))
        {
            // nếu parrent là enemy
            attackDirection = parentObject.GetComponent<FlyingEnemy>().attackDirection;
            cameraManager.StartShake(attackDirection, 0.1f, shakeIntensity);
           // Debug.Log("Flying Enemy Shake");
        }
        else if (parentObject.CompareTag("Boss"))
        {
            // nếu parrent là boss
            attackDirection = parentObject.GetComponent<Boss>().attackDirection;
            cameraManager.StartShake(attackDirection, 0.1f, shakeIntensity);
           // Debug.Log("Boss");
        }
        CreateHitEffect(position,attackDirection);
        CreateBloodEffect(position,attackDirection);    
    }
    private void CreateHitEffect(Vector3 position,Vector3 attackDirection)
    {
        //Debug.Log("Attack Direction:" + attackDirection.x);
        // Tạo một biến Quaternion để lưu trữ hướng của hit effect
        Quaternion rotation = Quaternion.identity;

        // Kiểm tra hướng tấn công (từ trái hay từ phải)
        if (attackDirection.x < 0)
        {
            // Nếu tấn công từ trái sang phải, ta sẽ flip hiệu ứng bằng cách xoay 180 độ theo trục Y
            rotation = Quaternion.Euler(0, 180, 0);
        }
        // Gọi Particle hoặc Instantiate hiệu ứng máu tại vị trí "position"
        Instantiate(hitEffectPrefab, position, rotation);
    }
    private void CreateBloodEffect(Vector3 position, Vector3 attackDirection)
    {
        //Debug.Log("Attack Direction:" + attackDirection.x);
        // Tạo một biến Quaternion để lưu trữ hướng của hit effect
        Quaternion rotation = Quaternion.identity;

        // Kiểm tra hướng tấn công (từ trái hay từ phải)
        if (attackDirection.x > 0)
        {
            // Nếu tấn công từ trái sang phải, ta sẽ flip hiệu ứng bằng cách xoay 180 độ theo trục Y
            rotation = Quaternion.Euler(0, 180, 0);
        }
        // Gọi Particle hoặc Instantiate hiệu ứng máu tại vị trí "position"
        Instantiate(bloodEffectPrefab, position, rotation);
    }


}
