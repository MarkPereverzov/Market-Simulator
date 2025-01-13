using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ProductManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject productItemPrefab;  // ������ ��� ������
    public Transform contentParent;       // ��������� ������� � ScrollView
    public TextMeshProUGUI totalItemsText; // ����� ��� ����������� ������ ���������� �������
    public TextMeshProUGUI totalPriceText; // ����� ��� ����������� ����� �����
    public GameObject productPanel;       // ������ �������
    public TMP_Text closePanelButtonText; // ����� �� ������ �������� ������ (���� �����)

    [Header("Product Data")]
    public Product[] products; // ������ ��������� �������

    private Dictionary<Product, int> productQuantities = new Dictionary<Product, int>();

    void Start()
    {
        PopulateProductList();
        UpdateTotal();

        // �������� ������ ������� � ������
        productPanel.SetActive(false);
    }

    void Update()
    {
        // ��������/�������� ������ �� ������� ������� B
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleProductPanel();
        }
    }

    void PopulateProductList()
    {
        foreach (var product in products)
        {
            // ������ ��������� ������� ��� ������
            GameObject item = Instantiate(productItemPrefab, contentParent);

            // ������������� �������� ������
            item.transform.Find("ProductName").GetComponent<TextMeshProUGUI>().text = product.name;

            // ������������� ���� ������
            item.transform.Find("ProductPrice").GetComponent<TextMeshProUGUI>().text = $"${product.price:F2}";

            // �������������� ���������� ������
            TextMeshProUGUI quantityText = item.transform.Find("QuantityText").GetComponent<TextMeshProUGUI>();
            quantityText.text = "0";
            productQuantities[product] = 0;

            // ����������� ������ ��� ���������� � ���������� ����������
            Button addButton = item.transform.Find("AddButton").GetComponent<Button>();
            Button subtractButton = item.transform.Find("SubtractButton").GetComponent<Button>();

            addButton.onClick.AddListener(() => ChangeQuantity(product, quantityText, 1));
            subtractButton.onClick.AddListener(() => ChangeQuantity(product, quantityText, -1));
        }
    }

    void ChangeQuantity(Product product, TextMeshProUGUI quantityText, int change)
    {
        // ��������� ���������� ������
        productQuantities[product] += change;

        // �� ��������� ������������� ��������
        if (productQuantities[product] < 0)
        {
            productQuantities[product] = 0;
        }

        // ��������� ����� � �����������
        quantityText.text = productQuantities[product].ToString();

        // ��������� ����� �����
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

        // ��������� ����� �� ������
        totalItemsText.text = $"Total Items: {totalItems}";
        totalPriceText.text = $"Total Price: ${totalPrice:F2}";
    }

    void ToggleProductPanel()
    {
        bool isActive = productPanel.activeSelf;

        // ������������ ��������� ������
        productPanel.SetActive(!isActive);

        // ������� ��������� � �������
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
