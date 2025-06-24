using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    public Image fillImage;         // Красная полоска
    public Enemy boss;             // Ссылка на DeadBoss

    private void Update()
    {
        if (boss != null && fillImage != null)
        {
            float fill = (float)boss.CurrentHealth / boss.MaxHealth;
            fillImage.fillAmount = fill;
        }

        // Повернуть к камере
        transform.rotation = Camera.main.transform.rotation;
    }
}
