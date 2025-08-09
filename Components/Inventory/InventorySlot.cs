using Godot;

public partial class InventorySlot : Node
{

    Item Item;
    int Quantity;

    public Item GetItem()
    {
        return Item;
    }

}
