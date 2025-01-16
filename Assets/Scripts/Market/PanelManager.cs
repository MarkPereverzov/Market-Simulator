using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject marketSystemPanel;
    public GameObject productPanel;
    public GameObject licensesPanel;
    public GameObject improvementsPanel;

    [Header("UI Buttons")]
    public Button licensesButton;
    public Button productsButton;
    public Button improvementsButton;

    private bool isMarketSystemActive = false;
    private GamePauseManager gamePauseManager;

    void Start()
    {
        // Ищем GamePauseManager в сцене
        gamePauseManager = Object.FindFirstObjectByType<GamePauseManager>();

        // Проверяем, был ли найден GamePauseManager
        if (gamePauseManager == null)
        {
            Debug.LogError("GamePauseManager not found in the scene!");
        }

        // Изначально скрываем все панели
        marketSystemPanel.SetActive(false);
        licensesPanel.SetActive(false);
        improvementsPanel.SetActive(false);

        // Назначаем обработчики кнопок
        licensesButton.onClick.AddListener(() => TogglePanel(licensesPanel, productPanel, improvementsPanel));
        productsButton.onClick.AddListener(() => TogglePanel(productPanel, licensesPanel, improvementsPanel));
        improvementsButton.onClick.AddListener(() => TogglePanel(improvementsPanel, productPanel, licensesPanel));

        PlayerManager.Instance.UpdateMoneyUI();
    }

    void Update()
    {
        // Отслеживаем нажатие на клавишу B для открытия/закрытия панели
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleMarketSystemPanel();
        }
    }

    void ToggleMarketSystemPanel()
    {
        isMarketSystemActive = !isMarketSystemActive;

        if (isMarketSystemActive)
        {
            // Открытие панели
            marketSystemPanel.SetActive(true);

            // Включаем паузу через GamePauseManager
            gamePauseManager?.TogglePause();

            // При открытии включаем панель продуктов по умолчанию
            TogglePanel(productPanel, licensesPanel, improvementsPanel);
        }
        else
        {
            // Закрытие панели
            marketSystemPanel.SetActive(false);

            // Отключаем паузу через GamePauseManager
            gamePauseManager?.TogglePause();
        }
    }

    // Переключение видимости панелей
    void TogglePanel(GameObject panelToShow, GameObject panelToHide1, GameObject panelToHide2)
    {
        panelToShow.SetActive(true); // Показываем выбранную панель
        panelToHide1.SetActive(false); // Скрываем остальные панели
        panelToHide2.SetActive(false);
    }
}
