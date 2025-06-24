using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    public static ItemTooltip Instance;

    public GameObject tooltipObject; // ��� UI ������ (������)
    public TextMeshProUGUI tooltipText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            HideTooltip();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void ShowTooltip(ItemData item, Vector2 position)
    {
        tooltipObject.SetActive(true);
        tooltipObject.transform.position = position;

        string stats = "";

        if (item.armorBonus > 0)
            stats += $"�����: +{item.armorBonus}\n";
        if (item.damageBonus > 0)
            stats += $"����: +{item.damageBonus}\n";
        if (item.attackCooldownModifier != 1f)
        {
            float percent = (1f - item.attackCooldownModifier) * 100f;
            stats += $"�������� �����: {(percent > 0 ? "+" : "")}{percent:F0}%\n";
        }

        tooltipText.text = $"<b>{item.itemName}</b>\n{stats}";
    }

    public void HideTooltip()
    {
        tooltipObject.SetActive(false);
    }
}
