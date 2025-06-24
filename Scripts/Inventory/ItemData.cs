using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;

    public int armorBonus;
    public int damageBonus;
    public float attackCooldownModifier = 1f; // ���������: 1 � ��� ���������, <1 � �������, >1 � ���������
}

public enum ItemType
{
    Helmet,
    Chest,
    Sword
}
