using UnityEngine;

public class AmberGoblin : Enemy
{
    [Header("Уклонение")]
    [Range(0f, 1f)]
    public float dodgeChance = 0.3f; // 25% шанс уклонения

    protected override void Start()
    {
        base.Start();
        shieldBlockPercentage = 0f; // не использует щит, только уклонение
    }

    public override void TakeDamage(int damage)
    {
        if (Random.value < dodgeChance)
        {
            Debug.Log($"{gameObject.name} уклонился от атаки!");
            animator.SetTrigger("Dodge");
            return; // полностью избегает урона
        }

        base.TakeDamage(damage);
    }

    protected override void Attack()
    {
        Debug.Log("AmberGoblin делает быструю атаку!");
        base.Attack();
    }

    // Убираем поведение щита — гоблин не умеет блокировать
    protected override void TryActivateShield() { }
    protected override void ActivateShield() { }
}
