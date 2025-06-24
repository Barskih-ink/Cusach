using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;

    public int armorBonus;
    public int damageBonus;
    public float attackCooldownModifier = 1f; // множитель: 1 Ч без изменений, <1 Ч быстрее, >1 Ч медленнее
}

public enum ItemType
{
    Helmet,
    Chest,
    Sword
}
