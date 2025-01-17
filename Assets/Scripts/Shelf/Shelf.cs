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
    // �������, ���������� ��� ��������� ����������� �����
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
    /// ��������� ������� �� �����.
    /// </summary>
    /// <param name="product">�������, ������� ����� ��������.</param>
    /// <returns>���������� true, ���� ������� �������� �������, ����� false.</returns>
    public bool AddProduct(ProductView product)
    {
        if (ProductList.Count >= MaxQuantity)
        {
            Debug.LogWarning("����� ���������! ���������� �������� �������.");
            return false;
        }

        ProductList.Add(product);
        Debug.Log($"������� {product.name} �������� �� �����.");
        OnShelfContentChanged?.Invoke(); // ����� �������
        return true;
    }

    /// <summary>
    /// ������� ������� � �����.
    /// </summary>
    /// <param name="product">�������, ������� ����� �������.</param>
    /// <returns>���������� true, ���� ������� ����� �������, ����� false.</returns>
    public bool RemoveProduct(ProductView product)
    {
        if (ProductList.Contains(product))
        {
            ProductList.Remove(product);
            Debug.Log($"������� {product.name} ����� � �����.");
            OnShelfContentChanged?.Invoke(); // ����� �������
            return true;
        }

        Debug.LogWarning("������� �� ������ �� �����!");
        return false;
    }
    public bool ClearProducts()
    {
        ProductList.Clear();
        Debug.Log($"�������� ������� � �����.");
        OnShelfContentChanged?.Invoke(); // ����� �������
        return true;
    }
}
