using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab; // Префаб клиента
    public Transform spawnArea;       // Область, где клиенты будут спавниться
    public float floorWidth = 10f;    // Ширина пола
    public float floorLength = 10f;   // Длина пола
    public string storeSize = "small"; // Размер магазина

    private int maxCustomers;          // Максимальное количество клиентов
    private int currentCustomerCount = 0;

    void Start()
    {
        // Устанавливаем максимальное количество клиентов в зависимости от размера магазина
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

        // Запускаем спавн клиентов
        InvokeRepeating(nameof(SpawnCustomer), 0f, 1f); // Спавн клиентов каждые 1 секунду
    }

    private void SpawnCustomer()
    {
        if (currentCustomerCount < maxCustomers)
        {
            // Генерация случайной позиции для спавна клиента
            Vector3 randomPosition = new Vector3(
                Random.Range(-floorWidth / 2, floorWidth / 2), // Случайная позиция по оси X
                1f, // Высота
                Random.Range(-floorLength / 2, floorLength / 2) // Случайная позиция по оси Z
            );

            // Создаем нового клиента
            GameObject newCustomer = Instantiate(customerPrefab, spawnArea.position + randomPosition, Quaternion.identity);

            // Получаем компонент CustomerMovement и передаем параметры пола
            CustomerMovement movement = newCustomer.GetComponent<CustomerMovement>();
            if (movement != null)
            {
                movement.areaWidth = floorWidth;   // Передаем ширину пола
                movement.areaLength = floorLength; // Передаем длину пола
            }

            currentCustomerCount++;
        }
    }
}
