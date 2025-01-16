using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int currentExperience = 0;       // ������� ����
    [SerializeField] private int experienceToNextLevel = 100; // ��������� ���� ��� ���������� ������
    [SerializeField] private float experienceGainInterval = 1f; // �������� ���������� ����� (� ��������)

    private float timer = 0f;
    private UIManager uiManager;
    private PlayerManager playerManager;

    void Start()
    {
        uiManager = Object.FindFirstObjectByType<UIManager>();  // ���������� ����� �����
        playerManager = PlayerManager.Instance;

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
            currentExperience -= experienceToNextLevel; // ����� �������� �����
            playerManager.LevelUp();                   // �������� ������� ����� PlayerManager
            experienceToNextLevel += 100;              // ����������� ��������� ���� ��� ���������� ������
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        uiManager?.UpdateLevelUI(playerManager.currentLevel, currentExperience, experienceToNextLevel);
    }
}
