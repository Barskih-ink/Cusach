using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public GameObject inventoryUI;
    public GameObject equipmentUI;
    public InventorySlot[] slots;

    private bool isOpen = false;

    public GameObject droppedItemPrefab;
    public Transform dropPoint; // например, пустой объект около игрока


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;
            inventoryUI.SetActive(isOpen);
            equipmentUI.SetActive(isOpen); // Добавить
        }
    }


    public bool AddItem(ItemData item)
    {
        foreach (var slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.SetItem(item);
                return true;
            }
        }

        Debug.Log("Инвентарь полон");
        return false;
    }

    public void DropItem(ItemData item)
    {
        if (droppedItemPrefab != null && dropPoint != null)
        {
            GameObject dropped = Instantiate(droppedItemPrefab, dropPoint.position, Quaternion.identity);
            ItemPickup pickup = dropped.GetComponent<ItemPickup>();
            if (pickup != null)
            {
                pickup.itemData = item;
            }
        }
    }

}
