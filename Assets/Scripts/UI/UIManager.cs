using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIManager Instance { get; private set; }
    public HealthBar healthBar;
    public StaminaBar staminaBar;
    public HealthBar bossHealth_Bar;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void SetHealth(int health)
    {
        healthBar.SetHealth(health);
    }
    public void SetStamina(float stamina)
    {
        staminaBar.SetStamina(stamina);
    }
    public void SetBossHealth(int health)
    {
        bossHealth_Bar.SetHealth(health);
    }


}
