using UnityEngine;

public class ProductViewManager : MonoBehaviour
{
    public static ProductViewManager Instance { get; private set; }

    [SerializeField] private GameObject productViewPrefab;
    [SerializeField] private Transform boxSpawnPoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("������� ������� ������ ��������� ProductViewManager. ��������� ������ ������.");
            Destroy(gameObject);
        }
    }
    public ProductView CreateProductView(Product product, Vector3 position = default)
    {
        if (position == default)
        {
            position = boxSpawnPoint.position;
        }
        if (productViewPrefab == null)
        {
            Debug.LogError("������ �� �����!");
            return null;
        }
        var instance = Instantiate(productViewPrefab, position, Quaternion.identity);
        var productView = instance.GetComponent<ProductView>();
        if (productView != null)
        {
            productView.Init(product);
            return productView;
        }
        else
        {
            Debug.LogError("�� ������� ����������� ��������� ProductView!");
        }
        return null;
    }
    void Start()
    {
        if (productViewPrefab == null)
        {
            productViewPrefab = Resources.Load<GameObject>("Prefabs/Box");
        }
        if (productViewPrefab != null)
        {
            Debug.Log("������ ������� ��������");
        }
        else
        {
            Debug.LogError("�� ������� ��������� ������");
        }
    }
}