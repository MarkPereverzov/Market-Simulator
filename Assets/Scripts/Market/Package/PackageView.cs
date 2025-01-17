using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PackageView : MonoBehaviour
{
    [SerializeField] private Package _package;

    public void Init(Package package)
    {
        _package = package;
        UpdateView();
    }

    private void UpdateView()
    {
        transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = _package.Quantity.ToString();
        transform.GetChild(0).GetComponentInChildren<Image>().sprite = _package.Product.productIcon ?? null;
    }

    void Start()
    {
        if (_package == null)
        {
            Debug.LogWarning("ProductView: Product not initialized!");
            return;
        }
    }

    public void Unpack() 
    {
        for (int i = 0; i < _package.Quantity; i++)
        {
            ProductViewManager.Instance.CreateProductView(_package.Product, transform.position);
        }
        Destroy(gameObject);
    }
}
