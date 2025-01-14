using UnityEngine;

public class FloorGrid : MonoBehaviour
{
    public float gridSize = 1f;      // ������ ����� �����
    public int gridWidth = 10;       // ������ ����� (� �������)
    public int gridLength = 10;      // ����� ����� (� �������)
    public Color gridColor = Color.green; // ���� ����� �����

    void OnDrawGizmos()
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