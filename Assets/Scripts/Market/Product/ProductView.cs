using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductView : MonoBehaviour
{
    [SerializeField] private Product _product;
    public void Init(Product product)
    {
        _product = product;
        UpdateView();
    }

    private void UpdateView()
    {
        transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = _product.productName;
        transform.GetChild(0).GetComponentInChildren<Image>().sprite = _product.productIcon ?? null;
    }

    void Start()
    {
        if (_product == null)
        {
            Debug.LogWarning("ProductView: Product not initialized!");
        }
    }
}
