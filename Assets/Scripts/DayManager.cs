using UnityEngine;

public class DayManager : MonoBehaviour
{

    private int currentDay = 0;        // Текущий день
    private float timeInMinutes = 420; // Время в минутах (7:00 = 420 минут)

    private const int startMinutes = 420; // 7:00 = 420 минут с начала дня
    private const int endMinutes = 1380;  // 23:00 = 1380 минут с начала дня
    private const int workingDayDuration = endMinutes - startMinutes; // 960 минут (16 часов)

    private UIManager uiManager;

    void Start()
    {
        uiManager = Object.FindFirstObjectByType<UIManager>(); // Используем метод для поиска UIManager
        UpdateDayAndTimeUI();
    }

    void Update()
    {
        // Увеличиваем время на 1 минуту каждую секунду
        timeInMinutes += Time.deltaTime;

        // Проверяем, завершился ли игровой день
        if (timeInMinutes >= endMinutes)
        {
            timeInMinutes = startMinutes; // Сбрасываем на начало следующего рабочего дня
            currentDay++;                 // Переходим на следующий день
        }

        // Обновляем интерфейс
        UpdateDayAndTimeUI();
    }

    void UpdateDayAndTimeUI()
    {
        // Вычисляем часы и минуты
        int hours = (int)(timeInMinutes / 60);
        int minutes = (int)(timeInMinutes % 60);

        // Передаем данные в UIManager
        uiManager?.UpdateDayUI(currentDay, hours, minutes);
    }
}
