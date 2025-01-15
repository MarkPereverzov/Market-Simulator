using UnityEngine;
using TMPro; // Добавляем TMP пространство имен

public class ShelfInteraction : MonoBehaviour
{
    public GameObject shelfItemPrefab; // Префаб товара
    public TMP_Text shelfText; // Текст на полке (TMP)
    public int itemsOnShelf = 0; // Количество товаров на полке

    void Update()
    {
        // При нажатии на F создаём товар, если коробка не пуста
        if (Input.GetKeyDown(KeyCode.F) && itemsOnShelf > 0)
        {
            // Создаем товар на полке
            PlaceItemOnShelf();

            // Уменьшаем количество товаров на коробке
            itemsOnShelf--;
            shelfText.text = "Items left: " + itemsOnShelf; // TMP текст
        }
    }

    void PlaceItemOnShelf()
    {
        // Создаем товар на полке
        Instantiate(shelfItemPrefab, transform.position, Quaternion.identity);
        Debug.Log("Item placed on the shelf!");
    }
}
