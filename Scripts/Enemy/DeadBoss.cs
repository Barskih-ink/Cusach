using UnityEngine;
using UnityEngine.UI;

public class DeadBoss : Enemy
{
    protected override void Start()
    {
        base.Start();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        base.Die();
    }

    protected override void Attack()
    {
        Debug.Log("јтакует!");
        base.Attack();
    }

    // ”бираем поведение щита Ч гоблин не умеет блокировать
    protected override void TryActivateShield() { }
    protected override void ActivateShield() { }
}
