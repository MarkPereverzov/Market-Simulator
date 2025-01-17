using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class Command
{
    public abstract void Execute();
    public abstract void Undo();
}

public class PickUpCommand : Command
{
    private PlayerController playerController;
    private GameObject itemObject;
    public PickUpCommand(PlayerController playerController, GameObject itemObject) 
    {
        this.playerController = playerController;
        this.itemObject = itemObject;
    }

    public override void Execute()
    {
        playerController.SetState(new HoldingItemState(playerController));
        playerController.HoldItem = itemObject;
        itemObject.transform.SetParent(playerController.handPosition);
        itemObject.transform.localPosition = Vector3.zero;
        itemObject.GetComponent<Rigidbody>().isKinematic = true;
        itemObject.transform.rotation = Quaternion.identity;
    }
    public override void Undo()
    {
        itemObject.transform.parent = null;
        playerController.HoldItem = null;
        itemObject.GetComponent<Rigidbody>().isKinematic = false;
        itemObject.transform.rotation = Quaternion.identity;
        playerController.SetState(new FreeState(playerController));
    }
}
public class DropCommand : Command
{
    private PlayerController playerController;
    private GameObject itemObject;

    public DropCommand(PlayerController playerController, GameObject itemObject)
    {
        this.playerController = playerController;
        this.itemObject = itemObject;
    }

    public override void Execute()
    {
        playerController.HoldItem = null;
        // Сбрасываем предмет из рук
        itemObject.transform.SetParent(null);  // Убираем объект из рук
        itemObject.GetComponent<Rigidbody>().isKinematic = false;  // Разрешаем физику
        itemObject.GetComponent<Rigidbody>().AddForce(playerController.transform.forward * 10f, ForceMode.Impulse);  // Применяем силу, чтобы "выбросить" предмет
        playerController.SetState(new FreeState(playerController));  // Возвращаем игрока в состояние "свободен"
    }

    public override void Undo()
    {
        playerController.HoldItem = itemObject;
        // Возвращаем предмет в руки игрока
        itemObject.transform.SetParent(playerController.handPosition);
        itemObject.transform.localPosition = Vector3.zero;
        itemObject.GetComponent<Rigidbody>().isKinematic = true;
        itemObject.transform.rotation = Quaternion.identity;
        playerController.SetState(new HoldingItemState(playerController));  // Устанавливаем состояние "держит предмет"
    }
}
public class UnpackCommand : Command
{
    private PlayerController playerController;
    private ShelfView shelfView;
    private Package package;
    public UnpackCommand(PlayerController playerController, ShelfView shelfView)
    {
        this.playerController = playerController;
        this.shelfView = shelfView;
    }

    public override void Execute()
    {
        PackageView packageView = playerController.HoldItem.GetComponent<PackageView>();
        package = packageView.Package;

        if(shelfView.Shelf == null)
            shelfView.Init(new Shelf(package.Product));
        Shelf shelf = shelfView.Shelf;

        if (shelf.Product.productName != package.Product.productName) return;

        while (package.Quantity > 0 && shelf.CurrentQuantity != shelf.MaxQuantity)
        {
            package.Quantity -= 1;
            ProductView productView = ProductViewManager.Instance.CreateProductView(package.Product);
            shelf.AddProduct(productView);
        }
        if (package.Quantity == 0)
        {
            CommandManager.Instance.ExecuteCommand(new DropCommand(playerController, packageView.gameObject));
            packageView.DestroyPackage();
        }
    }
    public override void Undo()
    {
       
    }
}