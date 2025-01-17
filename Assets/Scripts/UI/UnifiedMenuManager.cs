using UnityEngine;
using UnityEngine.UI;

public class UnifiedMenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject menuPanel;        // �������� ������ ����
    public GameObject buttonsPanel;     // ������ � ��������
    public GameObject settingsPanel;    // ������ ��������

    [Header("Settings Panels")]
    public GameObject videoPanel;       // ������ Video
    public GameObject audioPanel;       // ������ Audio
    public GameObject gamePanel;        // ������ Game

    [Header("Market Panels")]
    public GameObject marketSystemPanel;
    public GameObject productPanel;
    public GameObject licensesPanel;
    public GameObject improvementsPanel;

    [Header("Menu Buttons")]
    public Button continueButton;       // ������ "Continue"
    public Button settingsButton;       // ������ "Settings"
    public Button exitButton;           // ������ "Exit to Menu"

    [Header("Settings Buttons")]
    public Button videoButton;          // ������ "Video"
    public Button audioButton;          // ������ "Audio"
    public Button gameButton;           // ������ "Game"

    [Header("Market Buttons")]
    public Button licensesButton;
    public Button productsButton;
    public Button improvementsButton;

    private bool isMarketSystemActive = false;
    private GamePauseManager gamePauseManager;

    void Start()
    {
        // ������� GamePauseManager � �����
        gamePauseManager = Object.FindFirstObjectByType<GamePauseManager>();

        if (gamePauseManager == null)
        {
            Debug.LogError("GamePauseManager not found in the scene!");
        }

        // ��������, ��� ��� ������ ������ ��� ������
        menuPanel?.SetActive(false);
        settingsPanel?.SetActive(false);
        videoPanel?.SetActive(false);
        audioPanel?.SetActive(false);
        gamePanel?.SetActive(false);

        marketSystemPanel?.SetActive(false);
        productPanel?.SetActive(false);
        licensesPanel?.SetActive(false);
        improvementsPanel?.SetActive(false);

        // ��������� ����������� ������ ����
        continueButton?.onClick.AddListener(ResumeGame);
        settingsButton?.onClick.AddListener(OpenSettings);
        exitButton?.onClick.AddListener(ExitToMenu);

        // ��������� ����������� ������ ��������
        videoButton?.onClick.AddListener(OpenVideoPanel);
        audioButton?.onClick.AddListener(OpenAudioPanel);
        gameButton?.onClick.AddListener(OpenGamePanel);

        // ��������� ����������� ������ ����������
        licensesButton?.onClick.AddListener(() => TogglePanel(licensesPanel, productPanel, improvementsPanel));
        productsButton?.onClick.AddListener(() => TogglePanel(productPanel, licensesPanel, improvementsPanel));
        improvementsButton?.onClick.AddListener(() => TogglePanel(improvementsPanel, productPanel, licensesPanel));

        PlayerManager.Instance?.UpdateMoneyUI();
    }

    void Update()
    {
        // ����������� ������� �� ������� Esc ��� ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMarketSystemActive)
            {
                CloseMarketSystem();
            }
            TogglePause();
        }

        // ����������� ������� �� ������� B ��� ����������
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
        // SceneManager.LoadScene("MainMenu"); // �������� �������� �������� ����
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
