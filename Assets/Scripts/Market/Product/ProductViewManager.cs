using UnityEngine;

public class ProductViewManager : MonoBehaviour
{
    [SerializeField] private GameObject productViewPrefab;
    [SerializeField] private Transform parent;
    [SerializeField] private Transform boxSpawnPoint;

    public void CreateProductView(Product product)
    {
        var instance = Instantiate(productViewPrefab, boxSpawnPoint.position, Quaternion.identity);
        var productView = instance.GetComponent<ProductView>();
        productView.Init(product);
    }
}