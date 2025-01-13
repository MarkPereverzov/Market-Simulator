using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelProgressBarTMP : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI levelText;       // TMP ����� ��� ������
    [SerializeField] public TextMeshProUGUI experienceText;  // TMP ����� ��� ����� (� ������� 0/100)
    [SerializeField] public Slider experienceBar;            // ��������-��� ��� �����

    [SerializeField] public int currentLevel = 1;            // ��������� �������
    [SerializeField] public int currentExperience = 0;       // ������� ����
    [SerializeField] public int experienceToNextLevel = 100; // ��������� ���� ��� ���������� ������
    [SerializeField] public float experienceGainInterval = 1f; // �������� ���������� ����� (� ��������)

    private float timer = 0f; // ������ ��� ������������ ���������

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= experienceGainInterval)
        {
            timer = 0f;
            AddExperience(1);
        }
    }

    private void AddExperience(int amount)
    {
        currentExperience += amount;

        if (currentExperience >= experienceToNextLevel)
        {
            currentExperience -= experienceToNextLevel; // ����� �����
            LevelUp();
        }

        UpdateUI();
    }

    private void LevelUp()
    {
        currentLevel++;
        experienceToNextLevel += 100; // ���������� ���������� � �����
    }

    private void UpdateUI()
    {
        // ��������� ����� ������
        levelText.text = $"Market Level {currentLevel}";

        // ��������� ����� ����� � ������� "0/100"
        experienceText.text = $"{currentExperience}/{experienceToNextLevel}";

        // ��������� ��������-���
        experienceBar.value = (float)currentExperience / experienceToNextLevel;
    }
}
