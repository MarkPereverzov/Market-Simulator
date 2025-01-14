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
    public Button placeOrderButton;       // ������ "Place Order"
    public TMP_Text closePanelButtonText; // ����� �� ������ �������� ������ (���� �����)

    [Header("Product Data")]
    public Product[] products; // ������ ��������� �������

    [Header("Box Settings")]
    public Transform boxSpawnPoint;       // ����� ������ �������
    public GameObject boxPrefab;          // ������ �������

    private Dictionary<Product, int> productQuantities = new Dictionary<Product, int>();

    void Start()
    {
        PopulateProductList();
        UpdateTotal();

        // �������� ������ ������� � ������
        productPanel.SetActive(false);

        // ����������� ���������� � ������ "Place Order"
        placeOrderButton.onClick.AddListener(PlaceOrder);
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

            // ������������� �������� ������
            Image productIcon = item.transform.Find("ProductIcon").GetComponent<Image>();
            if (product.productIcon != null)
            {
                productIcon.sprite = product.productIcon;
            }

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

            // ������� ������� ��� ������� ���������� ������
            for (int i = 0; i < quantity; i++)
            {
                SpawnBox(product.productIcon);
            }
        }

        // ����� ������ ���������� ���������� �������
        ResetQuantities();
        UpdateTotal();

        Debug.Log("Order placed! Boxes have been spawned.");
    }


    void SpawnBox(Sprite productIcon)
    {
        Debug.Log("Attempting to spawn box...");

        // �������� �������
        GameObject box = Instantiate(boxPrefab, boxSpawnPoint.position, Quaternion.identity);

        // ����� BoxIcon ������ �������
        Image boxIcon = box.GetComponentInChildren<Image>();

        if (boxIcon == null)
        {
            Debug.LogError("BoxIcon Image component not found in the box prefab!");
            return;
        }

        // ��������� ������ ������
        boxIcon.sprite = productIcon;
        Debug.Log("Box spawned with icon: " + productIcon.name);
    }



    void ResetQuantities()
    {
        foreach (var product in products)
        {
            productQuantities[product] = 0;
        }

        // ��������� UI
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
    public Sprite productIcon; // ������ ��������
}
