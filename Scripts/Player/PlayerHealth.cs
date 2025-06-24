using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Здоровье")]
    public int maxHealth = 5;
    public int currentHealth;
    public Image healthBarFill;

    [Header("Хилки")]
    public int maxHeals = 1;
    public int currentHeals;
    public GameObject healIconPrefab;
    public Transform healContainer;
    private Image[] healIcons;

    [Header("Настройки")]
    public int healAmount = 2;
    public KeyCode healKey = KeyCode.E;

    [Header("Souls UI")]
    public int currentSouls = 0;
    public TMP_Text soulsCountText;

    [Header("Броня")]
    public int armor = 0; // Общая броня от предметов
    public int activeArmor = 0; // Временная активная броня
    public float armorRestoreDelay = 5f; // Время восстановления
    private float armorRestoreTimer = 0f;
    public bool armorBroken = false;


    [Header("Экран смерти")]
    public GameObject deathScreenUI;

    [Header("Звуки")]
    public AudioClip healSound;
    public AudioClip deathSound;
    public AudioClip hurtSound;

    public AudioSource healSource;
    public AudioSource deathSource;
    public AudioSource hurtSource;

    private Animator animator;
    public bool isDead { get; private set; } = false;

    private PlayerMovement playerMovement;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        currentHealth = maxHealth;

        currentHeals = maxHeals;
        GenerateHealIcons();
        UpdateHealthUI();
        UpdateHealUI();
        UpdateSoulsUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }

        if (Input.GetKeyDown(healKey))
        {
            UseHeal();
        }

        // Восстановление брони
        if (armorBroken)
        {
            armorRestoreTimer -= Time.deltaTime;
            if (armorRestoreTimer <= 0f)
            {
                activeArmor = armor;
                armorBroken = false;
            }
        }
    }

    public bool TrySpendSouls(int amount)
    {
        if (currentSouls >= amount)
        {
            currentSouls -= amount;
            UpdateSoulsUI();
            return true;
        }
        return false;
    }

    public void AddSouls(int amount)
    {
        currentSouls += amount;
        UpdateSoulsUI();
    }

    public void ResetSouls()
    {
        currentSouls = 0;
        UpdateSoulsUI();
    }

    private void UpdateSoulsUI()
    {
        if (soulsCountText != null)
        {
            soulsCountText.text = currentSouls.ToString();
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead || currentHealth <= 0) return;
        if (playerMovement != null && playerMovement.IsRolling) return;

        // Урон поглощается активной бронёй
        int effectiveArmor = Mathf.Min(activeArmor, amount);
        int effectiveDamage = amount - effectiveArmor;

        activeArmor -= effectiveArmor;

        if (activeArmor < armor && !armorBroken)
        {
            armorBroken = true;
            armorRestoreTimer = armorRestoreDelay;
        }

        currentHealth -= effectiveDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        animator.SetTrigger("Hurt");

        if (hurtSource != null && hurtSound != null)
            hurtSource.PlayOneShot(hurtSound);

        if (currentHealth <= 0)
        {
            Die();
        }
    }



    public void UseHeal()
    {
        if (currentHeals <= 0 || isDead || currentHealth >= maxHealth) return;

        currentHeals--;
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();
        UpdateHealUI();

        animator.SetTrigger("Heal");

        if (healSource != null && healSound != null)
        {
            healSource.PlayOneShot(healSound);
        }
    }

    private void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    private void UpdateHealUI()
    {
        for (int i = 0; i < healIcons.Length; i++)
        {
            if (healIcons[i] != null)
            {
                healIcons[i].color = (i < currentHeals) ? Color.white : Color.gray;
            }
        }
    }

    public void GenerateHealIcons()
    {
        if (healContainer == null || healIconPrefab == null) return;

        foreach (Transform child in healContainer)
        {
            Destroy(child.gameObject);
        }

        healIcons = new Image[maxHeals];

        for (int i = 0; i < maxHeals; i++)
        {
            GameObject icon = Instantiate(healIconPrefab, healContainer);
            Image img = icon.GetComponent<Image>();
            if (img != null)
            {
                healIcons[i] = img;
            }
        }
    }

    private void Die()
    {
        isDead = true;
        ResetSouls();
        animator.SetTrigger("Die");

        if (deathSource != null && deathSound != null)
        {
            deathSource.PlayOneShot(deathSound);
        }

        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(true);
        }

        StartCoroutine(HandleDeathRoutine());
    }

    private System.Collections.IEnumerator HandleDeathRoutine()
    {
        yield return new WaitForSeconds(2.5f);

        if (CheckpointManager.Instance != null && CheckpointManager.Instance.hasCheckpoint)
        {
            transform.position = CheckpointManager.Instance.respawnPosition;

            HealToFull();
            RefillHeals();

            isDead = false;
            animator.ResetTrigger("Die");
            animator.Play("idle");

            if (deathScreenUI != null)
            {
                deathScreenUI.SetActive(false);
            }
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void HealToFull()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void RefillHeals()
    {
        currentHeals = maxHeals;
        UpdateHealUI();
    }
}
