using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;

    public GameObject pickupPromptUI;

    private bool playerInRange;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.R))
        {
            bool added = InventoryManager.Instance.AddItem(itemData);
            if (added)
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            if (pickupPromptUI != null) pickupPromptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            if (pickupPromptUI != null) pickupPromptUI.SetActive(false);
        }
    }

}
