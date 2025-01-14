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

    void Start()
    {
        marketSystemPanel.SetActive(false);
        licensesPanel.SetActive(false);
        improvementsPanel.SetActive(false);

        licensesButton.onClick.AddListener(() => TogglePanel(licensesPanel, productPanel, improvementsPanel));
        productsButton.onClick.AddListener(() => TogglePanel(productPanel, licensesPanel, improvementsPanel));
        improvementsButton.onClick.AddListener(() => TogglePanel(improvementsPanel, productPanel, licensesPanel));

        UIManager.Instance.UpdateMoneyUI();  // Используем обновленный метод для отображения баланса
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleMarketSystemPanel();
        }
    }

    void ToggleMarketSystemPanel()
    {
        isMarketSystemActive = !isMarketSystemActive;
        marketSystemPanel.SetActive(isMarketSystemActive);

        if (isMarketSystemActive)
        {
            TogglePanel(productPanel, licensesPanel, improvementsPanel);
        }
        else
        {
            marketSystemPanel.SetActive(false);
        }
    }

    // Переключение между панелями
    void TogglePanel(GameObject panelToShow, GameObject panelToHide1, GameObject panelToHide2)
    {
        panelToShow.SetActive(true);  // Показываем нужную панель
        panelToHide1.SetActive(false); // Скрываем другую панель
        panelToHide2.SetActive(false); // Скрываем третью панель
    }
}
