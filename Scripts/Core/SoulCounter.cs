using UnityEngine;
using UnityEngine.UI;

public class SoulCounter : MonoBehaviour
{
    public int souls = 0;
    public Text soulText;

    void Start()
    {
        UpdateUI();
    }

    public void AddSouls(int amount)
    {
        souls += amount;
        UpdateUI();
    }

    public void SetSouls(int amount)
    {
        souls = amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (soulText != null)
        {
            soulText.text = $"Души: {souls}";
        }
    }
}
