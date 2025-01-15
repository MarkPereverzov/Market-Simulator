public class Box
{
    public Product product;  // Продукт, который содержится в коробке
    public int quantity;     // Количество товаров в коробке

    public Box(Product product, int quantity)
    {
        this.product = product;
        this.quantity = quantity;
    }
}
