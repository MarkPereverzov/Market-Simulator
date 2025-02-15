using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;
using static UnityEngine.Rendering.ReloadAttribute;
using System;
using Unity.VisualScripting;

public class ShelfView : MonoBehaviour, IInteractable
{
    [SerializeField] public Shelf Shelf { get; set; }

    [Header("Shelf Grid Settings")]
    [SerializeField] public int rows = 3; // ���������� �����
    [SerializeField] private int columns = 4; // ���������� �������
    [SerializeField] private float spacingX = 0.398f; // ���������� ����� ���������
    [SerializeField] private float spacingZ = 0.230f; // ���������� ����� ���������
    [SerializeField] private Vector3 startOffset = new Vector3(-0.4f, 0.35f, -0.35f); // �������� ������ ����� �� ������ �����

    private Transform[,] gridPositions; // ����� �������
    public void Init(Shelf shelf) 
    {
        InitializeGrid();
        Shelf = shelf;
        Shelf.OnShelfCarryProductChanged += Shelf_OnShelfCarryProductChanged;
        Shelf.OnShelfContentChanged += Shelf_OnShelfContentChanged;

        //�������������� ��������
        Shelf_OnShelfCarryProductChanged();
    }
    private void Shelf_OnShelfCarryProductChanged()
    {
        transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = Shelf.CurrentQuantity != 0 ? $"{Shelf.CurrentQuantity}/{Shelf.MaxQuantity}" : "Free";
        transform.GetChild(0).GetComponentInChildren<Image>().sprite = Shelf.Product.productIcon ?? null;
    }
    private void Shelf_OnShelfContentChanged()
    {
        transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = Shelf.CurrentQuantity != 0 ? $"{Shelf.CurrentQuantity}/{Shelf.MaxQuantity}" : "Free";
        UpdateGrid();
    }
    private void InitializeGrid()
    {
        gridPositions = new Transform[columns, rows];
        
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                // ���������� �������
                Vector3 position = transform.position + startOffset +
                                   new Vector3(row * spacingX, 0, col * spacingZ);

                // ������� ������ ������ ��� ����� ����������
                GameObject snapPoint = new GameObject($"SnapPoint_{col}_{row}");
                snapPoint.transform.position = position;
                snapPoint.transform.rotation = Quaternion.Euler(0, 180, 0);
                snapPoint.transform.parent = transform;

                // ��������� ����� � �����
                gridPositions[col, row] = snapPoint.transform;
            }
        }
    }
    public void UpdateGrid()
    {
        int i = 0;
        foreach (ProductView product in Shelf.ProductList)
        {
            int row = i / columns;
            int col = i % columns;

            product.GetComponent<BoxCollider>().enabled = false;
            product.GetComponent<Rigidbody>().isKinematic = true;
            product.transform.parent = transform;

            product.transform.position = gridPositions[col, row].position;
            product.transform.rotation = gridPositions[col, row].rotation;

            i++;
        }
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Interact(PlayerController player)
    {
        Debug.Log("� �� �� ��� ���� ���");
        if (player.CurrentState is HoldingItemState)
        {
            Debug.Log("Unpack");
            CommandManager.Instance.ExecuteCommand(new UnpackCommand(player, this));
        }
        else
        {
            Debug.Log("���� �������");
        }
    }
}
