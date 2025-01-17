using UnityEngine;

[System.Serializable]
public class Product
{
    public string productName;
    public float price;
    public Sprite productIcon;

    public int requiredLicenseId;
    public bool isUnlocked;

    public Product(string productName, float price, Sprite productIcon, int requiredLicenseId, bool isUnlocked)
    {
        this.productName = productName;
        this.price = price;
        this.productIcon = productIcon;
        this.requiredLicenseId = requiredLicenseId;
        this.isUnlocked = isUnlocked;
    }

    public override string ToString()
    {
        return $"Name: {productName}\nPrice: {price}\nUnlocked: {isUnlocked}";
    }
}
