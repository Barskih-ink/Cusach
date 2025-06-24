using UnityEngine;

public class GladiatorSkeleton : Enemy
{
    protected override void Start()
    {
        base.Start();
        shieldBlockPercentage = 0.5f;  // блокирует 75% урона
        shieldDuration = 1f;
        shieldCooldown = 4f;
    }

    protected override void ActivateShield()
    {
        base.ActivateShield();
        Debug.Log("GladiatorSkeleton поднимает щит");
        // Можно добавить анимацию или эффекты
    }

    protected override void Attack()
    {
        Debug.Log("GladiatorSkeleton атакует с щитом!");
        base.Attack();
    }
}
