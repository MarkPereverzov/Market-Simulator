using UnityEngine;

[System.Serializable]
public class Product
{
    public string productName;      // Имя продукта
    public float price;             // Цена продукта
    public Sprite productIcon;      // Иконка продукта
    public License requiredLicense; // Лицензия, необходимая для покупки товара
}
