using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class Bonfire : MonoBehaviour
{
    private bool isPlayerInRange = false;
    private PlayerMovement playerMovement; // Скрипт управления
    private PlayerCombat playerCombat;     // (если нужно отключить боёвку)
    public GameObject pauseMenuCanvas;
    private PlayerHealth playerHealth;
    public GameObject bonfireIconUI;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            OpenBonfireMenu();
        }
    }

    private void OpenBonfireMenu()
    {
        // Найдём игрока, если ссылки не установлены
        if (playerMovement == null || playerCombat == null || playerHealth == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerMovement = player.GetComponent<PlayerMovement>();
                playerCombat = player.GetComponent<PlayerCombat>();
                playerHealth = player.GetComponent<PlayerHealth>();
            }
        }

        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;

        if (playerMovement != null)
            playerMovement.enabled = false;
        if (playerCombat != null)
            playerCombat.enabled = false;

        CheckpointManager.Instance?.SetCheckpoint(transform.position);

        if (playerHealth != null)
        {
            playerHealth.HealToFull();
            playerHealth.RefillHeals();
        }
    }


    public void CloseBonfireMenu()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;

        // Найдём игрока, если ссылки не установлены
        if (playerMovement == null || playerCombat == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerMovement = player.GetComponent<PlayerMovement>();
                playerCombat = player.GetComponent<PlayerCombat>();
            }
        }

        if (playerMovement != null)
            playerMovement.enabled = true;
        if (playerCombat != null)
            playerCombat.enabled = true;
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            bonfireIconUI.SetActive(true);
            playerMovement = other.GetComponent<PlayerMovement>();
            playerCombat = other.GetComponent<PlayerCombat>();
            playerHealth = other.GetComponent<PlayerHealth>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            bonfireIconUI.SetActive(false);
        }
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Возвращаем время на случай, если оно было остановлено
        SceneManager.LoadScene("MainMenu"); // Укажи точное имя своей сцены
    }

}
