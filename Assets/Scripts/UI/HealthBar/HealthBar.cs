using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public List<GameObject> potions;
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;

        slider.value = health;
    }
    public void SetHealth(int health)
    {
        slider.value = health;
    }
    public void UsePotion()
    {
        if (potions.Count > 0)
        {
            // Ẩn bình máu đầu tiên trong danh sách (Potion3 -> Potion2 -> Potion1)
            GameObject potionToHide = potions[0];
            potionToHide.SetActive(false);

            // Xóa bình máu đã bị ẩn khỏi danh sách
            potions.RemoveAt(0);
        }
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);

    }
}
