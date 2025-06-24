using UnityEngine;

public class SpecterSkeleton : Enemy
{
    protected override void Start()
    {
        base.Start();
        shieldBlockPercentage = 1f;   // блокирует 50% урона
        shieldDuration = 1.5f;
        shieldCooldown = 3f;
    }

    protected override void ActivateShield()
    {
        base.ActivateShield();
        Debug.Log("SpecterSkeleton быстро активирует теневой щит!");
        // Можно добавить быстрый визуальный эффект
    }

    protected override void Attack()
    {
        Debug.Log("SpecterSkeleton атакует быстро!");
        base.Attack();
    }
}
