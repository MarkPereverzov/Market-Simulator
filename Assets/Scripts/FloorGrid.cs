using UnityEngine;

public class FloorGrid : MonoBehaviour
{
    public float gridSize = 1f;      // –азмер €чеек сетки
    public int gridWidth = 10;       // Ўирина сетки (в €чейках)
    public int gridLength = 10;      // ƒлина сетки (в €чейках)
    public Color gridColor = Color.green; // ÷вет линии сетки

    void OnDrawGizmos()
    {
        Gizmos.color = gridColor;

        // –исуем вертикальные линии
        for (int i = 0; i <= gridWidth; i++)
        {
            Vector3 start = new Vector3(i * gridSize, 0, 0);
            Vector3 end = new Vector3(i * gridSize, 0, gridLength * gridSize);
            Gizmos.DrawLine(start, end);
        }

        // –исуем горизонтальные линии
        for (int i = 0; i <= gridLength; i++)
        {
            Vector3 start = new Vector3(0, 0, i * gridSize);
            Vector3 end = new Vector3(gridWidth * gridSize, 0, i * gridSize);
            Gizmos.DrawLine(start, end);
        }
    }
}