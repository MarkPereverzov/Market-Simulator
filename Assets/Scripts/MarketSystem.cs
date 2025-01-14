using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ProductManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject marketSystemPanel;    // ������ Market System
    public GameObject productPanel;         // ������ �������
    public GameObject licensesPanel;        // ������ ��������

    [Header("UI Text")]
    public TextMeshProUGUI totalItemsText;  // ����� ��� ����������� ������ ���������� �������
    public TextMeshProUGUI totalPriceText;  // ����� ��� ����������� ����� �����
    public TextMeshProUGUI moneyText;       // ����� ��� ����������� ������� �����

    [Header("UI Buttons")]
    public Button licensesButton;           // ������ ��� ����������� ������ ��������
    public Button productsButton;           // ������ ��� ����������� ������ �������
    public Button placeOrderButton;         // ������ "Place Order"

    [Header("Product")]
    public GameObject productItemPrefab;    // ������ ��� ������
    public Transform contentParent;         // ��������� ������� � ScrollView

    [Header("Product List")]
    public Product[] products;              // ������ ��������� �������

    [Header("Product Box")]
    public Transform boxSpawnPoint;         // ����� ������ �������
    public GameObject boxPrefab;            // ������ �������



    private Dictionary<Product, int> productQuantities = new Dictionary<Product, int>();
    private float currentMoney = 100f;    // ��������� ����� �����
    private bool isMarketSystemActive = false;  // ���� ��� ������������ ��������� ������ Market System

    void Start()
    {
        PopulateProductList();
        UpdateTotal();
        UpdateMoneyUI();

        marketSystemPanel.SetActive(false);
        licensesPanel.SetActive(false);  // ���������� ������ ������ ��������

        placeOrderButton.onClick.AddListener(PlaceOrder);

        // ��������� ������� ��� ������ ������������ �������
        licensesButton.onClick.AddListener(() => TogglePanel(licensesPanel, productPanel));
        productsButton.onClick.AddListener(() => TogglePanel(productPanel, licensesPanel));
    }

    void Update()
    {
        // ��� ������� �� ������� B ����������/�������� ������ Market System
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleMarketSystemPanel();
        }
    }

    // ������������ ��������� ������ Market System
    void ToggleMarketSystemPanel()
    {
        isMarketSystemActive = !isMarketSystemActive;  // ����������� ��������� �����
        marketSystemPanel.SetActive(isMarketSystemActive); // ���������� ��� �������� ������

        if (isMarketSystemActive)
        {
            // ��� ��������� Market System ���������� �������� � �������� ��������
            productPanel.SetActive(true);
            licensesPanel.SetActive(false);
        }
        else
        {
            // ��� ����������� Market System �������� ��� �������� ������
            productPanel.SetActive(false);
            licensesPanel.SetActive(false);
        }
    }

    // ������� ��� ������������ ����� ��������
    void TogglePanel(GameObject panelToShow, GameObject panelToHide)
    {
        panelToShow.SetActive(true);  // ���������� ������ ������
        panelToHide.SetActive(false); // �������� ������ ������
    }

    // ��������� ������ �������
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

    // ��������� ���������� ������
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

    // ���������� ������ ���������� ������� � ����� �����
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

    // ���������� ������
    void PlaceOrder()
    {
        float totalPrice = 0;
        foreach (var product in products)
        {
            totalPrice += productQuantities[product] * product.price;
        }

        if (totalPrice > currentMoney)
        {
            return; // �� ������� ����� �� �����
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

    // ����� �������
    void SpawnBox(Sprite productIcon)
    {
        GameObject box = Instantiate(boxPrefab, boxSpawnPoint.position, Quaternion.identity);
        Image boxIcon = box.GetComponentInChildren<Image>();

        if (boxIcon != null)
        {
            boxIcon.sprite = productIcon;
        }
    }

    // ����� ��������� �������
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

    // ���������� ����������� ������� �����
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
    public Sprite productIcon; // ������ ��������
}