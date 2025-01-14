using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ProductManager : MonoBehaviour
{
    [Header("UI Text")]
    public TextMeshProUGUI totalItemsText;  // Текст для отображения общего количества товаров
    public TextMeshProUGUI totalPriceText;  // Текст для отображения общей суммы

    [Header("UI Buttons")]
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

    void Start()
    {
        PopulateProductList();
        UpdateTotal();

        // Привязываем функцию PlaceOrder к кнопке
        placeOrderButton.onClick.AddListener(PlaceOrder);
    }

    // Заполнение списка товаров
    void PopulateProductList()
    {
        foreach (var product in products)
        {
            GameObject item = Instantiate(productItemPrefab, contentParent);

            item.transform.Find("ProductName").GetComponent<TextMeshProUGUI>().text = product.productName;
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

        // Проверяем, можно ли оформить заказ
        placeOrderButton.interactable = totalPrice <= UIManager.Instance.GetCurrentMoney() && totalPrice > 0;
    }

    // Покупка товаров (оформление заказа)
    void PlaceOrder()
    {
        // Рассчитываем общую стоимость всех товаров
        float totalPrice = 0;
        foreach (var product in products)
        {
            totalPrice += productQuantities[product] * product.price;
        }

        // Проверяем, хватает ли денег
        if (totalPrice > UIManager.Instance.GetCurrentMoney())
        {
            Debug.Log("Not enough money!");
            return;  // Прерываем выполнение, если денег недостаточно
        }

        // Создаем коробки для всех товаров в корзине
        foreach (var product in products)
        {
            int quantity = productQuantities[product];
            for (int i = 0; i < quantity; i++)
            {
                SpawnBox(product.productIcon);  // Создаем коробку с товаром
            }
        }

        // Списываем деньги с баланса игрока
        UIManager.Instance.SpendMoney(totalPrice);

        // Сбрасываем количество товаров
        ResetQuantities();

        // Обновляем UI
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
}

[System.Serializable]
public class Product
{
    public string productName;  // Имя продукта
    public float price;         // Цена продукта
    public Sprite productIcon;  // Иконка продукта
}
