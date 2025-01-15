using UnityEngine;

[CreateAssetMenu(fileName = "NewProductList", menuName = "Market Simulator/Product List")]
public class ProductList : ScriptableObject
{
    public Product[] products;
}
