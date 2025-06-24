using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DebugMenu : MonoBehaviour
{
    [Header("Ссылки на UI")]
    public TMP_InputField soulsInputField;
    public Button addSoulsButton;
    public Button killAllEnemiesButton;

    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();

        addSoulsButton.onClick.AddListener(OnAddSoulsClicked);
        killAllEnemiesButton.onClick.AddListener(OnKillAllEnemiesClicked);
    }

    public GameObject debugMenuPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            debugMenuPanel.SetActive(!debugMenuPanel.activeSelf);
        }
    }


    private void OnAddSoulsClicked()
    {
        if (playerHealth == null)
        {
            Debug.LogWarning("PlayerHealth не найден!");
            return;
        }

        if (int.TryParse(soulsInputField.text, out int soulsToAdd))
        {
            playerHealth.AddSouls(soulsToAdd);
            Debug.Log($"Добавлено {soulsToAdd} душ");
        }
        else
        {
            Debug.LogWarning("Некорректное число для добавления душ");
        }
    }

    private void OnKillAllEnemiesClicked()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemies)
        {
            enemy.TakeDamage(99999); // Много урона, чтобы точно убить
        }

        Debug.Log($"Убито врагов: {enemies.Length}");
    }
}
