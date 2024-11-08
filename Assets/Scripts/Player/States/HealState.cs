using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealState : PlayerStateBase
{
    public HealState(PlayerScript player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Hello from heal State");
        player.isHealing = true;
        player.damageable.Heal(25);
        player.estucFlasks--;
        UIManager.Instance.SetHealth(player.damageable.CurrentHealth);
        UIManager.Instance.healthBar.UsePotion();
        GameObject healingEffectObject = Object.Instantiate(player.healingEffectPrefab, player.transform.position, Quaternion.identity);

        // lấy ra object đó
        HealingEffect healingEffect = healingEffectObject.GetComponent<HealingEffect>();

        // đăng ký callback cho sự kiện healing hoàn tất
        healingEffect.animationComplete.AddListener(OnHealingComplete);

        /*Giải thích:
Trong HealingEffect, sau khi animation kết thúc (sau khi hàm OnHealingEnd được gọi), UnityEvent healingAnimationComplete sẽ được kích hoạt.
Trong HealState, khi EnterState được gọi, bạn sẽ Instantiate hiệu ứng hồi máu và lấy component HealingEffect. Sau đó, đăng ký callback vào sự kiện healingAnimationComplete.
Khi sự kiện này được kích hoạt, callback OnHealingComplete sẽ được gọi, và bạn có thể chuyển trạng thái của player sang trạng thái khác (như idleState) khi quá trình hồi máu hoàn tất.
         */
    }

    public override void ExitState()
    {
        Debug.Log("Exit from heal State");
    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
            
    }
    private void OnHealingComplete()
    {
        // Chuyển sang trạng thái khác sau khi hồi máu kết thúc
        playerStateMachine.ChangeState(player.idleState);
    }
}
