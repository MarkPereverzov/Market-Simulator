using UnityEngine;

[System.Serializable]
public class Product
{
    public string productName;
    public float price;
    public Sprite productIcon;

    public int requiredLicenseId;
    public bool isUnlocked;
}
