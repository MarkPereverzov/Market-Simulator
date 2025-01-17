using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Shelf
{
    [SerializeField] public Product Product { get; private set; }
    public List<ProductView> ProductList { get; } = new List<ProductView>();
    [SerializeField] public int MaxQuantity { get; } = 12;

    public int CurrentQuantity => ProductList.Count;
    // Событие, вызываемое при изменении содержимого полки
    public event Action OnShelfContentChanged;
    public event Action OnShelfCarryProductChanged;

    public Shelf(Product product = default) 
    {
        if (product == default)
            product = null;
        else 
            Product = product;
        OnShelfCarryProductChanged?.Invoke();
    }

    /// <summary>
    /// Добавляет продукт на полку.
    /// </summary>
    /// <param name="product">Продукт, который нужно добавить.</param>
    /// <returns>Возвращает true, если продукт добавлен успешно, иначе false.</returns>
    public bool AddProduct(ProductView product)
    {
        if (ProductList.Count >= MaxQuantity)
        {
            Debug.LogWarning("Полка заполнена! Невозможно добавить продукт.");
            return false;
        }

        ProductList.Add(product);
        Debug.Log($"Продукт {product.name} добавлен на полку.");
        OnShelfContentChanged?.Invoke(); // Вызов события
        return true;
    }

    /// <summary>
    /// Удаляет продукт с полки.
    /// </summary>
    /// <param name="product">Продукт, который нужно удалить.</param>
    /// <returns>Возвращает true, если продукт удалён успешно, иначе false.</returns>
    public bool RemoveProduct(ProductView product)
    {
        if (ProductList.Contains(product))
        {
            ProductList.Remove(product);
            Debug.Log($"Продукт {product.name} удалён с полки.");
            OnShelfContentChanged?.Invoke(); // Вызов события
            return true;
        }

        Debug.LogWarning("Продукт не найден на полке!");
        return false;
    }
    public bool ClearProducts()
    {
        ProductList.Clear();
        Debug.Log($"Продукты удалённы с полки.");
        OnShelfContentChanged?.Invoke(); // Вызов события
        return true;
    }
}
