using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab; // ������ �������
    public Transform spawnArea;       // �������, ��� ������� ����� ����������
    public float floorWidth = 10f;    // ������ ����
    public float floorLength = 10f;   // ����� ����
    public string storeSize = "small"; // ������ ��������

    private int maxCustomers;          // ������������ ���������� ��������
    private int currentCustomerCount = 0;

    void Start()
    {
        // ������������� ������������ ���������� �������� � ����������� �� ������� ��������
        switch (storeSize.ToLower())
        {
            case "small":
                maxCustomers = 5;
                break;
            case "medium":
                maxCustomers = 10;
                break;
            case "large":
                maxCustomers = 20;
                break;
            case "extralarge":
                maxCustomers = 35;
                break;
            default:
                Debug.LogWarning("Invalid store size. Defaulting to 'medium'.");
                maxCustomers = 10;
                break;
        }

        // ��������� ����� ��������
        InvokeRepeating(nameof(SpawnCustomer), 0f, 1f); // ����� �������� ������ 1 �������
    }

    private void SpawnCustomer()
    {
        if (currentCustomerCount < maxCustomers)
        {
            // ��������� ��������� ������� ��� ������ �������
            Vector3 randomPosition = new Vector3(
                Random.Range(-floorWidth / 2, floorWidth / 2), // ��������� ������� �� ��� X
                1f, // ������
                Random.Range(-floorLength / 2, floorLength / 2) // ��������� ������� �� ��� Z
            );

            // ������� ������ �������
            GameObject newCustomer = Instantiate(customerPrefab, spawnArea.position + randomPosition, Quaternion.identity);

            // �������� ��������� CustomerMovement � �������� ��������� ����
            CustomerMovement movement = newCustomer.GetComponent<CustomerMovement>();
            if (movement != null)
            {
                movement.areaWidth = floorWidth;   // �������� ������ ����
                movement.areaLength = floorLength; // �������� ����� ����
            }

            currentCustomerCount++;
        }
    }
}
