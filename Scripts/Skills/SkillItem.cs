using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SkillItem : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text skillNameText;
    public TMP_Text priceText;
    public TMP_Text skillDescriptionText;
    public Button purchaseButton;       // кнопка
    public TMP_Text purchaseButtonText; // текст на кнопке (нужно добавить в инспекторе)

    private string skillId;
    private int price;
    public event Action<string> OnPurchaseRequested;

    public void Setup(string id, Sprite icon, string skillName, int price, string description, bool isPurchased)
    {
        skillId = id;
        iconImage.sprite = icon;
        skillNameText.text = skillName;
        priceText.text = price.ToString();
        skillDescriptionText.text = description;
        this.price = price;

        purchaseButton.interactable = !isPurchased;

        if (isPurchased)
        {
            purchaseButtonText.text = "Upgraded";   // меняем текст кнопки, а не цену
            priceText.gameObject.SetActive(false);  // скрываем цену (по желанию)
        }
        else
        {
            purchaseButtonText.text = "Upgrade";         // или любой другой текст по умолчанию
            priceText.gameObject.SetActive(true);   // показываем цену
        }

        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        OnPurchaseRequested?.Invoke(skillId);
    }

    public void MarkPurchased()
    {
        purchaseButton.interactable = false;
        purchaseButtonText.text = "Upgraded";
        priceText.gameObject.SetActive(false);
    }
}
