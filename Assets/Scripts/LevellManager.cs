using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int currentExperience = 0;       // Текущий опыт
    [SerializeField] private int experienceToNextLevel = 100; // Требуемый опыт для следующего уровня
    [SerializeField] private float experienceGainInterval = 1f; // Интервал добавления опыта (в секундах)

    private float timer = 0f;
    private UIManager uiManager;
    private PlayerManager playerManager;

    void Start()
    {
        uiManager = Object.FindFirstObjectByType<UIManager>();  // Используем новый метод
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
            currentExperience -= experienceToNextLevel; // Сброс текущего опыта
            playerManager.LevelUp();                   // Повышаем уровень через PlayerManager
            experienceToNextLevel += 100;              // Увеличиваем требуемый опыт для следующего уровня
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        uiManager?.UpdateLevelUI(playerManager.currentLevel, currentExperience, experienceToNextLevel);
    }
}
