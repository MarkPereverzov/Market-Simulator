using UnityEngine;

public class PackageViewManager : MonoBehaviour
{
    public static PackageViewManager Instance { get; private set; }

    [SerializeField] private GameObject packageViewPrefab;
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
            Debug.LogWarning("������� ������� ������ ��������� PackageViewManager. ��������� ������ ������.");
            Destroy(gameObject);
        }
    }

    public GameObject CreatePackageView(Package package)
    {
        var instance = Instantiate(packageViewPrefab, boxSpawnPoint.position, Quaternion.identity);
        var packageView = instance.GetComponent<PackageView>();
        packageView.Init(package);

        return instance;
    }
}
