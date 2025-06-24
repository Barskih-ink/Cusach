using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    private ItemData currentItem;

    private void Update()
    {
        // При наведении курсора и ПКМ
        if (currentItem != null && IsMouseOver() && Input.GetMouseButtonDown(1))
        {
            Vector2 mousePos = Input.mousePosition;
            ItemTooltip.Instance.ShowTooltip(currentItem, mousePos);
        }

        // Скрытие по клику в любом другом месте
        if (Input.GetMouseButtonDown(0))
        {
            ItemTooltip.Instance.HideTooltip();
        }
    }

    private bool IsMouseOver()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(
            GetComponent<RectTransform>(),
            Input.mousePosition,
            null
        );
    }


    public void SetItem(ItemData item)
    {
        currentItem = item;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public bool IsEmpty()
    {
        return currentItem == null;
    }

    public ItemData GetItem()
    {
        return currentItem;
    }

    public void OnClick()
    {
        if (currentItem != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                InventoryManager.Instance.DropItem(currentItem);
                ClearItem(); // убираем из инвентаря
            }
            else
            {
                bool equipped = EquipmentManager.Instance.EquipItem(currentItem);
                if (equipped)
                {
                    ClearItem();
                }
            }
        }
    }


    public void RemoveItem()
    {
        currentItem = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void ClearItem()
    {
        currentItem = null;
        icon.sprite = null;
        icon.enabled = false;
    }




}
