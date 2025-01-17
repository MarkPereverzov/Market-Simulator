using System;
using Unity.VisualScripting;
using UnityEngine;

public class Package
{
    private Product carriedProduct;
    private int quantity;

    public Product Product { get => carriedProduct; }
    public int Quantity { get => quantity; set { quantity = value; OnPackagePropertyChanged?.Invoke(); } }

    public event Action OnPackagePropertyChanged;
    public Package (Product carriedProduct, int quantity)
    {
        this.carriedProduct = carriedProduct;
        this.quantity = quantity;
    }

    public override string ToString()
    {
        return $"{carriedProduct} {quantity}";
    }
}
