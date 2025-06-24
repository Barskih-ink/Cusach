using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public ItemType slotType;
    public Image icon;
    private ItemData equippedItem;

    public bool Equip(ItemData item)
    {
        if (!IsEmpty())
        {
            Debug.Log("Слот уже занят: " + slotType);
            return false;
        }

        equippedItem = item;
        icon.sprite = item.icon;
        icon.enabled = true;
        return true;
    }


    public ItemData GetEquippedItem() => equippedItem;

    public bool IsEmpty() => equippedItem == null;

    public void Clear()
    {
        equippedItem = null;
        icon.enabled = false;
    }

    public void Unequip()
    {
        if (equippedItem != null)
        {
            bool added = InventoryManager.Instance.AddItem(equippedItem);
            if (added)
            {
                Clear();
                EquipmentManager.Instance.ApplyBonuses(); // пересчитать бонусы
            }
            else
            {
                Debug.Log("Инвентарь полон. Невозможно снять предмет.");
            }
        }
    }

    public void OnClick()
    {
        Unequip(); // снимаем предмет при клике на слот экипировки
    }


}
