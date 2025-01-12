using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public GameObject objectToMove;  // ������, ������� �� ����� ���������� (��������, �����)
    public float gridSize = 1f;      // ������ ����� �����
    public Color gridColor = Color.green; // ���� ����� �����
    public float moveDistance = 5f; // ����������, �� ������� ����� ����� ����� ����

    private bool isMoving = false;   // ���� ��� ��������, � ������ �� �� �����������
    private float floorWidth;        // ������ ����
    private float floorLength;       // ����� ����
    private int gridWidth;           // ���������� ����� �� ������
    private int gridLength;          // ���������� ����� �� �����

    void Start()
    {
        // �������� ������ ���� � ������� ���������� Renderer
        Renderer floorRenderer = GetComponent<Renderer>();
        if (floorRenderer != null)
        {
            floorWidth = floorRenderer.bounds.size.x;
            floorLength = floorRenderer.bounds.size.z;

            // ������������ ���������� �����, ����� ������� ���� ���
            gridWidth = Mathf.CeilToInt(floorWidth / gridSize);
            gridLength = Mathf.CeilToInt(floorLength / gridSize);
        }
        else
        {
            Debug.LogWarning("��� �� ����� Renderer. ���������, ��� ������ ���� ����� ���� ���������.");
        }
    }

    void Update()
    {
        // �������� ��� ��������� ����� ����������� ��� ������� F
        if (Input.GetKeyDown(KeyCode.F))
        {
            isMoving = !isMoving;  // ����������� ��������� �����������
            Debug.Log(isMoving ? "����������� ��������" : "����������� ���������");
        }

        // ���� �� � ������ �����������, ��������� ������� �������
        if (isMoving)
        {
            MoveObjectInFront();
        }
    }

    void MoveObjectInFront()
    {
        // �������� ������� ������
        Vector3 cameraPosition = Camera.main.transform.position;

        // �������� ����������� ������� ������
        Vector3 forwardDirection = Camera.main.transform.forward;

        // ������������ ������� ������� �� �����
        Vector3 targetPosition = cameraPosition + forwardDirection * moveDistance;

        // ������������ ����������� ���������, ����� �� �������� �� ������� ���������� ����
        targetPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, cameraPosition.x - moveDistance, cameraPosition.x + moveDistance),
            0,
            Mathf.Clamp(targetPosition.z, cameraPosition.z - moveDistance, cameraPosition.z + moveDistance)
        );

        // ������� ��������� ����� �� �����
        Vector3 nearestGridPosition = new Vector3(
            Mathf.Round(targetPosition.x / gridSize) * gridSize,
            0,
            Mathf.Round(targetPosition.z / gridSize) * gridSize
        );

        // ���������� ������ � ��������� ������
        objectToMove.transform.position = nearestGridPosition;
    }

    void OnDrawGizmos()
    {
        // ������ ����� ������ ���� �������� �����������
        if (isMoving)
        {
            Gizmos.color = gridColor;

            // ������ ������������ �����
            for (int i = 0; i <= gridWidth; i++)
            {
                Vector3 start = new Vector3(i * gridSize, 0, 0);
                Vector3 end = new Vector3(i * gridSize, 0, gridLength * gridSize);
                Gizmos.DrawLine(start, end);
            }

            // ������ �������������� �����
            for (int i = 0; i <= gridLength; i++)
            {
                Vector3 start = new Vector3(0, 0, i * gridSize);
                Vector3 end = new Vector3(gridWidth * gridSize, 0, i * gridSize);
                Gizmos.DrawLine(start, end);
            }
        }
    }
}
