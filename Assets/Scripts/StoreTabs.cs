using UnityEngine;
using UnityEngine.UI;

public class StoreTabs : MonoBehaviour
{
    public GameObject[] tabs; // Массив панелей для вкладок
    public Button[] tabButtons; // Кнопки для вкладок
    private int currentTabIndex = 0; // Индекс текущей вкладки

    void Start()
    {
        // Инициализация вкладок
        UpdateTabDisplay();

        // Добавление обработчиков событий для кнопок вкладок
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int index = i; // Локальная переменная для индекса
            tabButtons[i].onClick.AddListener(() => SwitchTab(index));
        }
    }

    void Update()
    {
        // Переключаем вкладки с помощью клавиши B
        if (Input.GetKeyDown(KeyCode.B))
        {
            currentTabIndex = (currentTabIndex + 1) % tabs.Length; // Переключаем вкладку
            UpdateTabDisplay();
        }
    }

    // Обновление отображения вкладок
    void UpdateTabDisplay()
    {
        // Скрываем все вкладки
        foreach (GameObject tab in tabs)
        {
            tab.SetActive(false);
        }

        // Показываем текущую вкладку
        tabs[currentTabIndex].SetActive(true);
    }

    // Переключение вкладки по кнопке
    void SwitchTab(int index)
    {
        currentTabIndex = index;
        UpdateTabDisplay();
    }
}
