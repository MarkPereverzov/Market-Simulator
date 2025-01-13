using UnityEngine;
using TMPro;

public class DayManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI dayText;  // ����� ��� ����������� �������� ���
    public TextMeshProUGUI timeText; // ����� ��� ����������� �������� �������

    private int currentDay = 0;        // ������� ����
    private float timeInMinutes = 420; // ����� � ������� (7:00 = 420 �����)

    private const int startMinutes = 420; // 7:00 = 420 ����� � ������ ���
    private const int endMinutes = 1380;  // 23:00 = 1380 ����� � ������ ���
    private const int workingDayDuration = endMinutes - startMinutes; // 960 ����� (16 �����)

    void Start()
    {
        // ���������� ��������� ���� � �����
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

        // ����������� � ���������� �����
        dayText.text = $"Day {currentDay}";
        timeText.text = $"{hours:00}:{minutes:00}";
    }
}
