using UnityEngine;

public class DayManager : MonoBehaviour
{

    private int currentDay = 0;        // ������� ����
    private float timeInMinutes = 420; // ����� � ������� (7:00 = 420 �����)

    private const int startMinutes = 420; // 7:00 = 420 ����� � ������ ���
    private const int endMinutes = 1380;  // 23:00 = 1380 ����� � ������ ���
    private const int workingDayDuration = endMinutes - startMinutes; // 960 ����� (16 �����)

    private UIManager uiManager;

    void Start()
    {
        uiManager = Object.FindFirstObjectByType<UIManager>(); // ���������� ����� ��� ������ UIManager
        UpdateDayAndTimeUI();
    }

    void Update()
    {
        // ����������� ����� �� 1 ������ ������ �������
        timeInMinutes += Time.deltaTime;

        // ���������, ���������� �� ������� ����
        if (timeInMinutes >= endMinutes)
        {
            timeInMinutes = startMinutes; // ���������� �� ������ ���������� �������� ���
            currentDay++;                 // ��������� �� ��������� ����
        }

        // ��������� ���������
        UpdateDayAndTimeUI();
    }

    void UpdateDayAndTimeUI()
    {
        // ��������� ���� � ������
        int hours = (int)(timeInMinutes / 60);
        int minutes = (int)(timeInMinutes % 60);

        // �������� ������ � UIManager
        uiManager?.UpdateDayUI(currentDay, hours, minutes);
    }
}
