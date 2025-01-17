using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;
using static UnityEngine.Rendering.ReloadAttribute;
using System;

public class ShelfView : MonoBehaviour
{
    [SerializeField] public Shelf Shelf { get; private set; }

    //DEBUG ONLY !!!!!
    [Header("Lists")]
    [SerializeField] public ProductList productList;

    [Header("Shelf Grid Settings")]
    [SerializeField] public int rows = 3; // Количество рядов
    [SerializeField] private int columns = 4; // Количество колонок
    [SerializeField] private float spacingX = 0.398f; // Расстояние между объектами
    [SerializeField] private float spacingZ = 0.230f; // Расстояние между объектами
    [SerializeField] private Vector3 startOffset = new Vector3(-0.4f, 0.35f, -0.35f); // Смещение первой точки от центра полки

    private Transform[,] gridPositions; // Сетка позиций
    public void Init(Shelf shelf) 
    {
        InitializeGrid();
        Shelf = shelf;
        Shelf.OnShelfCarryProductChanged += Shelf_OnShelfCarryProductChanged;
        Shelf.OnShelfContentChanged += Shelf_OnShelfContentChanged;
        //Инициализируем значения
        Shelf_OnShelfCarryProductChanged();
    }
    private void Shelf_OnShelfCarryProductChanged()
    {
        transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = Shelf.CurrentQuantity != 0 ? $"{Shelf.CurrentQuantity}/{Shelf.MaxQuantity}" : "Free";
        transform.GetChild(0).GetComponentInChildren<Image>().sprite = Shelf.Product.productIcon ?? null;
    }
    private void Shelf_OnShelfContentChanged()
    {
        UpdateGrid();
    }
    private void InitializeGrid()
    {
        gridPositions = new Transform[columns, rows];
        
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                // Рассчитать позицию
                Vector3 position = transform.position + startOffset +
                                   new Vector3(row * spacingX, 0, col * spacingZ);

                // Создать пустой объект для точки притяжения
                GameObject snapPoint = new GameObject($"SnapPoint_{col}_{row}");
                snapPoint.transform.position = position;
                snapPoint.transform.rotation = Quaternion.Euler(0, 180, 0);
                snapPoint.transform.parent = transform;

                // Сохранить точку в сетке
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

            Debug.Log(row);
            Debug.Log(col);

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
        //TEST
        Product product = productList.products[6];
        Shelf shelf = new Shelf(product);
        Init(shelf);

        for (int i = 0; i < 7; i++)
        {
            ProductView productView = ProductViewManager.Instance.CreateProductView(product);
            Shelf.AddProduct(productView);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
