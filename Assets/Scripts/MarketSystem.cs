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
    public Button placeOrderButton;       // Кнопка "Place Order"
    public TMP_Text closePanelButtonText; // Текст на кнопке закрытия панели (если нужно)

    [Header("Product Data")]
    public Product[] products; // Список доступных товаров

    [Header("Box Settings")]
    public Transform boxSpawnPoint;       // Точка спауна коробок
    public GameObject boxPrefab;          // Префаб коробки

    private Dictionary<Product, int> productQuantities = new Dictionary<Product, int>();

    void Start()
    {
        PopulateProductList();
        UpdateTotal();

        // Скрываем панель товаров в начале
        productPanel.SetActive(false);

        // Привязываем обработчик к кнопке "Place Order"
        placeOrderButton.onClick.AddListener(PlaceOrder);
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

            // Устанавливаем картинку товара
            Image productIcon = item.transform.Find("ProductIcon").GetComponent<Image>();
            if (product.productIcon != null)
            {
                productIcon.sprite = product.productIcon;
            }

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
        productQuantities[product] += change;

        if (productQuantities[product] < 0)
        {
            productQuantities[product] = 0;
        }

        quantityText.text = productQuantities[product].ToString();
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

        totalItemsText.text = $"Total Items: {totalItems}";
        totalPriceText.text = $"Total Price: ${totalPrice:F2}";
    }

    void PlaceOrder()
    {
        foreach (var product in products)
        {
            int quantity = productQuantities[product];

            // Спауним коробки для каждого купленного товара
            for (int i = 0; i < quantity; i++)
            {
                SpawnBox(product.productIcon);
            }
        }

        // После заказа сбрасываем количество товаров
        ResetQuantities();
        UpdateTotal();

        Debug.Log("Order placed! Boxes have been spawned.");
    }


    void SpawnBox(Sprite productIcon)
    {
        Debug.Log("Attempting to spawn box...");

        // Создание коробки
        GameObject box = Instantiate(boxPrefab, boxSpawnPoint.position, Quaternion.identity);

        // Поиск BoxIcon внутри коробки
        Image boxIcon = box.GetComponentInChildren<Image>();

        if (boxIcon == null)
        {
            Debug.LogError("BoxIcon Image component not found in the box prefab!");
            return;
        }

        // Установка иконки товара
        boxIcon.sprite = productIcon;
        Debug.Log("Box spawned with icon: " + productIcon.name);
    }



    void ResetQuantities()
    {
        foreach (var product in products)
        {
            productQuantities[product] = 0;
        }

        // Обновляем UI
        foreach (Transform child in contentParent)
        {
            TextMeshProUGUI quantityText = child.Find("QuantityText").GetComponent<TextMeshProUGUI>();
            quantityText.text = "0";
        }
    }

    void ToggleProductPanel()
    {
        bool isActive = productPanel.activeSelf;
        productPanel.SetActive(!isActive);
    }
}

[System.Serializable]
public class Product
{
    public string name;
    public float price;
    public Sprite productIcon; // Иконка продукта
}
