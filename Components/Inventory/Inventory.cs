using System;
using Godot;

public partial class Inventory : Panel
{
    GridContainer _container;
    Slot[] _slots;

    public override void _Ready()
    {
        _container = GetNode<GridContainer>("GridContainer");

        _slots = new Slot[_container.GetChildCount()];
        for (int i = 0; i < _container.GetChildCount(); i++)
        {
            _slots[i] = _container.GetChild(i) as Slot;
        }

        // Connect to GlobalSignals
        Signals.Instance.GivePlayerItem += OnGivePlayerItem;



    }

    private void OnGivePlayerItem(Item item)
    {
        GD.Print("GivePlayerItem signal received: " + item.Name);
        Slot activeSlot = GetSlot(item);
        if (activeSlot != null)
        {
            activeSlot.SetItem(item);
        }
    }


    public Slot GetSlot(Item item)
    {
        foreach (Slot slot in _slots)
        {
            if (slot.GetItem() == item)
            {
                return slot;
            }
        }
        return null;
    }
}
