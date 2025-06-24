using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    [Tooltip("Игрок, которого нужно скрыть")]
    public GameObject player;

    [Tooltip("Имя сцены главного меню")]
    public string mainMenuSceneName = "MainMenu";

    private bool levelEnded = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (levelEnded) return;

        if (other.CompareTag("Player"))
        {
            levelEnded = true;

            Debug.Log("Игрок коснулся шахты. Завершение уровня.");

            if (player != null)
            {
                player.SetActive(false); // Скрыть игрока
            }

            // Загружаем меню
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}
