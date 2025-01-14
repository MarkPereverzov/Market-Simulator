using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ProductManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject marketSystemPanel;    // Панель Market System
    public GameObject productPanel;         // Панель товаров
    public GameObject licensesPanel;        // Панель лицензий

    [Header("UI Text")]
    public TextMeshProUGUI totalItemsText;  // Текст для отображения общего количества товаров
    public TextMeshProUGUI totalPriceText;  // Текст для отображения общей суммы
    public TextMeshProUGUI moneyText;       // Текст для отображения текущих денег

    [Header("UI Buttons")]
    public Button licensesButton;           // Кнопка для отображения панели лицензий
    public Button productsButton;           // Кнопка для отображения панели товаров
    public Button placeOrderButton;         // Кнопка "Place Order"

    [Header("Product")]
    public GameObject productItemPrefab;    // Префаб для товара
    public Transform contentParent;         // Контейнер товаров в ScrollView

    [Header("Product List")]
    public Product[] products;              // Список доступных товаров

    [Header("Product Box")]
    public Transform boxSpawnPoint;         // Точка спауна коробок
    public GameObject boxPrefab;            // Префаб коробки



    private Dictionary<Product, int> productQuantities = new Dictionary<Product, int>();
    private float currentMoney = 100f;    // Стартовая сумма денег
    private bool isMarketSystemActive = false;  // Флаг для отслеживания состояния панели Market System

    void Start()
    {
        PopulateProductList();
        UpdateTotal();
        UpdateMoneyUI();

        marketSystemPanel.SetActive(false);
        licensesPanel.SetActive(false);  // Изначально скрыть панель лицензий

        placeOrderButton.onClick.AddListener(PlaceOrder);

        // Добавляем события для кнопок переключения панелей
        licensesButton.onClick.AddListener(() => TogglePanel(licensesPanel, productPanel));
        productsButton.onClick.AddListener(() => TogglePanel(productPanel, licensesPanel));
    }

    void Update()
    {
        // При нажатии на клавишу B отображаем/скрываем панель Market System
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleMarketSystemPanel();
        }
    }

    // Переключение состояния панели Market System
    void ToggleMarketSystemPanel()
    {
        isMarketSystemActive = !isMarketSystemActive;  // Инвертируем состояние флага
        marketSystemPanel.SetActive(isMarketSystemActive); // Показываем или скрываем панель

        if (isMarketSystemActive)
        {
            // При активации Market System показываем продукты и скрываем лицензии
            productPanel.SetActive(true);
            licensesPanel.SetActive(false);
        }
        else
        {
            // При деактивации Market System скрываем все дочерние панели
            productPanel.SetActive(false);
            licensesPanel.SetActive(false);
        }
    }

    // Функция для переключения между панелями
    void TogglePanel(GameObject panelToShow, GameObject panelToHide)
    {
        panelToShow.SetActive(true);  // Показываем нужную панель
        panelToHide.SetActive(false); // Скрываем другую панель
    }

    // Заполняем список товаров
    void PopulateProductList()
    {
        foreach (var product in products)
        {
            GameObject item = Instantiate(productItemPrefab, contentParent);

            item.transform.Find("ProductName").GetComponent<TextMeshProUGUI>().text = product.name;
            item.transform.Find("ProductPrice").GetComponent<TextMeshProUGUI>().text = $"${product.price:F2}";

            Image productIcon = item.transform.Find("ProductIcon").GetComponent<Image>();
            if (product.productIcon != null)
            {
                productIcon.sprite = product.productIcon;
            }

            TextMeshProUGUI quantityText = item.transform.Find("QuantityText").GetComponent<TextMeshProUGUI>();
            quantityText.text = "0";
            productQuantities[product] = 0;

            Button addButton = item.transform.Find("AddButton").GetComponent<Button>();
            Button subtractButton = item.transform.Find("SubtractButton").GetComponent<Button>();

            addButton.onClick.AddListener(() => ChangeQuantity(product, quantityText, 1));
            subtractButton.onClick.AddListener(() => ChangeQuantity(product, quantityText, -1));
        }
    }

    // Изменение количества товара
    void ChangeQuantity(Product product, TextMeshProUGUI quantityText, int change)
    {
        productQuantities[product] += change;

        if (productQuantities[product] < 0)
        {
            productQuantities[product] = 0;
        }

        quantityText.text = productQuantities[product].ToString();
        UpdateTotal();
    }

    // Обновление общего количества товаров и общей суммы
    void UpdateTotal()
    {
        int totalItems = 0;
        float totalPrice = 0;

        foreach (var product in products)
        {
            totalItems += productQuantities[product];
            totalPrice += productQuantities[product] * product.price;
        }

        totalItemsText.text = $"Total Items: {totalItems}";
        totalPriceText.text = $"Total Price: ${totalPrice:F2}";

        placeOrderButton.interactable = totalPrice <= currentMoney && totalPrice > 0;
    }

    // Оформление заказа
    void PlaceOrder()
    {
        float totalPrice = 0;
        foreach (var product in products)
        {
            totalPrice += productQuantities[product] * product.price;
        }

        if (totalPrice > currentMoney)
        {
            return; // Не хватает денег на заказ
        }

        foreach (var product in products)
        {
            int quantity = productQuantities[product];

            for (int i = 0; i < quantity; i++)
            {
                SpawnBox(product.productIcon);
            }
        }

        currentMoney -= totalPrice;
        UpdateMoneyUI();

        ResetQuantities();
        UpdateTotal();
    }

    // Спавн коробки
    void SpawnBox(Sprite productIcon)
    {
        GameObject box = Instantiate(boxPrefab, boxSpawnPoint.position, Quaternion.identity);
        Image boxIcon = box.GetComponentInChildren<Image>();

        if (boxIcon != null)
        {
            boxIcon.sprite = productIcon;
        }
    }

    // Сброс количеств товаров
    void ResetQuantities()
    {
        foreach (var product in products)
        {
            productQuantities[product] = 0;
        }

        foreach (Transform child in contentParent)
        {
            TextMeshProUGUI quantityText = child.Find("QuantityText").GetComponent<TextMeshProUGUI>();
            quantityText.text = "0";
        }
    }

    // Обновление отображения текущих денег
    void UpdateMoneyUI()
    {
        moneyText.text = $"Money: ${currentMoney:F2}";
    }
}

[System.Serializable]
public class Product
{
    public string name;
    public float price;
    public Sprite productIcon; // Иконка продукта
}