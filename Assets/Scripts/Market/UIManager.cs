using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Text")]
    public TextMeshProUGUI moneyText;       // Текст для отображения текущих денег

    private float currentMoney = 100f;      // Стартовая сумма денег

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
    }

    // Обновляем отображение денег
    public void UpdateMoneyUI()
    {
        moneyText.text = $"Money: ${currentMoney:F2}";
    }

    // Метод для изменения текущего баланса
    public void ChangeMoney(float amount)
    {
        currentMoney += amount;
        UpdateMoneyUI();
    }

    // Метод для вычитания денег (используется в ProductManager)
    public bool SpendMoney(float amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateMoneyUI();
            return true;  // Успешное списание денег
        }
        else
        {
            Debug.LogWarning("Not enough money!");
            return false;  // Недостаточно денег
        }
    }


    // Получить текущий баланс
    public float GetCurrentMoney()
    {
        return currentMoney;
    }
}
