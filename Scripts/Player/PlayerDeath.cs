using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Объект {other.name} вошёл в зону смерти");

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            Debug.Log("Игрок найден, наносим смертельный урон");
            playerHealth.TakeDamage(playerHealth.currentHealth);
        }
    }
}
