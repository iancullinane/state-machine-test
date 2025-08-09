using System;
using Godot;

public partial class Inventory : Panel
{
    GridContainer _container;
    InventorySlot[] _slots;

    public override void _Ready()
    {
        _container = GetNode<GridContainer>("GridContainer");

        _slots = new InventorySlot[_container.GetChildCount()];
        for (int i = 0; i < _container.GetChildCount(); i++)
        {
            _slots[i] = _container.GetChild(i) as InventorySlot;
        }

        // Connect to GlobalSignals
        Signals.Instance.GivePlayerItem += OnGivePlayerItem;



    }

    private void OnGivePlayerItem(Item item)
    {
        GD.Print("GivePlayerItem signal received: " + item.Name);
    }


    public InventorySlot GetSlot(Item item)
    {
        foreach (InventorySlot slot in _slots)
        {
            if (slot.GetItem() == item)
            {
                return slot;
            }
        }
        return null;
    }
}
