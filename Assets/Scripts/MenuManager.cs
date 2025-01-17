using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject menuPanel;        // �������� ������ ���� (MenuPanel)
    public GameObject buttonsPanel;     // ������ � �������� (Buttons Panel)
    public GameObject settingsPanel;    // ������ �������� (Settings Panel)

    [Header("Menu Buttons")]
    public Button continueButton;       // ������ "Continue"
    public Button settingsButton;       // ������ "Settings"
    public Button exitButton;           // ������ "Exit to Menu"

    private GamePauseManager gamePauseManager;  // ������ �� GamePauseManager

    void Start()
    {
        // ������� GamePauseManager � �����
        gamePauseManager = Object.FindFirstObjectByType<GamePauseManager>();

        // ���������, ��� �� ������ GamePauseManager
        if (gamePauseManager == null)
        {
            Debug.LogError("GamePauseManager not found in the scene!");
        }

        // ��������, ��� MenuPanel � SettingsPanel ������ ��� ������
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        // ��������� ����������� ������� � �������
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(ResumeGame);
        }
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OpenSettings);
        }
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitToMenu);
        }
    }

    void Update()
    {
        // ����������� ������� �� ������� Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // ���������� GamePauseManager ��� ������������ �����
    public void TogglePause()
    {
        if (gamePauseManager != null)
        {
            // ����������� ����� ����� GamePauseManager
            gamePauseManager.TogglePause();

            // �������� ������� ������ �����
            bool isPaused = gamePauseManager.IsPaused();

            // ��������/��������� MenuPanel � ��������/���������� ������
            if (menuPanel != null)
            {
                menuPanel.SetActive(isPaused);
            }

            if (buttonsPanel != null)
            {
                buttonsPanel.SetActive(isPaused);
            }

            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false); // ��� �������� ���� ��������� ������
            }
        }
    }

    // �������� ������ ��������
    public void OpenSettings()
    {
        if (buttonsPanel != null)
        {
            buttonsPanel.SetActive(false); // �������� Buttons Panel
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true); // ���������� Settings Panel
        }
    }

    // ������������� ����
    public void ResumeGame()
    {
        if (gamePauseManager != null)
        {
            gamePauseManager.TogglePause(); // ����������� ����� ����� GamePauseManager
        }

        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
    }

    // ����� � ������� ����
    public void ExitToMenu()
    {
        if (gamePauseManager != null)
        {
            gamePauseManager.TogglePause(); // ������� ����� ����� �������
        }

        Debug.Log("Returning to main menu"); // �������� ��� ������ � ������� ����
        // ����� ����� ��������� ����� � ������� ����, ��������:
        // SceneManager.LoadScene("MainMenu");
    }
}
