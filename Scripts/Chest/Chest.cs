using UnityEngine;

public class Chest : MonoBehaviour
{
    public ItemData itemInside;
    public GameObject interactPromptUI;

    public Sprite closedChestSprite;  // ������ ��������� �������
    public Sprite openedChestSprite;  // ������ ��������� �������

    private bool playerInRange = false;
    private bool isOpened = false;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer �� ������ �� ������� �������!");
        }
        else if (closedChestSprite != null)
        {
            spriteRenderer.sprite = closedChestSprite;  // ������������� ������ ��������� ������� � ������
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
                    spriteRenderer.sprite = openedChestSprite;  // ������ �� ������ ��������� �������
                }

                Debug.Log("����� ������ ������� �� �������: " + itemInside.itemName);
            }
            else
            {
                Debug.Log("��������� �����. ���������� ����� �������.");
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
