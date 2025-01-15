using UnityEngine;
using TMPro; // ��������� TMP ������������ ����

public class ShelfInteraction : MonoBehaviour
{
    public GameObject shelfItemPrefab; // ������ ������
    public TMP_Text shelfText; // ����� �� ����� (TMP)
    public int itemsOnShelf = 0; // ���������� ������� �� �����

    void Update()
    {
        // ��� ������� �� F ������ �����, ���� ������� �� �����
        if (Input.GetKeyDown(KeyCode.F) && itemsOnShelf > 0)
        {
            // ������� ����� �� �����
            PlaceItemOnShelf();

            // ��������� ���������� ������� �� �������
            itemsOnShelf--;
            shelfText.text = "Items left: " + itemsOnShelf; // TMP �����
        }
    }

    void PlaceItemOnShelf()
    {
        // ������� ����� �� �����
        Instantiate(shelfItemPrefab, transform.position, Quaternion.identity);
        Debug.Log("Item placed on the shelf!");
    }
}
