using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("User")]
    public float currentMoney = 1000f;

    [Header("Player Level")]
    public int currentLevel = 1;  // Начальный уровень игрока

    private UIManager uiManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        uiManager = Object.FindFirstObjectByType<UIManager>(); // Используем новый метод для поиска UIManager
    }

    public void LevelUp()
    {
        currentLevel++;
        uiManager?.UpdateLevelUI(currentLevel); // Обновляем только уровень в UI
    }

    public void UpdateMoneyUI()
    {
        uiManager?.UpdateMoneyUI(currentMoney);
    }

    public bool SpendMoney(float amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateMoneyUI();
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough money!");
            return false;
        }
    }

    public float GetCurrentMoney()
    {
        return currentMoney;
    }
}
