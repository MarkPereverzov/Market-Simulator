using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ProductManager : MonoBehaviour
{
    [Header("UI Text")]
    public TextMeshProUGUI totalItemsText;
    public TextMeshProUGUI totalPriceText;

    [Header("UI Buttons")]
    public Button placeOrderButton;

    [Header("Product")]
    public GameObject productItemPrefab;
    public Transform contentParent;

    [Header("Product List")]
    public Product[] products;

    [Header("Product Box")]
    public Transform boxSpawnPoint;
    public GameObject boxPrefab;

    private Dictionary<Product, int> productQuantities = new Dictionary<Product, int>();

    void Start()
    {
        PopulateProductList();
        UpdateTotal();

        placeOrderButton.onClick.AddListener(PlaceOrder);
    }

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

        placeOrderButton.interactable = totalPrice <= UIManager.Instance.GetCurrentMoney() && totalPrice > 0;
    }
    void PlaceOrder()
    {
        float totalPrice = 0;
        foreach (var product in products)
        {
            totalPrice += productQuantities[product] * product.price;
        }

        if (totalPrice > UIManager.Instance.GetCurrentMoney())
        {
            Debug.Log("Not enough money!");
            return;
        }

        foreach (var product in products)
        {
            int quantity = productQuantities[product];
            for (int i = 0; i < quantity; i++)
            {
                SpawnBox(product.productIcon);
            }
        }

        UIManager.Instance.SpendMoney(totalPrice);
        ResetQuantities();
        UpdateTotal();
    }

    void SpawnBox(Sprite productIcon)
    {
        GameObject box = Instantiate(boxPrefab, boxSpawnPoint.position, Quaternion.identity);
        Image boxIcon = box.GetComponentInChildren<Image>();

        if (boxIcon != null)
        {
            boxIcon.sprite = productIcon;
        }
    }

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