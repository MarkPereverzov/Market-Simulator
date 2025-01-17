using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Threading.Tasks;


public class ProductManager : MonoBehaviour
{
    [Header("Package View Manager")]
    [SerializeField] private PackageViewManager packageViewManager;

    [Header("UI Text")]
    public TextMeshProUGUI totalItemsText;
    public TextMeshProUGUI totalPriceText;

    [Header("UI Buttons")]
    public Button placeOrderButton;

    [Header("Product")]
    public GameObject productItemPrefab;
    public GameObject productItemLockedPrefab;
    public Transform contentParent;

    [Header("Lists")]
    public ProductList productList;
    public LicenseList licenseList;

    [Header("Product Box")]
    public Transform boxSpawnPoint;
    public GameObject boxPrefab;

    private Dictionary<Product, int> productQuantities = new Dictionary<Product, int>();
    private Dictionary<Product, GameObject> productItems = new Dictionary<Product, GameObject>();

    void Start()
    {
        PopulateProductList();
        UpdateTotal();

        placeOrderButton.onClick.AddListener(PlaceOrder);
    }

    void PopulateProductList()
    {
        foreach (var product in productList.products)
        {
            if (!productItems.ContainsKey(product))
            {
                GameObject item;

                // Инициализируем количество товара
                if (!productQuantities.ContainsKey(product))
                {
                    productQuantities[product] = 0;
                }

                if (product.isUnlocked)
                {
                    item = Instantiate(productItemPrefab, contentParent);
                    ConfigureUnlockedProductItem(item, product);
                }
                else
                {
                    item = Instantiate(productItemLockedPrefab, contentParent);
                    ConfigureLockedProductItem(item, product);
                }

                productItems[product] = item;
            }
            else
            {
                UpdateProductItem(product);
            }
        }
    }

    void ConfigureUnlockedProductItem(GameObject item, Product product)
    {
        item.transform.Find("ProductName").GetComponent<TextMeshProUGUI>().text = product.productName;
        item.transform.Find("ProductPrice").GetComponent<TextMeshProUGUI>().text = $"${product.price:F2}";

        Image productIcon = item.transform.Find("ProductIcon").GetComponent<Image>();
        if (product.productIcon != null)
        {
            productIcon.sprite = product.productIcon;
        }

        // Обновляем текст количества товаров в упаковке
        //TextMeshProUGUI quantityPerBoxText = item.transform.Find("ItemsPerBoxText").GetComponent<TextMeshProUGUI>();
        //quantityPerBoxText.text = $"{product.itemsPerBox} pcs";

        TextMeshProUGUI quantityText = item.transform.Find("QuantityText").GetComponent<TextMeshProUGUI>();
        quantityText.text = productQuantities[product].ToString();

        Button addButton = item.transform.Find("AddButton").GetComponent<Button>();
        Button subtractButton = item.transform.Find("SubtractButton").GetComponent<Button>();

        addButton.onClick.AddListener(() => ChangeQuantity(product, quantityText, 1));
        subtractButton.onClick.AddListener(() => ChangeQuantity(product, quantityText, -1));
    }



    void ConfigureLockedProductItem(GameObject item, Product product)
    {
        item.transform.Find("ProductName").GetComponent<TextMeshProUGUI>().text = product.productName;

        // Дополнительная логика для заблокированных товаров
    }
    public void UpdateProductItem(Product product)
    {
        if (productItems.ContainsKey(product))
        {
            GameObject item = productItems[product];

            if (product.isUnlocked)
            {
                Destroy(item); // Удаляем старый префаб
                GameObject newItem = Instantiate(productItemPrefab, contentParent);
                ConfigureUnlockedProductItem(newItem, product);
                productItems[product] = newItem; // Обновляем ссылку
            }
        }
    }

    void ChangeQuantity(Product product, TextMeshProUGUI quantityText, int change)
    {
        // Проверка на наличие лицензии
        if (product.requiredLicenseId != 0)
        {
            License requiredLicense = GetLicenseById(product.requiredLicenseId);
            if (requiredLicense == null || !requiredLicense.isPurchased)
            {
                Debug.LogWarning($"License with ID {product.requiredLicenseId} is required to add this product.");
                return;
            }
        }

        // Обновление количества товара в словаре
        productQuantities[product] = Mathf.Max(0, productQuantities[product] + change);

        // Обновление текста в UI
        quantityText.text = productQuantities[product].ToString();

        // Обновление общей суммы
        UpdateTotal();
    }


    License GetLicenseById(int id)
    {
        foreach (var license in licenseList.licenses)
        {
            if (license.id == id)
            {
                return license;
            }
        }
        return null;
    }
    void UpdateTotal()
    {
        int totalItems = 0;
        float totalPrice = 0;

        foreach (var product in productList.products)
        {
            if (productQuantities.ContainsKey(product)) // Проверяем, что товар существует в корзине
            {
                totalItems += productQuantities[product];
                totalPrice += productQuantities[product] * product.price;
            }
        }

        totalItemsText.text = $"Total Items: {totalItems}";
        totalPriceText.text = $"Total Price: ${totalPrice:F2}";

        placeOrderButton.interactable = totalPrice <= PlayerManager.Instance.GetCurrentMoney() && totalPrice > 0;
    }


    async void PlaceOrder()
    {
        float totalPrice = 0;

        // Подсчет общей стоимости заказа
        foreach (var product in productList.products)
        {
            int boxCount = productQuantities[product]; // Количество коробок
            totalPrice += boxCount * product.price; // Цена за коробки
        }

        // Проверка, хватает ли денег
        if (totalPrice > PlayerManager.Instance.GetCurrentMoney())
        {
            Debug.Log("Not enough money!");
            return;
        }

        // Создание коробок для каждого товара
        foreach (var product in productList.products)
        {
            int boxCount = productQuantities[product];
            SpawnBox(product, boxCount);
            await Task.Delay(300);
        }

        // Списание денег
        PlayerManager.Instance.SpendMoney(totalPrice);

        // Сброс количества коробок
        ResetQuantities();

        // Обновление UI
        UpdateTotal();
    }
    void SpawnBox(Product product, int quantity)
    {
        if (quantity == 0) return;
 
        Package newPackage = new Package(product, quantity);
        Debug.Log(newPackage);
        packageViewManager.CreatePackageView(newPackage);
    }
    void ResetQuantities()
    {
        foreach (var product in productList.products)
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