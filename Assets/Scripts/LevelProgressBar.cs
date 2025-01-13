using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelProgressBarTMP : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI levelText;       // TMP текст для уровня
    [SerializeField] public TextMeshProUGUI experienceText;  // TMP текст для опыта (в формате 0/100)
    [SerializeField] public Slider experienceBar;            // Прогресс-бар для опыта

    [SerializeField] public int currentLevel = 1;            // Начальный уровень
    [SerializeField] public int currentExperience = 0;       // Текущий опыт
    [SerializeField] public int experienceToNextLevel = 100; // Требуемый опыт для следующего уровня
    [SerializeField] public float experienceGainInterval = 1f; // Интервал добавления опыта (в секундах)

    private float timer = 0f; // Таймер для отслеживания интервала

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
            currentExperience -= experienceToNextLevel; // Сброс опыта
            LevelUp();
        }

        UpdateUI();
    }

    private void LevelUp()
    {
        currentLevel++;
        experienceToNextLevel += 100; // Увеличение требования к опыту
    }

    private void UpdateUI()
    {
        // Обновляем текст уровня
        levelText.text = $"Market Level {currentLevel}";

        // Обновляем текст опыта в формате "0/100"
        experienceText.text = $"{currentExperience}/{experienceToNextLevel}";

        // Обновляем прогресс-бар
        experienceBar.value = (float)currentExperience / experienceToNextLevel;
    }
}
