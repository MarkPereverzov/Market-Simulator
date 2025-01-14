using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LicensesManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject licenseItemPrefab;    // ������ �������� ��������
    public Transform contentParent;         // ������������ ������ ��� ������ ��������

    [Header("License List")]
    public License[] licenses;              // ������ ��������� ��������

    void Start()
    {
        PopulateLicenseList();
    }

    // ���������� ������ ��������
    void PopulateLicenseList()
    {
        foreach (var license in licenses)
        {
            GameObject item = Instantiate(licenseItemPrefab, contentParent);

            item.transform.Find("LicenseName").GetComponent<TextMeshProUGUI>().text = license.name;
            item.transform.Find("LicensePrice").GetComponent<TextMeshProUGUI>().text = $"${license.price:F2}";

            Image licenseIcon = item.transform.Find("LicenseIcon").GetComponent<Image>();
            if (license.icon != null)
            {
                licenseIcon.sprite = license.icon;
            }

            Button buyButton = item.transform.Find("BuyButton").GetComponent<Button>();
            buyButton.onClick.AddListener(() => PurchaseLicense(license));
        }
    }

    // ������� ��������
    // ������� ��������
    void PurchaseLicense(License license)
    {
        if (UIManager.Instance.SpendMoney(license.price))
        {
            Debug.Log($"License {license.name} purchased!");
        }
        else
        {
            Debug.Log("Not enough money to purchase this license!");
        }
    }

}

[System.Serializable]
public class License
{
    public string name;         // �������� ��������
    public float price;         // ���� ��������
    public Sprite icon;         // ������ ��������
}
