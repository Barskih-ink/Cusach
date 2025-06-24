using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [Header("Стартовая экипировка")]
    public ItemData starterHelmet;
    public ItemData starterChest;
    public ItemData starterSword;


    public static EquipmentManager Instance;

    public EquipmentSlot helmetSlot;
    public EquipmentSlot chestSlot;
    public EquipmentSlot swordSlot;

    private PlayerHealth playerHealth;
    private PlayerCombat playerCombat;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerCombat = FindObjectOfType<PlayerCombat>();

        if (starterHelmet != null) helmetSlot.Equip(starterHelmet);
        if (starterChest != null) chestSlot.Equip(starterChest);
        if (starterSword != null) swordSlot.Equip(starterSword);

        ApplyBonuses(); // Пересчёт бонусов

    }

    public bool EquipItem(ItemData item)
    {
        bool equipped = false;

        switch (item.itemType)
        {
            case ItemType.Helmet:
                equipped = helmetSlot.Equip(item);
                break;
            case ItemType.Chest:
                equipped = chestSlot.Equip(item);
                break;
            case ItemType.Sword:
                equipped = swordSlot.Equip(item);
                break;
            default:
                Debug.LogWarning("Неизвестный тип предмета: " + item.itemType);
                break;
        }

        ApplyBonuses();
        return equipped;
    }



    public void ApplyBonuses()
    {
        int totalArmor = 0;
        int totalDamage = 0;
        float cooldownModifier = 1f;

        if (!helmetSlot.IsEmpty())
        {
            totalArmor += helmetSlot.GetEquippedItem().armorBonus;
        }

        if (!chestSlot.IsEmpty())
        {
            totalArmor += chestSlot.GetEquippedItem().armorBonus;
        }

        if (!swordSlot.IsEmpty())
        {
            var sword = swordSlot.GetEquippedItem();
            totalDamage += sword.damageBonus;
            cooldownModifier *= sword.attackCooldownModifier;
        }

        if (playerHealth != null)
        {
            playerHealth.armor = totalArmor;

            // Обновляем активную броню, только если она не "сломана"
            if (!playerHealth.armorBroken)
            {
                playerHealth.activeArmor = totalArmor;
            }
        }

        if (playerCombat != null)
        {
            playerCombat.attackDamage = 1 + totalDamage;
            playerCombat.attackCooldown = 1f * cooldownModifier;
        }

    }
}
