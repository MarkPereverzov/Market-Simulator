using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LicensesManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject licenseItemPrefab;
    public Transform contentParent;

    [Header("License List")]
    public License[] licenses;

    void Start()
    {
        PopulateLicenseList();
    }

    void PopulateLicenseList()
    {
        foreach (var license in licenses)
        {
            GameObject item = Instantiate(licenseItemPrefab, contentParent);

            // Установка имени и цены лицензии
            item.transform.Find("LicenseName").GetComponent<TextMeshProUGUI>().text = license.name;
            item.transform.Find("LicensePrice").GetComponent<TextMeshProUGUI>().text = $"${license.price:F2}";

            // Настройка кнопки покупки
            Button buyButton = item.transform.Find("BuyButton").GetComponent<Button>();
            TextMeshProUGUI buttonText = buyButton.GetComponentInChildren<TextMeshProUGUI>();

            if (license.isPurchased)
            {
                buyButton.interactable = false; // Отключить кнопку
                if (buttonText != null)
                {
                    buttonText.text = "Bought"; // Установить текст "Bought"
                }
            }
            else
            {
                buyButton.interactable = true; // Включить кнопку
                if (buttonText != null)
                {
                    buttonText.text = "Buy"; // Установить текст "Buy"
                }

                // Добавить обработчик нажатия на кнопку
                buyButton.onClick.AddListener(() => PurchaseLicense(license, buyButton));
            }
        }
    }

    void PurchaseLicense(License license, Button buyButton)
    {
        if (license.isPurchased)
        {
            Debug.Log($"License {license.name} is already purchased!");
            return;
        }

        if (UIManager.Instance.SpendMoney(license.price))
        {
            license.isPurchased = true; // Отметить как купленную
            buyButton.interactable = false; // Отключить кнопку

            // Изменить текст кнопки на "Bought"
            TextMeshProUGUI buttonText = buyButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = "Bought";
            }

            Debug.Log($"License {license.name} purchased!");
        }
        else
        {
            Debug.Log("Not enough money to purchase this license!");
        }
    }
}
