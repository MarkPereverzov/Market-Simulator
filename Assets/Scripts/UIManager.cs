using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Day & Time")]
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI timeText;

    [Header("UI Money")]
    public TextMeshProUGUI moneyText;

    [Header("UI Level & Experience")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI experienceText;
    public Slider experienceBar;

    public void UpdateMoneyUI(float currentMoney)
    {
        moneyText.text = $"Money: ${currentMoney:F2}";
    }

    public void UpdateLevelUI(int currentLevel, int currentExperience = 0, int experienceToNextLevel = 0)
    {
        // Обновляем текст уровня
        levelText.text = $"Market Level {currentLevel}";

        // Обновляем текст опыта в формате "0/100"
        experienceText.text = $"{currentExperience}/{experienceToNextLevel}";

        // Обновляем прогресс-бар
        experienceBar.value = (float)currentExperience / experienceToNextLevel;
    }
    public void UpdateDayUI(int currentDay, int hours, int minutes)
    {
        // Форматируем и отображаем текст
        dayText.text = $"Day {currentDay}";
        timeText.text = $"{hours:00}:{minutes:00}";
    }
}
