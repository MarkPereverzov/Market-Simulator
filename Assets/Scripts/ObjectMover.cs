using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public GameObject objectToMove;  // Объект, который мы будем перемещать (например, касса)
    public float gridSize = 1f;      // Размер ячеек сетки
    public Color gridColor = Color.green; // Цвет линии сетки
    public float moveDistance = 5f; // Расстояние, на котором касса будет перед вами

    private bool isMoving = false;   // Флаг для проверки, в режиме ли мы перемещения
    private float floorWidth;        // Ширина пола
    private float floorLength;       // Длина пола
    private int gridWidth;           // Количество ячеек по ширине
    private int gridLength;          // Количество ячеек по длине

    void Start()
    {
        // Получаем размер пола с помощью компонента Renderer
        Renderer floorRenderer = GetComponent<Renderer>();
        if (floorRenderer != null)
        {
            floorWidth = floorRenderer.bounds.size.x;
            floorLength = floorRenderer.bounds.size.z;

            // Рассчитываем количество ячеек, чтобы покрыть весь пол
            gridWidth = Mathf.CeilToInt(floorWidth / gridSize);
            gridLength = Mathf.CeilToInt(floorLength / gridSize);
        }
        else
        {
            Debug.LogWarning("Пол не имеет Renderer. Убедитесь, что объект пола имеет этот компонент.");
        }
    }

    void Update()
    {
        // Включаем или выключаем режим перемещения при нажатии F
        if (Input.GetKeyDown(KeyCode.F))
        {
            isMoving = !isMoving;  // Переключаем состояние перемещения
            Debug.Log(isMoving ? "Перемещение включено" : "Перемещение выключено");
        }

        // Если мы в режиме перемещения, обновляем позицию объекта
        if (isMoving)
        {
            MoveObjectInFront();
        }
    }

    void MoveObjectInFront()
    {
        // Получаем позицию камеры
        Vector3 cameraPosition = Camera.main.transform.position;

        // Получаем направление взгляда камеры
        Vector3 forwardDirection = Camera.main.transform.forward;

        // Рассчитываем целевую позицию на сетке
        Vector3 targetPosition = cameraPosition + forwardDirection * moveDistance;

        // Ограничиваем перемещение объектами, чтобы не выходить за пределы допустимой зоны
        targetPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, cameraPosition.x - moveDistance, cameraPosition.x + moveDistance),
            0,
            Mathf.Clamp(targetPosition.z, cameraPosition.z - moveDistance, cameraPosition.z + moveDistance)
        );

        // Находим ближайшую точку на сетке
        Vector3 nearestGridPosition = new Vector3(
            Mathf.Round(targetPosition.x / gridSize) * gridSize,
            0,
            Mathf.Round(targetPosition.z / gridSize) * gridSize
        );

        // Перемещаем объект к ближайшей клетке
        objectToMove.transform.position = nearestGridPosition;
    }

    void OnDrawGizmos()
    {
        // Рисуем сетку только если включено перемещение
        if (isMoving)
        {
            Gizmos.color = gridColor;

            // Рисуем вертикальные линии
            for (int i = 0; i <= gridWidth; i++)
            {
                Vector3 start = new Vector3(i * gridSize, 0, 0);
                Vector3 end = new Vector3(i * gridSize, 0, gridLength * gridSize);
                Gizmos.DrawLine(start, end);
            }

            // Рисуем горизонтальные линии
            for (int i = 0; i <= gridLength; i++)
            {
                Vector3 start = new Vector3(0, 0, i * gridSize);
                Vector3 end = new Vector3(gridWidth * gridSize, 0, i * gridSize);
                Gizmos.DrawLine(start, end);
            }
        }
    }
}
