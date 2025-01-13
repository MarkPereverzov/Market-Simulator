using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ProductManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject productItemPrefab;  // Префаб для товара
    public Transform contentParent;       // Контейнер товаров в ScrollView
    public TextMeshProUGUI totalItemsText; // Текст для отображения общего количества товаров
    public TextMeshProUGUI totalPriceText; // Текст для отображения общей суммы
    public GameObject productPanel;       // Панель товаров
    public TMP_Text closePanelButtonText; // Текст на кнопке закрытия панели (если нужно)

    [Header("Product Data")]
    public Product[] products; // Список доступных товаров

    private Dictionary<Product, int> productQuantities = new Dictionary<Product, int>();

    void Start()
    {
        PopulateProductList();
        UpdateTotal();

        // Скрываем панель товаров в начале
        productPanel.SetActive(false);
    }

    void Update()
    {
        // Открытие/закрытие панели по нажатию клавиши B
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleProductPanel();
        }
    }

    void PopulateProductList()
    {
        foreach (var product in products)
        {
            // Создаём экземпляр префаба для товара
            GameObject item = Instantiate(productItemPrefab, contentParent);

            // Устанавливаем название товара
            item.transform.Find("ProductName").GetComponent<TextMeshProUGUI>().text = product.name;

            // Устанавливаем цену товара
            item.transform.Find("ProductPrice").GetComponent<TextMeshProUGUI>().text = $"${product.price:F2}";

            // Инициализируем количество товара
            TextMeshProUGUI quantityText = item.transform.Find("QuantityText").GetComponent<TextMeshProUGUI>();
            quantityText.text = "0";
            productQuantities[product] = 0;

            // Привязываем кнопки для добавления и уменьшения количества
            Button addButton = item.transform.Find("AddButton").GetComponent<Button>();
            Button subtractButton = item.transform.Find("SubtractButton").GetComponent<Button>();

            addButton.onClick.AddListener(() => ChangeQuantity(product, quantityText, 1));
            subtractButton.onClick.AddListener(() => ChangeQuantity(product, quantityText, -1));
        }
    }

    void ChangeQuantity(Product product, TextMeshProUGUI quantityText, int change)
    {
        // Обновляем количество товара
        productQuantities[product] += change;

        // Не допускаем отрицательных значений
        if (productQuantities[product] < 0)
        {
            productQuantities[product] = 0;
        }

        // Обновляем текст с количеством
        quantityText.text = productQuantities[product].ToString();

        // Обновляем общую сумму
        UpdateTotal();
    }

    void UpdateTotal()
    {
        int totalItems = 0;
        float totalPrice = 0;

        foreach (var product in products)
        {
            totalItems += productQuantities[product];
            totalPrice += productQuantities[product] * product.price;
        }

        // Обновляем текст на панели
        totalItemsText.text = $"Total Items: {totalItems}";
        totalPriceText.text = $"Total Price: ${totalPrice:F2}";
    }

    void ToggleProductPanel()
    {
        bool isActive = productPanel.activeSelf;

        // Переключение состояния панели
        productPanel.SetActive(!isActive);

        // Выводим сообщение в консоль
        Debug.Log(isActive ? "Product Panel Hidden" : "Product Panel Shown");
    }

    public void ClosePanelButton()
    {
        productPanel.SetActive(false);
        Debug.Log("Product Panel Closed via Button");
    }
}

[System.Serializable]
public class Product
{
    public string name;
    public float price;
}
