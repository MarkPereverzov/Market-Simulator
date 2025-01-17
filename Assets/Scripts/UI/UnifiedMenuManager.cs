using UnityEngine;
using UnityEngine.UI;

public class UnifiedMenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject menuPanel;        // Основная панель меню
    public GameObject buttonsPanel;     // Панель с кнопками
    public GameObject settingsPanel;    // Панель настроек

    [Header("Settings Panels")]
    public GameObject videoPanel;       // Панель Video
    public GameObject audioPanel;       // Панель Audio
    public GameObject gamePanel;        // Панель Game

    [Header("Market Panels")]
    public GameObject marketSystemPanel;
    public GameObject productPanel;
    public GameObject licensesPanel;
    public GameObject improvementsPanel;

    [Header("Menu Buttons")]
    public Button continueButton;       // Кнопка "Continue"
    public Button settingsButton;       // Кнопка "Settings"
    public Button exitButton;           // Кнопка "Exit to Menu"

    [Header("Settings Buttons")]
    public Button videoButton;          // Кнопка "Video"
    public Button audioButton;          // Кнопка "Audio"
    public Button gameButton;           // Кнопка "Game"

    [Header("Market Buttons")]
    public Button licensesButton;
    public Button productsButton;
    public Button improvementsButton;

    private bool isMarketSystemActive = false;
    private GamePauseManager gamePauseManager;

    void Start()
    {
        // Находим GamePauseManager в сцене
        gamePauseManager = Object.FindFirstObjectByType<GamePauseManager>();

        if (gamePauseManager == null)
        {
            Debug.LogError("GamePauseManager not found in the scene!");
        }

        // Убедимся, что все панели скрыты при старте
        menuPanel?.SetActive(false);
        settingsPanel?.SetActive(false);
        videoPanel?.SetActive(false);
        audioPanel?.SetActive(false);
        gamePanel?.SetActive(false);

        marketSystemPanel?.SetActive(false);
        productPanel?.SetActive(false);
        licensesPanel?.SetActive(false);
        improvementsPanel?.SetActive(false);

        // Назначаем обработчики кнопок меню
        continueButton?.onClick.AddListener(ResumeGame);
        settingsButton?.onClick.AddListener(OpenSettings);
        exitButton?.onClick.AddListener(ExitToMenu);

        // Назначаем обработчики кнопок настроек
        videoButton?.onClick.AddListener(OpenVideoPanel);
        audioButton?.onClick.AddListener(OpenAudioPanel);
        gameButton?.onClick.AddListener(OpenGamePanel);

        // Назначаем обработчики кнопок маркетинга
        licensesButton?.onClick.AddListener(() => TogglePanel(licensesPanel, productPanel, improvementsPanel));
        productsButton?.onClick.AddListener(() => TogglePanel(productPanel, licensesPanel, improvementsPanel));
        improvementsButton?.onClick.AddListener(() => TogglePanel(improvementsPanel, productPanel, licensesPanel));

        PlayerManager.Instance?.UpdateMoneyUI();
    }

    void Update()
    {
        // Отслеживаем нажатие на клавишу Esc для меню
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMarketSystemActive)
            {
                CloseMarketSystem();
            }
            TogglePause();
        }

        // Отслеживаем нажатие на клавишу B для маркетинга
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!menuPanel.activeSelf)
            {
                ToggleMarketSystemPanel();
            }
        }
    }

    public void TogglePause()
    {
        if (gamePauseManager != null)
        {
            gamePauseManager.TogglePause();
            bool isPaused = gamePauseManager.IsPaused();

            menuPanel?.SetActive(isPaused);
            buttonsPanel?.SetActive(isPaused);
            settingsPanel?.SetActive(false);
            videoPanel?.SetActive(false);
            audioPanel?.SetActive(false);
            gamePanel?.SetActive(false);
        }
    }

    public void OpenSettings()
    {
        buttonsPanel?.SetActive(false);
        settingsPanel?.SetActive(true);
        videoPanel?.SetActive(false);
        audioPanel?.SetActive(false);
        gamePanel?.SetActive(false);
    }

    public void OpenVideoPanel()
    {
        videoPanel?.SetActive(true);
        audioPanel?.SetActive(false);
        gamePanel?.SetActive(false);
    }

    public void OpenAudioPanel()
    {
        videoPanel?.SetActive(false);
        audioPanel?.SetActive(true);
        gamePanel?.SetActive(false);
    }

    public void OpenGamePanel()
    {
        videoPanel?.SetActive(false);
        audioPanel?.SetActive(false);
        gamePanel?.SetActive(true);
    }

    public void ResumeGame()
    {
        if (gamePauseManager != null) gamePauseManager.TogglePause();
        menuPanel?.SetActive(false);
    }

    public void ExitToMenu()
    {
        if (gamePauseManager != null) gamePauseManager.TogglePause();
        Debug.Log("Returning to main menu");
        // SceneManager.LoadScene("MainMenu"); // Реальная загрузка главного меню
    }

    void ToggleMarketSystemPanel()
    {
        isMarketSystemActive = !isMarketSystemActive;

        if (isMarketSystemActive)
        {
            marketSystemPanel?.SetActive(true);
            gamePauseManager?.TogglePause();
            TogglePanel(productPanel, licensesPanel, improvementsPanel);
        }
        else
        {
            CloseMarketSystem();
        }
    }

    void CloseMarketSystem()
    {
        isMarketSystemActive = false;
        marketSystemPanel?.SetActive(false);
        gamePauseManager?.TogglePause();
    }

    void TogglePanel(GameObject panelToShow, GameObject panelToHide1, GameObject panelToHide2)
    {
        panelToShow?.SetActive(true);
        panelToHide1?.SetActive(false);
        panelToHide2?.SetActive(false);
    }
}
