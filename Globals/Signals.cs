using Godot;

public partial class Signals : Node
{
    public static Signals Instance { get; private set; }

    [Signal]
    public delegate void GivePlayerItemEventHandler(Item item);

    [Signal]
    public delegate void InventoryOpenedEventHandler();

    [Signal]
    public delegate void InventoryClosedEventHandler();

    public override void _Ready()
    {
        Instance = this;
    }

    // Convenience methods for emitting signals
    public void EmitGivePlayerItem(Item item)
    {
        EmitSignal(SignalName.GivePlayerItem, item);
    }

    public void EmitInventoryOpened()
    {
        EmitSignal(SignalName.InventoryOpened);
    }

    public void EmitInventoryClosed()
    {
        EmitSignal(SignalName.InventoryClosed);
    }
}
