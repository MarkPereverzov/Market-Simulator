using UnityEngine;

public class PackageViewManager : MonoBehaviour
{
    [SerializeField] private GameObject packageViewPrefab;
    [SerializeField] private Transform boxSpawnPoint;

    public void CreatePackageView(Package package)
    {
        var instance = Instantiate(packageViewPrefab, boxSpawnPoint.position, Quaternion.identity);
        var packageView = instance.GetComponent<PackageView>();
        packageView.Init(package);
    }
}
