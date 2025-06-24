using UnityEngine;

public class Chest : MonoBehaviour
{
    public ItemData itemInside;
    public GameObject interactPromptUI;

    public Sprite closedChestSprite;  // Спрайт закрытого сундука
    public Sprite openedChestSprite;  // Спрайт открытого сундука

    private bool playerInRange = false;
    private bool isOpened = false;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer не найден на объекте сундука!");
        }
        else if (closedChestSprite != null)
        {
            spriteRenderer.sprite = closedChestSprite;  // Устанавливаем спрайт закрытого сундука в начале
        }
    }

    void Update()
    {
        if (playerInRange && !isOpened && Input.GetKeyDown(KeyCode.R))
        {
            bool added = InventoryManager.Instance.AddItem(itemInside);
            if (added)
            {
                isOpened = true;
                interactPromptUI.SetActive(false);

                if (openedChestSprite != null && spriteRenderer != null)
                {
                    spriteRenderer.sprite = openedChestSprite;  // Меняем на спрайт открытого сундука
                }

                Debug.Log("Игрок забрал предмет из сундука: " + itemInside.itemName);
            }
            else
            {
                Debug.Log("Инвентарь полон. Невозможно взять предмет.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened)
        {
            playerInRange = true;
            if (interactPromptUI != null) interactPromptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactPromptUI != null) interactPromptUI.SetActive(false);
        }
    }
}
